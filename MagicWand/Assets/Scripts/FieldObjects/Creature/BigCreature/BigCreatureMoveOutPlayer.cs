using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Splines;

//作成者:杉山
//巨大生物が道をどく演出

public class BigCreatureMoveOutPlayer : BigCreatureReactionTypeBase
{
    [SerializeField]
    PlayableDirector _moveOutDirecter;

    [SerializeField]
    AnimatorReference _bigCreatureAnimator;

    [SerializeField]
    SplineAnimate _splineAnimate;

    void Start()
    {
        _splineAnimate.enabled = false;
    }

    public void WakeUp()
    {
        _bigCreatureAnimator.Animator.SetTrigger(BigCreatureAnimatorProperty.MoveOutTriggerName);
    }

    public void Fly()
    {
        _splineAnimate.enabled = true;
        _splineAnimate.Play();
    }

    public override async UniTask TakeReactionAsync()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _moveOutDirecter.Play();

        await _moveOutDirecter.WaitForStoppedAsync(ct);
    }
}
