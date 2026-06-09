using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//魔法内容

public abstract class MagicContentTypeBase : MonoBehaviour
{
    public abstract UniTask ActivateAsync(CancellationToken token);
}
