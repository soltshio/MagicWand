using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//魔法球の誘導演出

public partial class MagicSphereLeadEffectController : MonoBehaviour
{
    [SerializeField]
    LeadEffectController _leadEffectController;

    [SerializeField]
    float _activeDuration=1f;

    [SerializeField]
    float _deactiveDuration = 0.2f;

    [SerializeField]
    LeadMagicSphereColorController _leadMagicSphereColorController;

    [SerializeField]
    MagicSpheresList _magicSpheresList;

    [SerializeField]
    SpellCastList _spellCastList;

    [SerializeField]
    AudioSource _leadEffectAudioSource;

    [SerializeField]
    AudioClip _leadSE;

    SingleTaskCancellation _singleTaskCancellation = new();

    public async UniTask ActiveLeadAsync(int? preActiveSphereIndex, List<(EMagic magic, int index)> activeSphereIndex_MagicList)
    {
        //誘導エフェクト効果音を流し始める
        PlaySound();

        var newCt = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        _leadEffectController.InitLeadEffect(preActiveSphereIndex, activeSphereIndex_MagicList,_activeDuration);

        ProgressTimer progressTimer = new(_activeDuration);

        //魔法球に色をつける
        _leadMagicSphereColorController.PaintMagicColorToMagicSphere(activeSphereIndex_MagicList);

        while (!progressTimer.IsFinished)
        {
            progressTimer.Tick();

            float progress = progressTimer.CalcProgress();

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: newCt);
        }
    }

    public async UniTask DeactiveLeadAsync()
    {
        var newCt = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        //全ての魔法球を元に戻す
        _leadMagicSphereColorController.PaintDefaultColorToAllMagicSphere();

        await UniTask.Delay(TimeSpan.FromSeconds(_deactiveDuration), cancellationToken: newCt);
    }

    void Awake()
    {
        _leadMagicSphereColorController.Awake(_magicSpheresList, _spellCastList);
        _leadEffectController.Awake(_magicSpheresList, _spellCastList);
    }

    public void PlaySound()
    {
        _leadEffectAudioSource.PlayOneShot(_leadSE);
    }
}
