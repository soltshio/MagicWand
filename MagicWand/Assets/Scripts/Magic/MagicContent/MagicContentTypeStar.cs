using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//星魔法の内容

public class MagicContentTypeStar : MagicContentTypeBase
{
    [SerializeField]
    BigCreature _bigCreature;

    [SerializeField]
    WaitUntilAllFinishTasksEventDirecter _starEffectDirecter;

    //SignalReceiverであるタイミングで一度タイムラインを一時停止させる(他のオブジェクトへの影響処理が終わればまた再生させる)
    public void PauseTimelineForAffectFieldObjects()
    {
        _starEffectDirecter.PauseUntilAllFinishTasksAsync().Forget();
    }

    public void AffectToBigCreature()
    {
        //でか生物に魔法を当てる
        _starEffectDirecter.AddTasks(_bigCreature.TakeMagicAsync(EMagic.Star));
    }

    public override async UniTask ActivateAsync(CancellationToken ct)
    {
        _starEffectDirecter.ClearTasks();

        await _starEffectDirecter.StartPlayingAndWaitUntilFinishPlayingAsync(ct);
    }
}
