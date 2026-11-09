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

    public event Action<bool> OnSwitchIsRunning;

    public bool IsRunning 
    {
        private set
        {
            if(_isRunning == value) return;

            _isRunning = value;
            OnSwitchIsRunning?.Invoke(_isRunning);
        }
        get { return _isRunning; }
    }

    public void UpdateRunning(CancellationToken ct)
    {
        var newCt = _singleTaskCancellation.CancelAndReCreateToken(ct);

        TimeOutAsync(newCt).Forget();
    }

    async UniTask TimeOutAsync(CancellationToken ct)
    {
        IsRunning = true;

        await UniTask.Delay(TimeSpan.FromSeconds(_timeOutSec), cancellationToken: ct);

        IsRunning = false;
    }
}
