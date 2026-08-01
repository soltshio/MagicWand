using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//魔法球の誘導演出

[System.Serializable]
public class MagicSphereLeadEffect
{
    [SerializeField]
    GameObject _leadEffectPrefab;

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

    MagicSpheresList _magicSpheresList;

    Dictionary<EMagic, SpellCast> _spellCastsDictionary;

    MagicSphereMaterialController[] _activedSphereMaterialControllers;

    public void Awake(MagicSpheresList magicSpheresList, Dictionary<EMagic, SpellCast> spellCastsDictionary)
    {
        _magicSpheresList = magicSpheresList;
        _spellCastsDictionary = spellCastsDictionary;
    }

    public async UniTask ActiveLeadAsync(int? preActiveSphereIndex, List<(EMagic magic, int index)> activeSphereIndex_MagicList,CancellationToken ct)
    {
        //誘導エフェクトの初期化
        //＝＞誘導エフェクトのプレハブを作成
        //＝＞誘導エフェクトのスタート地点と目的地点を設定

        //色をつける予定の魔法球のマテリアル操作用のコンポーネントを取得


        ProgressTimer progressTimer = new(_activeDuration);

        while(!progressTimer.IsFinished)
        {
            progressTimer.Tick();

            //誘導エフェクトを動かす

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }



        //魔法球に色をつける
    }

    public async UniTask DeactiveLeadAsync(CancellationToken ct)
    {
        //色がついた魔法球を元に戻す
    }
}
