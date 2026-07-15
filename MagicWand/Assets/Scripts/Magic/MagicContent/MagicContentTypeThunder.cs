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
    PlayableDirector _thunderEffectDirecter;

    List<UniTask> runningTasks = new();

    //SignalReceiverであるタイミングで一度タイムラインを一時停止させる(他のオブジェクトへの影響処理が終わればまた再生させる)
    public void PauseTimelineForAffectFieldObjects()
    {
        AffectEventAsync().Forget();
    }

    public override async UniTask ActivateAsync(CancellationToken token)
    {
        runningTasks.Clear();

        _thunderEffectDirecter.Play();

        //タイムラインの再生が終わるまで待つ
        await _thunderEffectDirecter.WaitForStoppedAsync(this.GetCancellationTokenOnDestroy());
    }

    //一旦タイムラインを一時停止し、他のオブジェクトへの影響処理が終わるのを待ってからまた再生させる
    async UniTask AffectEventAsync()
    {
        _thunderEffectDirecter.Pause();

        //でか生物に魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Thunder));
        await UniTask.WhenAll(runningTasks);

        _thunderEffectDirecter.Play();
    }
}
