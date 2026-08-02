using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

//作成者:杉山
//巨大生物のビックリした反応の演出

public class SurpriseReaction : MonoBehaviour
{
    [SerializeField]
    PlayableDirector _surpriseReactionDirector;

    [SerializeField]
    Animator _bigCreatureAnimator;

    //巨大生物に二度寝のモーションをさせる
    public void PlayBigCreatureGoBackToSleepMotion()
    {
        _bigCreatureAnimator.SetBool(BigCreatureAnimatorProperty.GoBackToSleepBoolName,true);
    }

    public async UniTask TakeSurpriseReactionAsync(CancellationToken ct)
    {
        _surpriseReactionDirector.Play();

        //タイムラインの再生が終わるまで待つ
        await _surpriseReactionDirector.WaitForStoppedAsync(ct);

        //巨大生物に元のモーションをさせる
        _bigCreatureAnimator.SetBool(BigCreatureAnimatorProperty.GoBackToSleepBoolName, false);
    }
}
