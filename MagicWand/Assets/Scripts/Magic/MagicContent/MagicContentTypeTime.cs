using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//時間魔法の内容

public class MagicContentTypeTime : MagicContentTypeBase
{
    [Header("地面関係")]
    
    [SerializeField]
    GroundGrassAlphaController _groundGrassAlphaController;

    [SerializeField]
    float _alphaDelta;

    [SerializeField]
    float _shiftGrassAmountDuration;

    [Header("でか生き物関係")]

    [SerializeField]
    BigCreature _bigCreature;

    [Header("魔法のエフェクト関係")]

    [Tooltip("時計の効果音が入ったAudioSource")] [SerializeField]
    AudioSource _clockAudioSource;

    [Tooltip("魔法の影響を与えるまでに遅らせる時間")] [SerializeField]
    float _delayDurationAffection = 2f;

    [SerializeField]
    ClockEffectActivator _clockEffectActivator;

    public override async UniTask ActivateAsync(CancellationToken token)
    {
        _clockAudioSource.Play();

        //時計のエフェクトを表示させる
        _clockEffectActivator.ActivateAsync().Forget();

        //エフェクトが出て少し遅らせてから他のものに魔法の影響を与える
        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationAffection), cancellationToken: token);

        await AffectToAround();

        //時計のエフェクトを非表示にさせる
        _clockEffectActivator.DeactivateAsync().Forget();

        _clockAudioSource.Stop();
    }

    async UniTask AffectToAround()
    {
        List<UniTask> runningTasks = new();

        //でか生物に魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Time));

        //地面に草を生やす
        float newAlpha = CalcNewGrassAlpha();
        runningTasks.Add(_groundGrassAlphaController.SetGrassAlphaAsync(newAlpha, _shiftGrassAmountDuration));

        await UniTask.WhenAll(runningTasks);
    }

    float CalcNewGrassAlpha()
    {
        float newAlpha = _groundGrassAlphaController.CurrentAlpha + _alphaDelta;

        return Mathf.Clamp(newAlpha, _groundGrassAlphaController.MinAlpha, _groundGrassAlphaController.MaxAlpha);
    }
}
