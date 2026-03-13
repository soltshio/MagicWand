using UnityEngine;

//ジョイコンでの杖の操作

public class JoyconWandController : MonoBehaviour
{
    [SerializeField]
    MovingAveragedJoyconOrientation _movingAveragedJoyconOrientation;

    public Quaternion UpdateWandOrientation(WandController wandController)
    {
        var joyconOrientation = _movingAveragedJoyconOrientation.SmoothedOrientation;

        return JoyconOrientationToWandRotation(joyconOrientation);
    }

    //ジョイコンの回転から杖の回転に変換する
    Quaternion JoyconOrientationToWandRotation(Quaternion joyconOrientation)
    {
        //y軸回転とz軸回転を入れ替える
        Quaternion c = Quaternion.AngleAxis(90f, Vector3.right);
        Quaternion wandRot = c * joyconOrientation * Quaternion.Inverse(c);

        //x軸回転を90度加える
        wandRot *= Quaternion.AngleAxis(90f, Vector3.right);

        return wandRot;
    }
}
