using UnityEngine;

//作成者:杉山
//ジョイコンでの杖の操作

public class JoyconWandController : MonoBehaviour
{
    [SerializeField]
    MovingAveragedJoyconOrientation _movingAveragedJoyconOrientation;

    public void NewWandRot(WandController wandController,Transform wand)
    {
        var joyconOrientation = _movingAveragedJoyconOrientation.SmoothedOrientation;

        var wandRot = JoyconOrientationToWandRotation(joyconOrientation);

        wand.localRotation = wandRot * wandController.OriginRot;
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
