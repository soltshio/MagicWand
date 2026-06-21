using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//時間魔法の内容

public class MagicContentTypeTime : MagicContentTypeBase
{
    [SerializeField]
    BigCreature _bigCreature;

    [Tooltip("時計の効果音が入ったAudioSource")] [SerializeField]
    AudioSource _clockAudioSource;

    [Tooltip("魔法の影響を与えるまでに遅らせる時間")] [SerializeField]
    float _delayDurationAffection = 2f;

    [SerializeField]
    ClockEffectActivator _clockEffectActivator;

    public override async UniTask ActivateAsync(CancellationToken token)
    {
        _clockAudioSource.Play();

        List<UniTask> runningTasks = new();

        //時計のエフェクトを表示させる
        _clockEffectActivator.ActivateAsync().Forget();

        //エフェクトが出て少し遅らせてから他のものに魔法の影響を与える
        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationAffection), cancellationToken: token);

        //でか生物に魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Time));

        await UniTask.WhenAll(runningTasks);

        //時計のエフェクトを非表示にさせる
        _clockEffectActivator.DeactivateAsync().Forget();

        _clockAudioSource.Stop();
    }
}
