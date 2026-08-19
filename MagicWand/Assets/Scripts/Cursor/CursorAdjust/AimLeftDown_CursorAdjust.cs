using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//作成者:杉山
//カーソルの位置の調整シーンの左下にカーソルを合わせる処理

[System.Serializable]
public class AimLeftDown_CursorAdjust
{
    [SerializeField]
    TextMeshProUGUI _guideAimLeftDownText;

    HokuyoDataReceiver _hokuyoDataReceiver;

    bool _isConfirmed = true;//位置が確定されたか(これがfalse->trueになった時に次の処理に移る)

    public void Initialize(HokuyoDataReceiver hokuyoDataReceiver)
    {
        _hokuyoDataReceiver = hokuyoDataReceiver;
    }

    public async UniTask<Vector2> GetCurrentDetectionPortLeftDownPosAsync(CancellationToken ct)
    {
        _guideAimLeftDownText.gameObject.SetActive(true);
        _isConfirmed = false;

        await UniTask.WaitUntil(() => _isConfirmed, cancellationToken: ct);

        _guideAimLeftDownText.gameObject.SetActive(false);

        Vector2 currentDetectionPortLeftDownPos = _hokuyoDataReceiver.DetectionPortPosition;
        return currentDetectionPortLeftDownPos;
    }

    public void GetInputKey(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (_isConfirmed == true) return;

        _isConfirmed = true;
    }
}
