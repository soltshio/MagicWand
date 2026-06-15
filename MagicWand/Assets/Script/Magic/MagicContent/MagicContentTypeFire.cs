using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//炎魔法の内容

public class MagicContentTypeFire : MagicContentTypeBase
{
    [SerializeField]
    BigCreature _bigCreature;

    [Tooltip("日向の効果音が入ったAudioSource")] [SerializeField]
    AudioSource _audioSunSource;

    [Tooltip("魔法の影響を与えるまでに遅らせる時間")] [SerializeField]
    float _delayDurationAffection = 2f;

    public override async UniTask ActivateAsync(CancellationToken token)
    {
        //日向の効果音を鳴らし始める
        _audioSunSource.Play();

        List<UniTask> runningTasks = new();

        //エフェクトが出て少し遅らせてから他のものに魔法の影響を与える
        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationAffection), cancellationToken: token);

        //でか生物に魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Fire));

        await UniTask.WhenAll(runningTasks);

        _audioSunSource.Stop();
    }
}
