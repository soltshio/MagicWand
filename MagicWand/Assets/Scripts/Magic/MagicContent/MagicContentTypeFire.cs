using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;

//作成者:杉山
//炎魔法の内容

public class MagicContentTypeFire : MagicContentTypeBase
{
    [SerializeField]
    BigCreature _bigCreature;

    [SerializeField]
    PlayableDirector _sunEffectDirecter;

    [Tooltip("日向の効果音が入ったAudioSource")] [SerializeField]
    AudioSource _sunAudioSource;

    [SerializeField]
    AudioClip _sunSE;

    [SerializeField]
    SunLensActivator _sunLensActivator;

    List<UniTask> runningTasks = new();

    public void SunLensActivate()
    {
        _sunLensActivator.ActivateAsync().Forget();
        _sunAudioSource.PlayOneShot(_sunSE);
    }

    public void SunLensDeactivate()
    {
        _sunLensActivator.DeactivateAsync().Forget();
    }

    //SignalReceiverであるタイミングで一度タイムラインを一時停止させる(他のオブジェクトへの影響処理が終わればまた再生させる)
    public void PauseTimelineForAffectFieldObjects()
    {
        AffectEventAsync().Forget();
    }

    public override async UniTask ActivateAsync(CancellationToken token)
    {
        runningTasks.Clear();

        _sunEffectDirecter.Play();

        //タイムラインの再生が終わるまで待つ
        await _sunEffectDirecter.WaitForStoppedAsync(this.GetCancellationTokenOnDestroy());
    }

    //一旦タイムラインを一時停止し、他のオブジェクトへの影響処理が終わるのを待ってからまた再生させる
    async UniTask AffectEventAsync()
    {
        _sunEffectDirecter.Pause();

        //でか生物に魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Thunder));
        await UniTask.WhenAll(runningTasks);

        _sunEffectDirecter.Play();
    }
}
