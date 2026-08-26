using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//作成者:杉山
//カーソルのトレイルエフェクトの制御をする
//杖が一度離れてから触れた際にトレイルが途切れるようにすること

public class CursorTrailEffectController : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _trailEffect;

    [SerializeField]
    float _replayEffectWaitDuration = 0.2f;

    HokuyoDataReceiver _hokuyoDataReceiver;

    SingleTaskCancellation _singleTaskCancellation = new();

    async void OnEnable()
    {
        _hokuyoDataReceiver = await HokuyoDataReceiver.GetInstanceAsync(this.GetCancellationTokenOnDestroy());

        _hokuyoDataReceiver.OnSwitchIsExistObject += ResetEffectOnEnterObjectInHokuyoDetect;
    }

    void ResetEffectOnEnterObjectInHokuyoDetect(bool isExistObject)
    {
        if (!isExistObject) return;

        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        RemoveTrailAsync(ct).Forget();
    }

    async UniTask RemoveTrailAsync(CancellationToken ct)
    {
        _trailEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        //ほんの一瞬だけ待ってからエフェクトを再生し始める
        await UniTask.Delay(TimeSpan.FromSeconds(_replayEffectWaitDuration), cancellationToken: ct);

        _trailEffect.Play();
    }
}
