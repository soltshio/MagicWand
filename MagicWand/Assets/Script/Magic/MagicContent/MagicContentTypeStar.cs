using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//星魔法の内容

public class MagicContentTypeStar : MagicContentTypeBase
{
    public override async UniTask ActivateAsync(CancellationToken token)
    {
        await UniTask.Yield(token);
    }
}
