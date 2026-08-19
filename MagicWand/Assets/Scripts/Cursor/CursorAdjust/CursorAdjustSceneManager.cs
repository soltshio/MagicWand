using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//作成者:杉山
//カーソルの位置を調整するシーンの制御

public class CursorAdjustSceneManager : MonoBehaviour
{
    [SerializeField]
    Start_CursorAdjust _start_CursorAdjust;

    [SerializeField]
    Finish_CursorAdjust _finish_CursorAdjust;

    [SerializeField]
    AimCenter_CursorAdjust _aimCenter_CursorAdjust;

    [SerializeField]
    AimRightUp_CursorAdjust _aimRightUp_CursorAdjust;

    [SerializeField]
    AimRightDown_CursorAdjust _aimRightDown_CursorAdjust;

    [SerializeField]
    AimLeftDown_CursorAdjust _aimLeftDown_CursorAdjust;

    [SerializeField]
    AdjustHokuyoCenter_CursorAdjust _adjustHokuyoCenter_CursorAdjust;

    [SerializeField]
    AdjustHokuyoSize_CursorAdjust _adjustHokuyoSize_CursorAdjust;

    async void Start()
    {
        try
        {
            var ct = this.GetCancellationTokenOnDestroy();

            //初期化処理
            await InitializeAsync(ct);

            //補正開始
            await _start_CursorAdjust.InformStartAsync(ct);

            //中心に合わせる
            Vector2 centerBlobPos = await _aimCenter_CursorAdjust.GetCurrentCenterPosAsync(ct);

            //画面の中心補正処理を行う
            _adjustHokuyoCenter_CursorAdjust.AdjustHokuyoCenter(centerBlobPos);

            //右上に合わせる
            Vector2 rightUpBlobPos = await _aimRightUp_CursorAdjust.GetCurrentRightUpPosAsync(ct);
            //右下に合わせる
            Vector2 rightDownBlobPos = await _aimRightDown_CursorAdjust.GetCurrentRightDownPosAsync(ct);
            //左下に合わせる
            Vector2 leftDownBlobPos = await _aimLeftDown_CursorAdjust.GetCurrentLeftDownPosAsync(ct);

            //画面の大きさ補正処理を行う
            _adjustHokuyoSize_CursorAdjust.AdjustHokuyoSize(rightUpBlobPos, rightDownBlobPos, leftDownBlobPos);

            //補正終了
            await _finish_CursorAdjust.InformFinishAsync(ct);

            //タイトルシーンに遷移
            LoadTitleScene();
        }
        catch 
        {
            Debug.Log("補正処理が中断されました");
        }
    }

    async UniTask InitializeAsync(CancellationToken ct)
    {
        var hokuyoDataReceiver = await HokuyoDataReceiver.GetInstanceAsync(ct);
        var hokuyoDataTransmitter = await HokuyoDataTransmitter.GetInstanceAsync(ct);

        _aimCenter_CursorAdjust.Initialize(hokuyoDataReceiver);
        _aimRightUp_CursorAdjust.Initialize(hokuyoDataReceiver);
        _aimRightDown_CursorAdjust.Initialize(hokuyoDataReceiver);
        _aimLeftDown_CursorAdjust.Initialize(hokuyoDataReceiver);
        _adjustHokuyoCenter_CursorAdjust.Initialize(hokuyoDataTransmitter, hokuyoDataReceiver);
        _adjustHokuyoSize_CursorAdjust.Initialize(hokuyoDataTransmitter, hokuyoDataReceiver);
    }

    public void GetInputKey(InputAction.CallbackContext context)
    {
        _aimCenter_CursorAdjust.GetInputKey(context);
        _aimRightUp_CursorAdjust.GetInputKey(context);
        _aimRightDown_CursorAdjust.GetInputKey(context);
        _aimLeftDown_CursorAdjust.GetInputKey(context);
    }

    void LoadTitleScene()
    {
        SceneManager.LoadScene(SceneNameList.TitleScene);
    }
}
