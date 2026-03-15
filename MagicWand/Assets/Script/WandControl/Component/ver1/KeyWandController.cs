using UnityEngine;
using UnityEngine.InputSystem;

//作成者:杉山
//キーでの杖の操作(デバッグ用)

public class KeyWandController : MonoBehaviour
{
    [Tooltip("y軸回転のスピード")] [SerializeField]
    float _yRotSpeed = 90f;

    [Tooltip("x軸回転のスピード")] [SerializeField]
    float _xRotSpeed = 90f;

    [SerializeField]
    Transform _rotOffsetTrs;

    Vector2 _getVec=Vector2.zero;

    public void CatchInput(InputAction.CallbackContext context)
    {
        _getVec = context.ReadValue<Vector2>();
    }

    public void NewWandRot(WandController wandController, Transform wand)
    {
        //y軸回転
        Quaternion yRot = Quaternion.AngleAxis(_getVec.x * _yRotSpeed * Time.deltaTime, _rotOffsetTrs.up);

        //x軸回転
        Quaternion xRot = Quaternion.AngleAxis(_getVec.y * -_xRotSpeed * Time.deltaTime, _rotOffsetTrs.right);

        wand.rotation = yRot * xRot * wand.rotation;
    }
}
