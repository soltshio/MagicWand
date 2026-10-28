using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//雨魔法の内容

public class MagicContentTypeRain : MagicContentTypeBase
{
    [SerializeField]
    BigCreatureReactionManager _bigCreature;

    [SerializeField]
    GrassesGrowth _grassesGrowth;

    [SerializeField]
    WaitUntilAllFinishTasksEventDirecter _rainEffectDirecter;

    [SerializeField]
    ParticleSystem _rainParticle;

    [Tooltip("雨の効果音が入ったAudioSource")] [SerializeField]
    AudioSource _audioRainSource;

    [SerializeField]
    SendManager _sendManager;

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

    public void AffectToGrasses()
    {
        //草を成長させる
        _rainEffectDirecter.AddTasks(_grassesGrowth.TakeMagicAsync(EMagic.Rain));
    }

    //水のステートに入るようマイコンに指示を送る
    public void SendChangeToWaterCommandToMicon()
    {
        _sendManager.AddMessage(EMiconCommand.StartWater);
    }

    //水の効果音を鳴らすようマイコンに指示を送る
    public void SendPlayWaterSoundCommandToMicon()
    {
        _sendManager.AddMessage(EMiconCommand.InvokeWater_PlaySound);
    }

    //水のステートを終わるようマイコンに指示を送る
    public void SendFinishWaterCommandToMicon()
    {
        _sendManager.AddMessage(EMiconCommand.FinishWater);
    }

    public override async UniTask ActivateAsync(CancellationToken ct)
    {
        _rainEffectDirecter.ClearTasks();

        await _rainEffectDirecter.StartPlayingAndWaitUntilFinishPlayingAsync(ct);
    }
}
