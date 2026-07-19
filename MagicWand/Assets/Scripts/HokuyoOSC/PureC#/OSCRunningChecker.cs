using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//作成者:杉山
//OSC通信が行われているかのチェックを行い

[System.Serializable]
public class OSCRunningChecker
{
    [SerializeField]
    float _timeOutSec = 2f;

    bool _isRunning = false;
    CancellationTokenSource _cts;

    public bool IsRunning { get { return _isRunning; } }

    public void UpdateRunning(CancellationToken ct)
    {
        CancelRunningUniTask();

        var newCt = CreateLinkedToken(ct);

        TimeOutAsync(ct).Forget();
    }

    void CancelRunningUniTask()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    CancellationToken CreateLinkedToken(CancellationToken ct)
    {
        _cts = new CancellationTokenSource();

        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, ct);

        return linkedCts.Token;
    }

    async UniTask TimeOutAsync(CancellationToken ct)
    {
        _isRunning = true;

        await UniTask.Delay(TimeSpan.FromSeconds(_timeOutSec), cancellationToken: ct);

        _isRunning = false;
    }
}
