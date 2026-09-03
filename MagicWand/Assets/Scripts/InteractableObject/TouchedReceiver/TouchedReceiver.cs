using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//作成者:杉山
//何かがオブジェクト(自分)に触れたことを受け取り通知するクラス(レイなどがこのオブジェクトに当たった時にInformTouch()を呼ぶことでこのオブジェクトに触れたことを伝える)

public class TouchedReceiver : MonoBehaviour
{
    [SerializeField]
    float _timeOutDurationToResetIsTouched = 0.1f;

    bool _isTouched = false;

    SingleTaskCancellation _singleTaskCancellation = new();

    public event Action OnTouchedEnter;
    public event Action OnTouchedExit;

    public bool IsTouched
    {
        get { return _isTouched; }
        private set
        {
            if (value == _isTouched) return;

            _isTouched = value;

            if (value == true) OnTouchedEnter?.Invoke();
            else OnTouchedExit?.Invoke();
        }
    }

    //オブジェクトに触れていることを通知する
    public void InformTouch()
    {
        if(!isActiveAndEnabled) return;

        var newCt = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        TouchUpdateAsync(newCt).Forget();
    }

    //触れた知らせを受け取ってから
    async UniTask TouchUpdateAsync(CancellationToken ct)
    {
        IsTouched = true;

        await UniTask.Delay(TimeSpan.FromSeconds(_timeOutDurationToResetIsTouched), cancellationToken: ct);

        IsTouched = false;
    }
}
