using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;

//作成者:杉山
//巨大生物のビックリした反応の演出

public class SurpriseReaction : MonoBehaviour
{
    [SerializeField]
    PlayableDirector _surprise_StillSleeping_ReactionDirector;

    [SerializeField]
    PlayableDirector _surprise_WakeUp_ReactionDirector;

    [SerializeField]
    Animator _bigCreatureAnimator;

    //巨大生物に二度寝のモーションをさせる
    public void PlayBigCreatureGoBackToSleepMotion()
    {
        _bigCreatureAnimator.SetBool(BigCreatureAnimatorProperty.GoBackToSleepBoolName,true);
    }

    public async UniTask TakeSurpriseReactionAsync(BigCreatureStatus updatedBigCreatureStatus,CancellationToken ct)
    {
        PlayableDirector _surpriseReactionDirecter;

        //巨大生物が起きているかによって流すタイムラインを変える
        if(updatedBigCreatureStatus.IsWakeUp)
        {
            _surpriseReactionDirecter = _surprise_WakeUp_ReactionDirector;
        }
        else
        {
            _surpriseReactionDirecter = _surprise_StillSleeping_ReactionDirector;
        }

        _surpriseReactionDirecter.Play();

        //タイムラインの再生が終わるまで待つ
        await _surpriseReactionDirecter.WaitForStoppedAsync(ct);

        //巨大生物に元のモーションをさせる
        _bigCreatureAnimator.SetBool(BigCreatureAnimatorProperty.GoBackToSleepBoolName, false);
    }
}
