using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//ジョイコンの入力情報を管理するクラス
//enableがオフの間は入力を受け付けない

//左、右のジョイコンどちらか片方のみを使用(両方あれば右を優先)
//加速度、ジャイロ、傾きを取得可能

public class JoyconInputManager : MonoBehaviour
{
    private Joycon _joycon;

    Vector3 _gyro=Vector3.zero;
    Vector3 _accel=Vector3.zero;
    Quaternion _orientation=Quaternion.identity;


    //取得可能な入力情報
    public Vector3 Gyro => _gyro;//ジャイロ
    public Vector3 Accel => _accel;//加速度
    public Quaternion Orientation => _orientation;//傾き


    private void Start()
    {
        TryGetJoycon();
    }

    private void Update()
    {
        if(_joycon==null) return;

        GetGyro();
        GetAccel();
        GetOrientation();
    }

    void GetGyro()
    {
        _gyro = _joycon.GetGyro();
    }

    void GetAccel()
    {
        _accel = _joycon.GetAccel();
    }

    void GetOrientation()
    {
        _orientation = _joycon.GetVector();
    }

    void TryGetJoycon()
    {
        var joycons = JoyconManager.Instance.j;

        if (joycons == null || joycons.Count <= 0) return;

        _joycon = joycons.Find(c => !c.isLeft);//右を取得

        //右の取得に失敗すれば左も取得してみる
        if (_joycon != null) return;

        _joycon = joycons.Find(c => c.isLeft);
    }
}
