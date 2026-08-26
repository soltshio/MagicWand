using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//カーソルのトレイルエフェクトの制御をする
//杖が一度離れてから触れた際にトレイルが途切れるようにすること

public class CursorTrailEffectController : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _trailEffect;

    HokuyoDataReceiver _hokuyoDataReceiver;

    async void OnEnable()
    {
        _hokuyoDataReceiver = await HokuyoDataReceiver.GetInstanceAsync(this.GetCancellationTokenOnDestroy());

        _hokuyoDataReceiver.OnSwitchIsExistObject += ResetEffectOnEnterObjectInHokuyoDetect;
    }

    void ResetEffectOnEnterObjectInHokuyoDetect(bool isExistObject)
    {
        if (!isExistObject) return;

        _trailEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _trailEffect.Play();
    }
}
