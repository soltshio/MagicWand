using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//雨魔法の内容

public class MagicContentTypeRain : MagicContentTypeBase
{
    [SerializeField]
    BigCreature _bigCreature;

    public override async UniTask ActivateAsync(CancellationToken token)
    {
        List<UniTask> runningTasks = new();

        //でか生物に雷魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Thunder));

        await UniTask.WhenAll(runningTasks);
    }
}
