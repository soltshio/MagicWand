using UnityEngine;
using UnityEngine.InputSystem;

//作成者：杉山
//杖を動かす
//ジョイコンが見つからなかったら、キーボードでも動かせる

public class WandController : MonoBehaviour
{
    [Tooltip("y軸回転のスピード")] [SerializeField]
    float _yRotSpeed = 190f;

    [Tooltip("x軸回転のスピード")] [SerializeField]
    float _xRotSpeed = 190f;

    [SerializeField]
    Transform _wand;

    [SerializeField]
    MovingAveragedJoyconOrientation _movingAveragedJoyconOrientation;

    [SerializeField]
    JoyconInputManager _joyconInputManager;

    Quaternion _originRot;

    Vector2 _getVec = Vector2.zero;

    public void CatchInput(InputAction.CallbackContext context)
    {
        _getVec = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        //ジョイコンが接続されている場合
        if(_joyconInputManager.IsConnected)
        {
            UpdateOrientation_Joycon();
        }
        else
        {
            UpdateOrientation_Key();
        }
    }

    void UpdateOrientation_Joycon()
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

    void UpdateOrientation_Key()
    {
        //y軸回転
        Quaternion yRot = Quaternion.AngleAxis(_getVec.x * _yRotSpeed * Time.deltaTime, _wand.parent.up);

        //x軸回転
        Quaternion xRot = Quaternion.AngleAxis(_getVec.y * -_xRotSpeed * Time.deltaTime, _wand.parent.right);

        _wand.rotation = yRot * xRot * _wand.rotation;
    }
}
