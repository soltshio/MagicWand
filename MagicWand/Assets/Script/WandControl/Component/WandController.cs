using UnityEngine;

//作成者:杉山
//杖を制御するクラス

public class WandController : MonoBehaviour
{
    [SerializeField] 
    JoyconInputManager _joyconInputManager;

    [Tooltip("杖(動かす対象)")] [SerializeField]
    Transform _wand;

    Quaternion _originRot = Quaternion.identity;//基準の方向

    const int _movingAverageWindowSize = 40;//移動平均のウィンドウサイズ
    QuaternionMovingAverage _movingAverage;//移動平均を取るクラス

    private void Awake()
    {
        _movingAverage = new QuaternionMovingAverage(_movingAverageWindowSize);
    }

    private void Update()
    {
        Quaternion newRot;

        var orientation = _joyconInputManager.Orientation;

        //y軸回転とz軸回転を入れ替える
        Quaternion c = Quaternion.AngleAxis(90f, Vector3.right);

        newRot = c * orientation * Quaternion.Inverse(c);

        //基準の方向に合わせる
        newRot = newRot * _originRot;

        //移動平均処理
        _wand.rotation = _movingAverage.AddValue(newRot);
    }
}
