using UnityEngine;
using UnityEngine.InputSystem;

//作成者:杉山
//杖を制御するクラス

public class WandController : MonoBehaviour
{
    [SerializeField] 
    JoyconInputManager _joyconInputManager;

    [Tooltip("杖(動かす対象)")] [SerializeField]
    Transform _wand;

    const int _movingAverageWindowSize = 20;//移動平均のウィンドウサイズ
    QuaternionMovingAverage _movingAverage;//移動平均を取るクラス

    Quaternion _originJoyconOrientation = Quaternion.identity;

    Quaternion _currentRot=Quaternion.identity;

    public void ResetPos(InputAction.CallbackContext context)//回転をリセット
    {
        if (!context.performed) return;

        _originJoyconOrientation= _joyconInputManager.Orientation * Quaternion.AngleAxis(90f,Vector3.right);
    }

    private void Awake()
    {
        _movingAverage = new QuaternionMovingAverage(_movingAverageWindowSize);

        _currentRot = Quaternion.identity;
    }

    private void Update()
    {
        Quaternion newRot;

        var joyconOrientation = _joyconInputManager.Orientation;

        //基準の回転との計算
        joyconOrientation = Quaternion.Inverse(_originJoyconOrientation) * joyconOrientation;

        //y軸回転とz軸回転を入れ替える
        Quaternion c = Quaternion.AngleAxis(90f, Vector3.right);

        newRot = c * joyconOrientation * Quaternion.Inverse(c);

        //移動平均処理
        _currentRot = _movingAverage.AddValue(newRot);

        //杖を回転させる
        _wand.localRotation = _currentRot;
    }
}
