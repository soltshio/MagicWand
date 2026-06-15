using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//雨魔法の内容

public class MagicContentTypeRain : MagicContentTypeBase
{
    [SerializeField]
    BigCreature _bigCreature;

    [SerializeField]
    ParticleSystem _rainParticle;

    [Tooltip("雨の効果音が入ったAudioSource")] [SerializeField]
    AudioSource _audioRainSource;

    [Tooltip("魔法の影響を与えるまでに遅らせる時間")] [SerializeField]
    float _delayDurationAffection = 2f;

    public override async UniTask ActivateAsync(CancellationToken token)
    {
        //雨のエフェクトを表示する
        _rainParticle.Play();

        //雨の効果音を鳴らし始める
        _audioRainSource.Play();

        List<UniTask> runningTasks = new();

        //雨エフェクトが出て少し遅らせてから他のものに魔法の影響を与える
        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationAffection), cancellationToken: token);

        //でか生物に魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Rain));

        await UniTask.WhenAll(runningTasks);

        //雨のエフェクトを非表示にする
        _rainParticle.Stop();

        //雨の効果音をストップする
        _audioRainSource.Stop();
    }
}
