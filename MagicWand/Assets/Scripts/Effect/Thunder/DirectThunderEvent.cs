using Cysharp.Threading.Tasks;
using System;
using Unity.Cinemachine;
using UnityEngine;

//作成者:杉山
//目の前に落ちる雷の演出

public class DirectThunderEvent : MonoBehaviour
{
    [Tooltip("雷が落ちるまで待つ時間")] [SerializeField]
    float _delayDurationToLightningStrike;

    [SerializeField]
    ParticleSystem _thunderEffect;

    [SerializeField]
    AudioSource _audioSource;

    [Tooltip("目の前に落ちる雷の効果音")] [SerializeField]
    AudioClip _directThunderSE;

    [Tooltip("カメラの揺れ")] [SerializeField]
    CinemachineImpulseSource _impulseSource;

    public void CauseDirectThunder()
    {
        CauseDirectThunderAsync().Forget();
    }

    async UniTask CauseDirectThunderAsync()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _thunderEffect.Play();

        //少し待ってから雷が地面に落ちた時の演出を入れる
        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationToLightningStrike), cancellationToken: ct);

        _audioSource.PlayOneShot(_directThunderSE);
        _impulseSource.GenerateImpulse();
    }
}
