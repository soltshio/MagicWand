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

    [SerializeField]
    AnimatorReference _bigCreatureAnimator;

    //巨大生物に二度寝のモーションをさせる
    public void PlayBigCreatureGoBackToSleepMotion()
    {
        _bigCreatureAnimator.Animator.SetBool(BigCreatureAnimatorProperty.GoBackToSleepBoolName, true);
    }

    public override async UniTask TakeReactionAsync()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _goBackToSleepDirecter.Play();

        await _goBackToSleepDirecter.WaitForStoppedAsync(ct);
    }
}
