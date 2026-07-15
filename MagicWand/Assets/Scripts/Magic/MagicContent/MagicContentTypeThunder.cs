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

    //SignalReceiverであるタイミングで一度タイムラインを一時停止させる(他のオブジェクトへの影響処理が終わればまた再生させる)
    public void PauseTimelineForAffectFieldObjects()
    {
        _thunderEffectDirecter.Pause();
    }

    public override async UniTask ActivateAsync(CancellationToken token)
    {
        _thunderEffectDirecter.Play();

        //一旦一時停止になるまで待つ
        await UniTask.WaitUntil(() => _thunderEffectDirecter.state == PlayState.Paused, cancellationToken: token);

        //ここから魔法の影響を近くの物に与える
        await AffectToAround();

        //また再生させて今度は終わるまで待つ
        _thunderEffectDirecter.Play();
        await _thunderEffectDirecter.WaitForStoppedAsync(token);
    }

    async UniTask AffectToAround()
    {
        List<UniTask> runningTasks = new();

        //でか生物に魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Thunder));

        await UniTask.WhenAll(runningTasks);
    }
}
