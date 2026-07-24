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
    GroundGrassAlphaController _groundGrassAlphaController;

    [Tooltip("草の量の変化量(0を最低値、1を最大値として設定する)")] [SerializeField] [Range(0, 1)]
    float _alphaDeltaRate;

    [SerializeField]
    float _shiftGrassAmountDuration;

    [Header("でか生き物関係")]

    [SerializeField]
    BigCreature _bigCreature;

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

    public void AffectToGroundGrass()
    {
        //地面に草を生やす
        float newAlphaRate = CalcNewGrassAlpha();
        _timeEffectDirecter.AddTasks(_groundGrassAlphaController.SetGrassAlphaAsync(newAlphaRate, _shiftGrassAmountDuration));
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

    float CalcNewGrassAlpha()
    {
        float newAlphaRate = _groundGrassAlphaController.CurrentAlphaRate + _alphaDeltaRate;

        return Mathf.Clamp01(newAlphaRate);
    }
}
