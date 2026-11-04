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
    BigCreatureReactionManager _bigCreature;

    [SerializeField]
    WaitUntilAllFinishTasksEventDirecter _thunderEffectDirecter;

    [SerializeField]
    LensFlareActivator _lensFlareActivator;

    SendManager _sendManagerInstance;

    async void Start()
    {
        await UniTask.WaitUntil(() => SendManager.Instance != null, cancellationToken: this.GetCancellationTokenOnDestroy());

        _sendManagerInstance = SendManager.Instance;
    }

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

    //太陽の光をつける
    public void ActivateLensFlare()
    {
        _lensFlareActivator.ActivateAsync().Forget();
    }

    //太陽の光を消す
    public void DeactivateLensFlare()
    {
        _lensFlareActivator.DeactivateAsync().Forget();
    }

    //雷の効果音を鳴らすようマイコンに指示を送る
    public void SendPlayThunderSoundCommandToMicon()
    {
        if(_sendManagerInstance == null) return;
        _sendManagerInstance.AddMessage(EMiconCommand.InvokeThunder_PlaySound);
    }

    //雷のステートを終わるようマイコンに指示を送る
    public void SendFinishThunderCommandToMicon()
    {
        if(_sendManagerInstance == null) return;
        _sendManagerInstance.AddMessage(EMiconCommand.FinishThunder);
    }

    public override async UniTask ActivateAsync(CancellationToken ct)
    {
        _thunderEffectDirecter.ClearTasks();

        await _thunderEffectDirecter.StartPlayingAndWaitUntilFinishPlayingAsync(ct);
    }
}
