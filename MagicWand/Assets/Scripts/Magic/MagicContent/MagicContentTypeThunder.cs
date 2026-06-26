using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Cinemachine;
using UnityEngine;

//作成者:杉山
//雷魔法の発動内容

public class MagicContentTypeThunder : MagicContentTypeBase
{
    [SerializeField]
    BigCreature _bigCreature;

    [Tooltip("雷雲の演出")] [SerializeField]
    ThunderEventPlayer _thunderEventPlayer;

    [Tooltip("演出が終わってから他のものに魔法の影響を与えるまでの遅延時間")] [SerializeField]
    float _delayDurationMagicAffection;

    public override async UniTask ActivateAsync(CancellationToken token)
    {
        //雷雲が発生し、雷が落ちる演出
        await _thunderEventPlayer.CauseThunderEventAsync();

        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationMagicAffection), cancellationToken: token);

        List<UniTask> runningTasks = new();

        //ここから魔法の影響を近くの物に与える

        //でか生物に魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Thunder));

        await UniTask.WhenAll(runningTasks);
    }
}
