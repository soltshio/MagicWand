using System;
using UnityEngine;
using UnityEngine.InputSystem;

//作成者:杉山
//北陽レーザーでカーソルを動かすクラス

public class CursorControllerByHokuyo : MonoBehaviour
{
    [SerializeField]
    HokuyoBlobPosReceiver _hokuyoBlobPosReceiver;

    [SerializeField]
    InputActionReference _escapeAction;

    [SerializeField][Min(1)]
    int _movingAverageSize;

    [Tooltip("北陽レーザー検知範囲内に何もない時はカーソルを真ん中に戻すか？")] [SerializeField]
    bool _shouldReturnCursorToCenterWhenIsNotExist;

    Vector2MovingAverage _movingAverage;

    bool _isActive = true;//これがtrueになっている間は北陽レーザーから値を受け取ってカーソルを動かすようにする。false時はマウスで動かせるようにする。

    Vector2 _windowCenter = new Vector2(0.5f,0.5f);

    private void Awake()
    {
        _movingAverage = new(_movingAverageSize);
    }

    private void OnEnable()
    {
        _escapeAction.action.performed += OnCancelHokuyoControlMode;
        _escapeAction.action.Enable();

        _hokuyoBlobPosReceiver.OnCatchPos += MoveCursor;
        _hokuyoBlobPosReceiver.OnSwitchExistObject += ClearMovingAverage;
    }

    private void OnDisable()
    {
        _escapeAction.action.performed -= OnCancelHokuyoControlMode;
        _escapeAction.action.Disable();

        _hokuyoBlobPosReceiver.OnCatchPos -= MoveCursor;
        _hokuyoBlobPosReceiver.OnSwitchExistObject -= ClearMovingAverage;
    }

    private void OnCancelHokuyoControlMode(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        _isActive = false;

#if UNITY_EDITOR
        Debug.Log("北陽レーザーでのカーソル操作モードを解除します");
#endif
    }

    void MoveCursor(Vector2 blobPos)
    {
        if (!_isActive) return;
        if (!_hokuyoBlobPosReceiver.IsExistObject) return;

        blobPos = _movingAverage.AddValue(blobPos);

        var mainCamera = Camera.main;
        Vector2 cursorWarpPos = mainCamera.ViewportToScreenPoint(blobPos);

        Mouse.current.WarpCursorPosition(cursorWarpPos);
    }

    void ClearMovingAverage(bool isExistObject)
    {
        //北陽レーザー検知範囲内に何もない場合は移動平均をクリアする
        if (isExistObject) return;

        _movingAverage.Clear();

        if (!_shouldReturnCursorToCenterWhenIsNotExist) return;

        //真ん中に戻す
        var mainCamera = Camera.main;
        Vector2 cursorWarpPos = mainCamera.ViewportToScreenPoint(_windowCenter);

        Mouse.current.WarpCursorPosition(cursorWarpPos);
    }
}
