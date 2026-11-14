using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//作成者:杉山
//何かがオブジェクト(自分)に数秒間触れ続けたことを受け取り通知するクラス(レイなどがこのオブジェクトに当たった時にInformTouch()を呼ぶことでこのオブジェクトに触れたことを伝える)

public class HoverTouchedReceiver : MonoBehaviour,ITouchable
{
    [SerializeField]
    float _timeOutDurationToResetIsTouched = 0.1f;

    [Tooltip("決定までに何秒間触れ続けなければならないか")] [SerializeField]
    float _hoverDuration = 3f;

    float _hoverProgress = 0f;
    bool _isTouched = false;

    SingleTaskCancellation _resetIsTouchedCancellation = new();
    SingleTaskCancellation _hoverCancellation = new();

    public event Action OnTouchedEnter;
    public event Action OnTouchedExit;
    public event Action OnSubmitted;//決定されたか

    public float HoverDuration
    {
        get { return _hoverDuration; }
        set { _hoverDuration = value; }
    }

    public float HoverProgress { get { return _hoverProgress; } }

    public bool IsTouched
    {
        get { return _isTouched; }
        private set
        {
            if (value == _isTouched) return;

            _isTouched = value;

            if (value)//触れている
            {
                OnTouchedEnter?.Invoke();
                //ホバー判定処理を開始
                var ct = _hoverCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());
                HoverAsync(ct).Forget();
            }
            else//触れていない
            {
                OnTouchedExit?.Invoke();
                //ホバー判定処理を停止
                _hoverCancellation.Cancel();
            }
        }
    }


    //タッチされたことを外部から呼んで知らせる
    public void OnTouched()
    {
        if (!isActiveAndEnabled) return;

        var newCt = _resetIsTouchedCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        ResetIsTouchedAsync(newCt).Forget();
    }

    //触れた知らせを受け取って、少し経過してからタッチされたという判定をリセットする
    async UniTask ResetIsTouchedAsync(CancellationToken ct)
    {
        IsTouched = true;

        await UniTask.Delay(TimeSpan.FromSeconds(_timeOutDurationToResetIsTouched), cancellationToken: ct);

        IsTouched = false;
    }

    //ホバーの判定処理
    async UniTask HoverAsync(CancellationToken ct)
    {
        try
        {
            _hoverProgress = 0;

            ProgressTimer pt = new(_hoverDuration);

            while (!pt.IsFinished)
            {
                pt.Tick();
                _hoverProgress = pt.CalcProgress();

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
            }

            OnSubmitted?.Invoke();
        }
        finally
        {
            _hoverProgress = 0;
        }
    }

    void OnEnable()
    {
        _hoverProgress = 0;
    }

    private void OnDisable()
    {
        IsTouched = false;
    }
}
