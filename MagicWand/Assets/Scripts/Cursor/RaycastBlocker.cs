using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//作成者:杉山
//北陽レーザー検知範囲内にオブジェクトが無い時にUIが反応しないようにする機能

public class RaycastBlocker : MonoBehaviour
{
    [Tooltip("北陽レーザー検知範囲内にオブジェクトが無くなってから、UIを反応させなくするまでの時間")] [SerializeField]
    float _timeOutToBlockRaycast = 0.5f;

    [SerializeField]
    Image _blockRaycastPanel;

    HokuyoDataReceiver _hokuyoDataReceiver;

    SingleTaskCancellation _singleTaskCancellation = new ();

    async void OnEnable()
    {
        _blockRaycastPanel.enabled = false;

        _hokuyoDataReceiver = await HokuyoDataReceiver.GetInstanceAsync(this.GetCancellationTokenOnDestroy());

        _hokuyoDataReceiver.OnSwitchIsExistObject += StartCountDownToBlockRaycast;
    }

    private void OnDisable()
    {
        _hokuyoDataReceiver.OnSwitchIsExistObject -= StartCountDownToBlockRaycast;
    }

    void StartCountDownToBlockRaycast(bool isExistObject)
    {
        if(isExistObject)
        {
            _blockRaycastPanel.enabled = false;
        }
        else
        {
            var newCt = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

            CountDownToBlockRaycastAsync(newCt).Forget();
        }
    }

    async UniTask CountDownToBlockRaycastAsync(CancellationToken ct)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_timeOutToBlockRaycast), cancellationToken: ct);

        _blockRaycastPanel.enabled = true;
    }
}
