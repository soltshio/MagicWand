using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;

//作成者:杉山
//巨大生物が二度寝をする演出

public class BigCreatureGoBackToSleepPlayer : BigCreatureReactionTypeBase
{
    [SerializeField]
    PlayableDirector _goBackToSleepDirecter;

    //巨大生物のモーションを二度寝から普段のモーションに戻す

    public override async UniTask TakeReactionAsync()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _goBackToSleepDirecter.Play();

        await _goBackToSleepDirecter.WaitForStoppedAsync(ct);
    }
}
