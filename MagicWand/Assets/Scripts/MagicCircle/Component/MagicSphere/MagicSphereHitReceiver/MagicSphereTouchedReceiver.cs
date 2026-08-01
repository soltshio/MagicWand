using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//作成者:杉山
//プレイヤーが魔法球に触れたことを受け取り通知するクラス

public class MagicSphereTouchedReceiver : MonoBehaviour
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
        set
        {
            if (value == _isTouched) return;

            if (value == true) OnTouchedEnter?.Invoke();
            else OnTouchedExit?.Invoke();
        }
    }

    //魔法球に触れていることを通知する
    public void InformTouch()
    {
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
