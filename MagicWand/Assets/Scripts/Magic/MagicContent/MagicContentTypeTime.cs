using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//時間魔法の内容

public class MagicContentTypeTime : MagicContentTypeBase
{
    [SerializeField]
    WaitUntilAllFinishTasksEventDirecter _timeEffectDirecter;

    [Header("地面関係")]

    [SerializeField]
    GroundGrassSetter _groundGrassSetter;

    [Header("でか生き物関係")]

    [SerializeField]
    BigCreatureReactionManager _bigCreature;

    [Header("草関係")]

    [SerializeField]
    GrassesGrowth _grassesGrowth;

    [Header("魔法のエフェクト関係")]

    [Tooltip("時計の効果音が入ったAudioSource")] [SerializeField]
    AudioSource _clockAudioSource;

    [SerializeField]
    ClockEffectActivator _clockEffectActivator;

    //SignalReceiverであるタイミングで一度タイムラインを一時停止させる(他のオブジェクトへの影響処理が終わればまた再生させる)
    public void PauseTimelineForAffectFieldObjects()
    {
        _timeEffectDirecter.PauseUntilAllFinishTasksAsync().Forget();
    }

    public void AffectToBigCreature()
    {
        //でか生物に魔法を当てる
        _timeEffectDirecter.AddTasks(_bigCreature.TakeMagicAsync(EMagic.Time));
    }

    public void AffectToGrasses()
    {
        //草を成長させる
        _timeEffectDirecter.AddTasks(_grassesGrowth.TakeMagicAsync(EMagic.Rain));
    }

    public void AffectToGroundGrass()
    {
        //地面に草を生やす
        _timeEffectDirecter.AddTasks(_groundGrassSetter.GrowGrassOnGroundAsync());
    }

    public void ActivateClockEffect()
    {
        _clockEffectActivator.ActivateAsync().Forget();
        _clockAudioSource.Play();
    }

    public void DeactivateClockEffect()
    {
        _clockEffectActivator.DeactivateAsync().Forget();
        _clockAudioSource.Stop();
    }

    public override async UniTask ActivateAsync(CancellationToken ct)
    {
        _timeEffectDirecter.ClearTasks();

        await _timeEffectDirecter.StartPlayingAndWaitUntilFinishPlayingAsync(ct);
    }
}
