using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

//作成者：杉山
//魔法陣をなぞるカーソルの動き
//レイを飛ばして、魔法陣上の球との当たり判定をとる

public class MagicCircleTracerCursor : MonoBehaviour
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

        if (!hit.collider.CompareTag(TagNameList.MagicSphere)) return;

        var magicSphere = hit.collider.GetComponent<MagicSphereTouchedReceiver>();

        if(magicSphere == null) return;

        magicSphere.InformTouch();
    }
}
