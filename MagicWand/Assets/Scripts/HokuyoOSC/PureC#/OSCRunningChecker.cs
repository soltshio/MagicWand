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

    SingleTaskCancellation _singleTaskCancellation = new();

    public bool IsRunning { get { return _isRunning; } }

    public void UpdateRunning(CancellationToken ct)
    {
        var newCt = _singleTaskCancellation.CancelAndReCreateToken(ct);

        TimeOutAsync(ct).Forget();
    }

    async UniTask TimeOutAsync(CancellationToken ct)
    {
        _isRunning = true;

        await UniTask.Delay(TimeSpan.FromSeconds(_timeOutSec), cancellationToken: ct);

        _isRunning = false;
    }
}
