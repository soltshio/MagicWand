using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//時間魔法の内容

public class MagicContentTypeTime : MagicContentTypeBase
{
    public override async UniTask ActivateAsync(CancellationToken token)
    {
        await UniTask.Yield(token);
    }
}
