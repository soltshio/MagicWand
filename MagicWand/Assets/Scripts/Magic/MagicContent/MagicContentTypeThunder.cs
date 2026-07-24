using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

//作成者:杉山
//雷魔法の発動内容

public class MagicContentTypeThunder : MagicContentTypeBase
{
    [SerializeField]
    BigCreature _bigCreature;

    [SerializeField]
    WaitUntilAllFinishTasksEventDirecter _thunderEffectDirecter;

    //SignalReceiverであるタイミングで一度タイムラインを一時停止させる(他のオブジェクトへの影響処理が終わればまた再生させる)
    public void PauseTimelineForAffectFieldObjects()
    {
        _thunderEffectDirecter.PauseUntilAllFinishTasksAsync().Forget();
    }

    public void AffectToBigCreature()
    {
        //でか生物に魔法を当てる
        _thunderEffectDirecter.AddTasks(_bigCreature.TakeMagicAsync(EMagic.Thunder));
    }

    public override async UniTask ActivateAsync(CancellationToken ct)
    {
        _thunderEffectDirecter.ClearTasks();

        await _thunderEffectDirecter.StartPlayingAndWaitUntilFinishPlayingAsync(ct);
    }
}
