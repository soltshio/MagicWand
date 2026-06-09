using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//炎魔法の内容

public class MagicContentTypeFire : MagicContentTypeBase
{
    public override async UniTask ActivateAsync(CancellationToken token)
    {
        await UniTask.Yield(token);
    }
}
