using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//魔法球の誘導演出

public class MagicSphereLeadEffectController : MonoBehaviour
{
    [SerializeField]
    LeadEffect _leadEffectPrefab;

    [Tooltip("魔法陣の中心位置")] [SerializeField]
    Transform _magicCircleCenter;

    [SerializeField]
    float _leadEffectLifeTime=7f;

    [SerializeField]
    float _activeDuration=1f;

    [SerializeField]
    float _deactiveDuration = 0.2f;

    [SerializeField]
    Color _deactiveColor;

    [SerializeField]
    MagicSpheresList _magicSpheresList;

    [SerializeField]
    SerializableDictionary<EMagic, SpellCast> _spellCastsDictionary;

    public async UniTask ActiveLeadAsync(int? preActiveSphereIndex, List<(EMagic magic, int index)> activeSphereIndex_MagicList)
    {
        CancellationToken ct = this.GetCancellationTokenOnDestroy();

        //誘導エフェクトのスタート地点を求める
        Vector3 start = CalcStartPos(preActiveSphereIndex);

        //誘導エフェクトの初期化
        LeadEffect[] leadEffects = InitLeadEffect(activeSphereIndex_MagicList, start);

        ProgressTimer progressTimer = new(_activeDuration);

        while(!progressTimer.IsFinished)
        {
            progressTimer.Tick();

            float progress = progressTimer.CalcProgress();

            //誘導エフェクトを動かす
            ControlLeadEffectsPos(leadEffects, progress);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }

        //魔法球に色をつける
        var magicSphereMaterialControllers = _magicSpheresList.GetComponentsArrayFromMagicSpheres<MagicSphereMaterialController>();

        for(int i=0; i< activeSphereIndex_MagicList.Count; i++)
        {
            var index = activeSphereIndex_MagicList[i].index;
            var magic = activeSphereIndex_MagicList[i].magic;

            //色を取得
            if (!_spellCastsDictionary.TryGetValue(magic, out var spellCast)) continue;

            Color sphereColor = spellCast.MagicSphereColor;

            //色を変える球のマテリアルコントローラーを取得
            var magicSphereMaterialController = _magicSpheresList.GetComponentFromMagicSphere<MagicSphereMaterialController>(index);

            if (magicSphereMaterialController == null) continue;

            magicSphereMaterialController.SetColor(sphereColor);
        }

    }

    public async UniTask DeactiveLeadAsync()
    {
        CancellationToken ct = this.GetCancellationTokenOnDestroy();

        await UniTask.Delay(TimeSpan.FromSeconds(_deactiveDuration), cancellationToken: ct);

        //全ての魔法球を元に戻す
        var magicSphereMaterialControllers = _magicSpheresList.GetComponentsArrayFromMagicSpheres<MagicSphereMaterialController>();

        for(int i=0; i<magicSphereMaterialControllers.Length ;i++)
        {
            var matController = magicSphereMaterialControllers[i];

            if (matController == null) continue;

            matController.SetColor(_deactiveColor);
        }
    }

    Vector3 CalcStartPos(int? preActiveSphereIndex)
    {
        if (preActiveSphereIndex == null)
        {
            return _magicCircleCenter.position;
        }
        else
        {
            return _magicSpheresList.MagicSphereObjects[(int)preActiveSphereIndex].transform.position;
        }
    }

    Vector3 CalcEndPos(int nextActiveSphereIndex)
    {
        return _magicSpheresList.MagicSphereObjects[nextActiveSphereIndex].transform.position;
    }

    LeadEffect[] InitLeadEffect(List<(EMagic magic, int index)> activeSphereIndex_MagicList,Vector3 start)
    {
        LeadEffect[] leadEffects = new LeadEffect[activeSphereIndex_MagicList.Count];

        for (int i = 0; i < activeSphereIndex_MagicList.Count; i++)
        {
            //終点を求める
            Vector3 end = CalcEndPos(activeSphereIndex_MagicList[i].index);

            var leadEffectInstance = Instantiate(_leadEffectPrefab);
            leadEffectInstance.Initialize(start, end);
            Destroy(leadEffectInstance, _leadEffectLifeTime);

            leadEffects[i] = leadEffectInstance;
        }

        return leadEffects;
    }

    void ControlLeadEffectsPos(LeadEffect[] leadEffects,float progress)
    {
        for(int i=0; i<leadEffects.Length ;i++)
        {
            leadEffects[i].SetPos(progress);
        }
    }
}
