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

    bool _isActive = true;//これがtrueになっている間は北陽レーザーから値を受け取ってカーソルを動かすようにする。false時はマウスで動かせるようにする。
    

    private void OnEnable()
    {
        _escapeAction.action.performed += OnCancelHokuyoControlMode;
        _escapeAction.action.Enable();

        _hokuyoBlobPosReceiver.OnMovePos += MoveCursor;
    }

    private void OnDisable()
    {
        _escapeAction.action.performed -= OnCancelHokuyoControlMode;
        _escapeAction.action.Disable();

        _hokuyoBlobPosReceiver.OnMovePos -= MoveCursor;
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

        var mainCamera = Camera.main;
        Vector2 cursorWarpPos = mainCamera.ViewportToScreenPoint(blobPos);

        Mouse.current.WarpCursorPosition(cursorWarpPos);
    }
}
