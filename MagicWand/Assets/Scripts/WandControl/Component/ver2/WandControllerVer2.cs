using UnityEngine;

//作成者:杉山
//杖を動かすクラス(バージョン2)
//ボタンを押している間だけジャイロオン
//ひっくり返っているかは考慮する
//回転速度は調整不可

public class WandControllerVer2 : MonoBehaviour
{
    [Tooltip("杖(動かす対象)")] [SerializeField]
    Transform _wand;

    [SerializeField]
    MovingAveragedJoyconOrientation _movingAveragedJoyconOrientation;

    [SerializeField]
    JoyconInputManager _joyconInputManager;

    Quaternion _originRot;

    private void Update()
    {
        if (_joyconInputManager.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            OnEnterControlMode();
        }
        else if (_joyconInputManager.GetButton(Joycon.Button.DPAD_RIGHT))
        {
            OnUpdateControlMode();
        }
        else if (_joyconInputManager.GetButtonUp(Joycon.Button.DPAD_RIGHT))
        {
            OnExitControlMode();
        }
    }

    //ボタンを押した瞬間
    void OnEnterControlMode()
    {
        _originRot = _movingAveragedJoyconOrientation.SmoothedOrientation;
        _wand.localRotation = Quaternion.identity;
    }

    //ボタンを押している間
    void OnUpdateControlMode()
    {
        Quaternion delta = _movingAveragedJoyconOrientation.SmoothedOrientation * Quaternion.Inverse(_originRot);

        delta = JoyconOrientationToWandRotation(delta);

        _wand.localRotation = delta;
    }

    //ボタンを離した瞬間
    void OnExitControlMode()
    {
        _wand.localRotation = Quaternion.identity;
    }

    //ジョイコンの回転から杖の回転に変換する
    Quaternion JoyconOrientationToWandRotation(Quaternion joyconOrientation)
    {
        //y軸回転とz軸回転を入れ替える
        Quaternion c = Quaternion.AngleAxis(90f, Vector3.right);
        Quaternion wandRot = c * joyconOrientation * Quaternion.Inverse(c);

        return wandRot;
    }
}
