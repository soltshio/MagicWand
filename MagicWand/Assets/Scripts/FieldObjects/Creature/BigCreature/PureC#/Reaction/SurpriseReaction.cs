using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

//作成者:杉山
//ビックリした反応の演出

public class SurpriseReaction : MonoBehaviour
{
    [Tooltip("!の文字アニメーションを表示し始めてから非表示にし始めるまでに待つ時間")] [SerializeField]
    float _waitDurationFromStartActiveToStartDeactive=1.5f;

    [SerializeField]
    Animator _surpriseTextAnimator;

    [SerializeField]
    TextMeshProUGUI _surpriseText;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    AudioClip _surpriseSE;

    public async UniTask TakeSurpriseReactionAsync(CancellationToken ct)
    {
        PlayAudio(_surpriseSE);

        _surpriseText.enabled = true;
        _surpriseTextAnimator.SetTrigger(SurpriseTextAnimatorProperty.ActiveTriggerName);

        await UniTask.Delay(TimeSpan.FromSeconds(_waitDurationFromStartActiveToStartDeactive), cancellationToken: ct);

        _surpriseTextAnimator.SetTrigger(SurpriseTextAnimatorProperty.DeactiveTriggerName);

        //現在の非表示アニメーションが終わるまで待つ
        await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);//すぐに状態が切り替わるとは限らないため1フレーム待機

        float length = _surpriseTextAnimator.GetCurrentAnimatorStateInfo(0).length;
        await UniTask.Delay(TimeSpan.FromSeconds(length), cancellationToken: ct);

        _surpriseText.enabled = false;
        _audioSource.Stop();
    }

    void Start()
    {
        _surpriseText.enabled = false;
    }

    void PlayAudio(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
