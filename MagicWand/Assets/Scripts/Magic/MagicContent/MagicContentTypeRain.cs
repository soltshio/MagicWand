using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//雨魔法の内容

public class MagicContentTypeRain : MagicContentTypeBase
{
    [SerializeField]
    BigCreature _bigCreature;

    [SerializeField]
    WaitUntilAllFinishTasksEventDirecter _rainEffectDirecter;

    [SerializeField]
    ParticleSystem _rainParticle;

    [Tooltip("雨の効果音が入ったAudioSource")] [SerializeField]
    AudioSource _audioRainSource;

    public void StartRaining()
    {
        _rainParticle.Play();
        _audioRainSource.Play();
    }

    public void StopRaining()
    {
        _rainParticle.Stop();
        _audioRainSource.Stop();
    }

    //SignalReceiverであるタイミングで一度タイムラインを一時停止させる(他のオブジェクトへの影響処理が終わればまた再生させる)
    public void PauseTimelineForAffectFieldObjects()
    {
        _rainEffectDirecter.PauseUntilAllFinishTasksAsync().Forget();
    }

    public void AffectToBigCreature()
    {
        //でか生物に魔法を当てる
        _rainEffectDirecter.AddTasks(_bigCreature.TakeMagicAsync(EMagic.Rain));
    }

    public override async UniTask ActivateAsync(CancellationToken ct)
    {
        _rainEffectDirecter.ClearTasks();

        await _rainEffectDirecter.StartPlayingAndWaitUntilFinishPlayingAsync(ct);
    }
}
