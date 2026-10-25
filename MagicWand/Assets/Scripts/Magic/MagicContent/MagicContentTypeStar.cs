using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//星魔法の内容

public class MagicContentTypeStar : MagicContentTypeBase
{
    [SerializeField]
    BigCreatureReactionManager _bigCreature;

    [SerializeField]
    WaitUntilAllFinishTasksEventDirecter _starEffectDirecter;

    [SerializeField]
    SendManager _sendManager;

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

    //星のステートに入るようマイコンに指示を送る
    public void SendChangeToStarCommandToMicon()
    {
        _sendManager.AddMessage(EMiconCommand.ChangeToStar);
    }

    //星の効果音を鳴らすようマイコンに指示を送る
    public void SendPlayStarSoundCommandToMicon()
    {
        _sendManager.AddMessage(EMiconCommand.PlayStarSound);
    }

    //星のステートを終わるようマイコンに指示を送る
    public void SendFinishStarCommandToMicon()
    {
        _sendManager.AddMessage(EMiconCommand.FinishStar);
    }

    public override async UniTask ActivateAsync(CancellationToken ct)
    {
        _starEffectDirecter.ClearTasks();

        await _starEffectDirecter.StartPlayingAndWaitUntilFinishPlayingAsync(ct);
    }
}
