using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Splines;

//作成者:杉山
//でか生き物が道をどく演出
//TODO:タイムライン化させる

public class BigCreatureMoveOut : MonoBehaviour
{
    [SerializeField]
    PlayableDirector _moveOutDirecter;

    [SerializeField]
    Animator _bigCreatureAnimator;

    [SerializeField]
    SplineAnimate _splineAnimate;

    public void WakeUp()
    {
        _bigCreatureAnimator.SetTrigger(BigCreatureAnimatorProperty.MoveOutTriggerName);
    }

    public void Fly()
    {
        _splineAnimate.Play();
    }

    public async UniTask MoveOutAsync(CancellationToken ct)
    {
        _moveOutDirecter.Play();

        await _moveOutDirecter.WaitForStoppedAsync(ct);
    }
}
