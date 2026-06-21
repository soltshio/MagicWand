using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//作成者:杉山
//時計のエフェクトの表示・非表示を行う機能

public class ClockEffectActivator : MonoBehaviour
{
    [Tooltip("何秒かけて太陽光レンズの表示・非表示を行うか")] [SerializeField]
    float _shiftDuration = 1.5f;

    [Tooltip("時計の針の回るスピード")] [SerializeField]
    float _clockHandRotateSpeed = 1.0f;

    [SerializeField]
    RectTransform _clockHandsRotateCenterTrs;

    [SerializeField]
    Image[] _clockImages; 

    [SerializeField]
    CanvasGroup _clockCanvasGroup;

    bool _isActive = false;

    const float _minAlpha = 0;
    const float _maxAlpha = 1;

    public async UniTask ActivateAsync()
    {
        _isActive = true;

        await ShiftAlphaAsync(this.GetCancellationTokenOnDestroy(), _minAlpha, _maxAlpha, _shiftDuration);
    }

    public async UniTask DeactivateAsync()
    {
        await ShiftAlphaAsync(this.GetCancellationTokenOnDestroy(), _maxAlpha, _minAlpha, _shiftDuration);

        _isActive = false;

        SwitchImageEnabled(false);
    }

    void Start()
    {
        //最初は非表示に
        _clockCanvasGroup.alpha = _minAlpha;

        SwitchImageEnabled(false);
    }

    void Update()
    {
        //時計の針の回転処理
        if (!_isActive) return;

        float speed = _clockHandRotateSpeed * Time.deltaTime;

        _clockHandsRotateCenterTrs.Rotate(Vector3.forward * speed, Space.Self);
    }

    async UniTask ShiftAlphaAsync(CancellationToken ct, float fromAlpha, float toAlpha, float duration)
    {
        SwitchImageEnabled(true);

        float elapsed = 0f;

        while (true)
        {
            elapsed += Time.deltaTime;

            float rate = elapsed / duration;
            float newAlpha = Mathf.Lerp(fromAlpha, toAlpha, rate);

            _clockCanvasGroup.alpha = newAlpha;

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);

            if (elapsed > duration) break;
        }

        _clockCanvasGroup.alpha = toAlpha;
    }

    //時計の画像のenabledを切り替える
    void SwitchImageEnabled(bool enabled)
    {
        for(int i=0; i<_clockImages.Length ;i++)
        {
            _clockImages[i].enabled = enabled;
        }
    }
}
