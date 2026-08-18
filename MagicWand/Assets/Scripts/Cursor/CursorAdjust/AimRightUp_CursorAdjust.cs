using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//作成者:杉山
//カーソルの位置の調整シーンの右上にカーソルを合わせる処理

[System.Serializable]
public class AimRightUp_CursorAdjust
{
    [SerializeField]
    TextMeshProUGUI _guideAimRightUpText;

    HokuyoDataReceiver _hokuyoDataReceiver;

    bool _isConfirmed = true;//位置が確定されたか(これがfalse->trueになった時に次の処理に移る)

    public void Initialize(HokuyoDataReceiver hokuyoDataReceiver)
    {
        _hokuyoDataReceiver = hokuyoDataReceiver;
    }

    public async UniTask<Vector2> GetCurrentRightUpPosAsync(CancellationToken ct)
    {
        _guideAimRightUpText.gameObject.SetActive(true);
        _isConfirmed = false;

        await UniTask.WaitUntil(() => _isConfirmed, cancellationToken: ct);

        _guideAimRightUpText.gameObject.SetActive(false);

        Vector2 blobPos = _hokuyoDataReceiver.BlobPosition;
        return blobPos;
    }

    public void GetInputKey(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (_isConfirmed == true) return;

        _isConfirmed = true;
    }
}
