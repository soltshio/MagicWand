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
        await UniTask.WaitUntil(() => SerialHandler.Instance != null, cancellationToken: this.GetCancellationTokenOnDestroy());

        // SerialHandlerのインスタンスを取得
        _serialHandlerInstance = SerialHandler.Instance;
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
