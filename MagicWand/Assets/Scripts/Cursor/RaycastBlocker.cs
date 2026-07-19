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

    HokuyoBlobPosReceiver _hokuyoBlobPosReceiver;

    CancellationTokenSource _cts;

    void OnEnable()
    {
        _blockRaycastPanel.enabled = false;

        SetHokuyoBlobPosReceiver();

        if (_hokuyoBlobPosReceiver == null) return;

        _hokuyoBlobPosReceiver.OnSwitchIsExistObject += StartCountDownToBlockRaycast;
    }

    private void OnDisable()
    {
        SetHokuyoBlobPosReceiver();

        if (_hokuyoBlobPosReceiver == null) return;

        _hokuyoBlobPosReceiver.OnSwitchIsExistObject -= StartCountDownToBlockRaycast;
    }

    void StartCountDownToBlockRaycast(bool isExistObject)
    {
        if (isExistObject) return;

        CancelRunningUniTask();

        var newCt = CreateLinkedToken(this.GetCancellationTokenOnDestroy());

        CountDownToBlockRaycastAsync(newCt).Forget();
    }

    async UniTask CountDownToBlockRaycastAsync(CancellationToken ct)
    {
        _blockRaycastPanel.enabled = false;

        await UniTask.Delay(TimeSpan.FromSeconds(_timeOutToBlockRaycast), cancellationToken: ct);

        _blockRaycastPanel.enabled = true;
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

    void SetHokuyoBlobPosReceiver()
    {
        if (_hokuyoBlobPosReceiver != null) return;

        //まだ取得出来ていなかったら、取得処理を行う

        var receiverObj = GameObject.FindWithTag(TagNameList.OSCReceiver);

        if (receiverObj == null) return;

        var hokuyoBlobPosReceiver = receiverObj.GetComponent<HokuyoBlobPosReceiver>();

        _hokuyoBlobPosReceiver = hokuyoBlobPosReceiver;
    }
}
