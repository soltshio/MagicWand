using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//雨魔法の内容

public class MagicContentTypeRain : MagicContentTypeBase
{
    public override async UniTask ActivateAsync(CancellationToken token)
    {
        await UniTask.Yield(token);
    }
}
