using UnityEngine;

//作成者:杉山
//移動平均されたジョイコンの傾き

public class MovingAveragedJoyconOrientation : MonoBehaviour
{
    [Tooltip("移動平均のウィンドウサイズ")] [SerializeField]
    int _movingAverageWindowSize = 45;

    [SerializeField]
    JoyconInputManager _joyconInputManager;

    QuaternionMovingAverage _movingAverage;//移動平均を取るクラス
    Quaternion _currentOrientation=Quaternion.identity;//現在の傾き

    public Quaternion SmoothedOrientation => _currentOrientation;//移動平均された傾き

    private void Awake()
    {
        _movingAverage = new QuaternionMovingAverage(_movingAverageWindowSize);
    }
    private void Update()
    {
        var joyconOrientation = _joyconInputManager.Orientation;

        //移動平均処理
        _currentOrientation = _movingAverage.AddValue(joyconOrientation);
    }
}
