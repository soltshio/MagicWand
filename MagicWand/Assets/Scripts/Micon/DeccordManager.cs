// Unityでシリアル通信で送られてくるデータをデコードする雛形
// 2025_8月Ver.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//マイコンから来るメッセージをデコードする
//作成者:杉山
//参考サイト https://github.com/soltshio/UnityHaraZemi1/blob/main/haraZemiUnityTest/Assets/Scripts/Component/Micon/DeccordManager.cs

public class DeccordManager : MonoBehaviour
{

    const int _errorNum = -1; 

    private void OnEnable()
    {
        if (SerialHandler.Instance == null) return;

        // 信号受信時に呼ばれる関数としてOnDataReceived関数を登録
        SerialHandler.Instance.OnDataReceived += OnDataReceived;
    }

    private void OnDisable()
    {
        if (SerialHandler.Instance == null) return;

        SerialHandler.Instance.OnDataReceived -= OnDataReceived;
    }

    //受信した信号(message)に対する処理
    //messageのプロトコル
    //加速度センサー系は符号+5桁数字=6桁
    //S(6桁GyroX)(6桁GyroZ)(スイッチ1桁)(4桁ロータリーエンコーダー)E
    void OnDataReceived(string message)
    {
        if (message == null)
            return;

        // ここでデコード処理等を記述
        //Debug.Log(message);
    }
}
