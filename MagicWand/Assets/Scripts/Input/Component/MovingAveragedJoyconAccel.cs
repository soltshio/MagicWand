using UnityEngine;

//作成者:杉山
//移動平均されたジョイコンの加速度

public class MovingAveragedJoyconAccel : MonoBehaviour
{
    [Tooltip("移動平均のウィンドウサイズ")] [SerializeField]
    int _movingAverageWindowSize = 45;

    [SerializeField]
    JoyconInputManager _joyconInputManager;

    Vector3MovingAverage _movingAverage;//移動平均を取るクラス
    Vector3 _currentAccel;//現在の加速度

    public Vector3 SmoothedAccel => _currentAccel;//移動平均された加速度

    private void Awake()
    {
        _movingAverage = new Vector3MovingAverage(_movingAverageWindowSize);

        _currentAccel = Vector3.zero;
    }
    private void Update()
    {
        var joyconAccel = _joyconInputManager.Accel;

        //移動平均処理
        _currentAccel = _movingAverage.AddValue(joyconAccel);
    }
}
