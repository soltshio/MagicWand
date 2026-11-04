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

    SendManager _sendManagerInstance;

    async void Start()
    {
        await UniTask.WaitUntil(() => SendManager.Instance != null, cancellationToken: this.GetCancellationTokenOnDestroy());

        _sendManagerInstance = SendManager.Instance;
    }

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

    //星の効果音を鳴らすようマイコンに指示を送る
    public void SendPlayStarSoundCommandToMicon()
    {
        if(_sendManagerInstance == null) return;
        _sendManagerInstance.AddMessage(EMiconCommand.InvokeStar_PlaySound);
    }

    //星のステートを終わるようマイコンに指示を送る
    public void SendFinishStarCommandToMicon()
    {
        if(_sendManagerInstance == null) return;
        _sendManagerInstance.AddMessage(EMiconCommand.FinishStar);
    }

    public override async UniTask ActivateAsync(CancellationToken ct)
    {
        _starEffectDirecter.ClearTasks();

        await _starEffectDirecter.StartPlayingAndWaitUntilFinishPlayingAsync(ct);
    }
}
