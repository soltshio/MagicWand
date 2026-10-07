using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;

//作成者:杉山
//でか生き物が道をどく演出
//TODO:タイムライン化させる

public class BigCreatureMoveOut : MonoBehaviour
{
    [SerializeField]
    PlayableDirector _moveOutDirecter;

    public async UniTask WalkAsync(CancellationToken ct)
    {
        _moveOutDirecter.Play();

        await _moveOutDirecter.WaitForStoppedAsync(ct);
    }
}
