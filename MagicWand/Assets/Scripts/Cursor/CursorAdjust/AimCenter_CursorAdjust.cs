using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//作成者:杉山
//カーソルの位置の調整シーンの中心にカーソルを合わせる処理

[System.Serializable]
public class AimCenter_CursorAdjust
{
    [SerializeField]
    TextMeshProUGUI _guideAimCenterText;

    HokuyoDataReceiver _hokuyoDataReceiver;

    bool _isConfirmed = true;//位置が確定されたか(これがfalse->trueになった時に次の処理に移る)

    public void Initialize(HokuyoDataReceiver hokuyoDataReceiver)
    {
        _hokuyoDataReceiver = hokuyoDataReceiver;
    }

    public async UniTask<Vector2> GetCurrentDetectionPortCenterPosAsync(CancellationToken ct)
    {
        _guideAimCenterText.gameObject.SetActive(true);
        _isConfirmed = false;

        await UniTask.WaitUntil(() => _isConfirmed, cancellationToken: ct);

        _guideAimCenterText.gameObject.SetActive(false);

        Vector2 currentDetectionPortCenterPos = _hokuyoDataReceiver.DetectionPortPosition;
        return currentDetectionPortCenterPos;
    }

    public void GetInputKey(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (_isConfirmed == true) return;

        _isConfirmed = true;
    }
}
