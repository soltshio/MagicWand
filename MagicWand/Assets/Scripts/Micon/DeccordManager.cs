// Unityでシリアル通信で送られてくるデータをデコードする雛形
// 2025_8月Ver.


using Cysharp.Threading.Tasks;
using UnityEngine;

//マイコンから来るメッセージをデコードする
//作成者:杉山
//参考サイト https://github.com/soltshio/UnityHaraZemi1/blob/main/haraZemiUnityTest/Assets/Scripts/Component/Micon/DeccordManager.cs

public class DeccordManager : MonoBehaviour
{
    SerialHandler _serialHandlerInstance;

    SingleTaskCancellation _singleTaskCancellation = new();

    async void Start()
    {
        // SerialHandlerのインスタンスを取得
        _serialHandlerInstance = await SerialHandler.GetInstanceAsync(this.GetCancellationTokenOnDestroy());
    }

    async void OnEnable()
    {
        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        await UniTask.WaitUntil(() => _serialHandlerInstance != null, cancellationToken: ct);

        // 信号受信時に呼ばれる関数としてOnDataReceived関数を登録
        _serialHandlerInstance.OnDataReceived += OnDataReceived;
    }

    async void OnDisable()
    {
        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        await UniTask.WaitUntil(() => _serialHandlerInstance != null, cancellationToken: ct);

        _serialHandlerInstance.OnDataReceived -= OnDataReceived;
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
        Debug.Log(message);
    }
}
