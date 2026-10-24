using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//マイコンにメッセージを送信するクラス
//参考サイト　https://github.com/soltshio/UnityHaraZemi1/blob/main/haraZemiUnityTest/Assets/Scripts/Component/Micon/SendManager.cs

public class SendManager : MonoBehaviour
{
    SerialHandler _serialHandlerInstance;

    Queue<string> _messageQueue = new Queue<string>();

    public void AddMessage(string message)
    {
        _messageQueue.Enqueue(message);
    }

    async void Start()
    {
        await UniTask.WaitUntil(() => SerialHandler.Instance != null, cancellationToken: this.GetCancellationTokenOnDestroy());

        // SerialHandlerのインスタンスを取得
        _serialHandlerInstance = SerialHandler.Instance;
    }

    void Update()
    {
        if (_serialHandlerInstance == null) return;
        if (_messageQueue.Count == 0) return;

        //毎フレームに1つずつメッセージを送る
        string message = _messageQueue.Dequeue();
        _serialHandlerInstance.Write(message);
    }
}
