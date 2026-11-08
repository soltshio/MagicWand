using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//作成者:杉山
//北陽レーザー検知範囲内に物が無くなった際にレイキャストをブロックする機能

public class RaycastBlockManagerOnHokuyoExit : MonoBehaviour
{
    [Tooltip("北陽レーザー検知範囲内にオブジェクトが無くなってから、UIを反応させなくするまでの時間")] [SerializeField]
    float _timeOutToBlockRaycast = 0.5f;

    HokuyoDataReceiver _hokuyoDataReceiver;
    RaycastBlockSingletonPanel _raycastBlockSingletonPanel;

    SingleTaskCancellation _singleTaskCancellation = new();

    async void OnEnable()
    {
        if(_hokuyoDataReceiver == null)
        {
            _hokuyoDataReceiver = await HokuyoDataReceiver.GetInstanceAsync(this.GetCancellationTokenOnDestroy());
        }

        if(_raycastBlockSingletonPanel == null)
        {
            _raycastBlockSingletonPanel = await RaycastBlockSingletonPanel.GetInstanceAsync(this.GetCancellationTokenOnDestroy());
        }


        _hokuyoDataReceiver.OnSwitchIsExistObject += StartCountDownToBlockRaycast;

        //レイキャストをブロックするためのパネルの表示状態の初期化
        bool shouldBlockRaycast = !_hokuyoDataReceiver.IsExistObject && _hokuyoDataReceiver.IsRunning;
        _raycastBlockSingletonPanel.SetBlockRaycastPanelEnabled(shouldBlockRaycast);
    }

    private void OnDisable()
    {
        if (_hokuyoDataReceiver == null) return;
        
        _hokuyoDataReceiver.OnSwitchIsExistObject -= StartCountDownToBlockRaycast;
        _singleTaskCancellation.Cancel();
    }

    //北陽レーザーの検知範囲からものが無くなった時に、一定時間後にレイキャストをブロックする
    void StartCountDownToBlockRaycast(bool isExistObject)
    {
        var newCt = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        if (isExistObject)
        {
            _raycastBlockSingletonPanel.SetBlockRaycastPanelEnabled(false);
        }
        else
        {
            CountDownToBlockRaycastAsync(newCt).Forget();
        }
    }

    async UniTask CountDownToBlockRaycastAsync(CancellationToken ct)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_timeOutToBlockRaycast), cancellationToken: ct);

        _raycastBlockSingletonPanel.SetBlockRaycastPanelEnabled(true);
    }
}
