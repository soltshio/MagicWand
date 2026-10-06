using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

//作成者:杉山
//レンズフレアのエフェクトの表示・非表示を行う機能

public class LensFlareActivator : MonoBehaviour
{
    [Tooltip("何秒かけてレンズフレアの表示・非表示を行うか")] [SerializeField]
    float _shiftDuration = 1.5f;

    [Tooltip("初期状態のintensity")] [SerializeField]
    float _defaultIntensity = 0f;

    [Tooltip("非表示時のintensity")] [SerializeField]
    float _offIntensity = 0f;

    [Tooltip("表示時のintensity")] [SerializeField]
    float _onIntensity = 0.5f;

    [SerializeField]
    LensFlareComponentSRP _lensFlare;

    public async UniTask ActivateAsync()
    {
        await ShiftIntensityAsync(this.GetCancellationTokenOnDestroy(), _offIntensity, _onIntensity, _shiftDuration);
    }

    public async UniTask DeactivateAsync()
    {
        await ShiftIntensityAsync(this.GetCancellationTokenOnDestroy(), _onIntensity, _offIntensity, _shiftDuration);
    }

    void Start()
    {
        _lensFlare.intensity = _defaultIntensity;
    }

    async UniTask ShiftIntensityAsync(CancellationToken ct,float fromIntensity,float toIntensity,float duration)
    {
        float elapsed = 0f;

        while(true)
        {
            elapsed += Time.deltaTime;

            float rate = elapsed / duration;
            float newIntensity = Mathf.Lerp(fromIntensity, toIntensity, rate);

            _lensFlare.intensity = newIntensity;

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);

            if (elapsed > duration) break;
        }

        _lensFlare.intensity = toIntensity;
    }
}
