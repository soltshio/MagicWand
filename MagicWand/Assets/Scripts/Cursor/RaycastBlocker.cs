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

    SingleTaskCancellation _singleTaskCancellation = new ();

    async void OnEnable()
    {
        _blockRaycastPanel.enabled = false;

        //取得のために1フレーム遅らせる
        var ct = this.GetCancellationTokenOnDestroy();
        await UniTask.Yield(cancellationToken: ct);

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
