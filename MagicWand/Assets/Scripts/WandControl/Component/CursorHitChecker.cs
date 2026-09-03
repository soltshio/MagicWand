using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

//作成者：杉山
//レイを飛ばして、カーソルとオブジェクトの当たり判定をする

public class CursorHitChecker : MonoBehaviour
{
    [SerializeField]
    Camera _magicCircleCamera;

    [SerializeField]
    float _deactiveDurationOnObjectEnter=0.05f;

    bool _isCursorDeactiveOnWandExit = false;

    HokuyoDataReceiver _hokuyoDataReceiver;
    SingleTaskCancellation _singleTaskCancellation = new();

    async void OnEnable()
    {
        _hokuyoDataReceiver = await HokuyoDataReceiver.GetInstanceAsync(this.GetCancellationTokenOnDestroy());

        _hokuyoDataReceiver.OnSwitchIsExistObject += NotifyOnObjectEnter;
    }

    void OnDisable()
    {
        _hokuyoDataReceiver.OnSwitchIsExistObject -= NotifyOnObjectEnter;
    }

    void NotifyOnObjectEnter(bool isExistObject)
    {
        if (!isExistObject) return;

        var newCt = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());
        CursorDeactiveAsync(newCt).Forget();
    }

    async UniTask CursorDeactiveAsync(CancellationToken ct)
    {
        _isCursorDeactiveOnWandExit = true;

        await UniTask.Delay(TimeSpan.FromSeconds(_deactiveDurationOnObjectEnter), cancellationToken: ct);

        _isCursorDeactiveOnWandExit = false;
    }

    void Update()
    {
        if (_hokuyoDataReceiver == null) return;

        if (_isCursorDeactiveOnWandExit) return;

        //OSC通信が動いているかつ、北陽レーザーの検知範囲内にオブジェクトが無い
        if (_hokuyoDataReceiver.IsRunning && !_hokuyoDataReceiver.IsExistObject) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = _magicCircleCamera.ScreenPointToRay(mousePos);

        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit)) return;

        if (!hit.collider.CompareTag(TagNameList.RaycastableObject)) return;

        var touchedReceiver = hit.collider.GetComponent<TouchedReceiver>();

        if(touchedReceiver == null) return;

        touchedReceiver.InformTouch();
    }
}
