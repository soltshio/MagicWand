using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;

//作成者:杉山
//炎魔法の内容

public class MagicContentTypeSun : MagicContentTypeBase
{
    [SerializeField]
    BigCreatureReactionManager _bigCreature;

    [SerializeField]
    WaitUntilAllFinishTasksEventDirecter _sunEffectDirecter;

    [Tooltip("日向の効果音が入ったAudioSource")] [SerializeField]
    AudioSource _sunAudioSource;

    [SerializeField]
    AudioClip _sunSE;

    [SerializeField]
    LensFlareActivator _sunLensActivator;

    [SerializeField]
    ParticleSystem _sunParticle;

    [SerializeField]
    SendManager _sendManager;

    public void SunLensActivate()
    {
        _sunLensActivator.ActivateAsync().Forget();
        _sunAudioSource.PlayOneShot(_sunSE);
        _sunParticle.Play();
    }

    public void SunLensDeactivate()
    {
        _sunLensActivator.DeactivateAsync().Forget();
        _sunParticle.Stop();
    }

    //SignalReceiverであるタイミングで一度タイムラインを一時停止させる(他のオブジェクトへの影響処理が終わればまた再生させる)
    public void PauseTimelineForAffectFieldObjects()
    {
        _sunEffectDirecter.PauseUntilAllFinishTasksAsync().Forget();
    }

    public void AffectToBigCreature()
    {
        //でか生物に魔法を当てる
        _sunEffectDirecter.AddTasks(_bigCreature.TakeMagicAsync(EMagic.Sun));
    }

    //太陽のステートに入るようマイコンに指示を送る
    public void SendChangeToSunCommandToMicon()
    {
        _sendManager.AddMessage(EMiconCommand.ChangeToSun);
    }

    //太陽の効果音を鳴らすようマイコンに指示を送る
    public void SendPlaySunSoundCommandToMicon()
    {
        _sendManager.AddMessage(EMiconCommand.PlaySunSound);
    }

    //太陽のステートを終わるようマイコンに指示を送る
    public void SendFinishSunCommandToMicon()
    {
        _sendManager.AddMessage(EMiconCommand.FinishSun);
    }

    public override async UniTask ActivateAsync(CancellationToken ct)
    {
        _sunEffectDirecter.ClearTasks();

        await _sunEffectDirecter.StartPlayingAndWaitUntilFinishPlayingAsync(ct);
    }
}
