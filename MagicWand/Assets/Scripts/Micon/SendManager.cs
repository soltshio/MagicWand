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

    //コマンドごとのマイコンに送るメッセージ(英語大文字2文字、数字１文字の合計３文字)
    readonly Dictionary<EMiconCommand, string> _commandToMessageMap = new Dictionary<EMiconCommand, string>
    {
        //太陽魔法
        { EMiconCommand.StartSun, "SN0" },
        { EMiconCommand.PlaySunSound, "SN1" },
        { EMiconCommand.FinishSun, "SN2" },
        //星魔法
        { EMiconCommand.ChangeToStar, "ST0" },
        { EMiconCommand.PlayStarSound, "ST1" },
        { EMiconCommand.FinishStar, "ST2" },
        //雷魔法
        { EMiconCommand.ChangeToThunder, "TN0" },
        { EMiconCommand.PlayThunderSound, "TN1" },
        { EMiconCommand.FinishThunder, "TN2" },
        //水魔法
        { EMiconCommand.ChangeToWater, "WT0" },
        { EMiconCommand.PlayWaterSound, "WT1" },
        { EMiconCommand.FinishWater, "WT2" },
        //時魔法
        { EMiconCommand.ChangeToTime, "TM0" },
        { EMiconCommand.PlayTimeSound, "TM1" },
        { EMiconCommand.FinishTime, "TM2" },
    };


    public void AddMessage(EMiconCommand command)
    {
        if (!_commandToMessageMap.TryGetValue(command, out string message)) return;
        
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
