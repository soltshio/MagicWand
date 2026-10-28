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
        { EMiconCommand.StartSun,                  "SNst" },
        { EMiconCommand.CastSun,                   "SNcs" },
        { EMiconCommand.InvokeSun,                 "SNiv" },
        { EMiconCommand.InvokeSun_PlaySound,       "SNps" },
        { EMiconCommand.FinishSun,                 "SNfn" },
        //星魔法
        { EMiconCommand.StartStar,                 "STst" },
        { EMiconCommand.CastStar,                  "STcs" },
        { EMiconCommand.InvokeStar,                "STiv" },
        { EMiconCommand.InvokeStar_PlaySound,      "STps" },
        { EMiconCommand.FinishStar,                "STfn" },
        //雷魔法
        { EMiconCommand.StartThunder,              "TNst" },
        { EMiconCommand.CastThunder,               "TNcs" },
        { EMiconCommand.InvokeThunder,             "TNiv" },
        { EMiconCommand.InvokeThunder_PlaySound,   "TNps" },
        { EMiconCommand.FinishThunder,             "TNfn" },
        //水魔法
        { EMiconCommand.StartWater,                "WTst" },
        { EMiconCommand.CastWater,                 "WTcs" },
        { EMiconCommand.InvokeWater,               "WTiv" },
        { EMiconCommand.InvokeWater_PlaySound,     "WTps" },
        { EMiconCommand.FinishWater,               "WTfn" },
        //時魔法
        { EMiconCommand.StartTime,                 "TMst" },
        { EMiconCommand.CastTime,                  "TMcs" },
        { EMiconCommand.InvokeTime,                "TMiv" },
        { EMiconCommand.InvokeTime_PlaySound,      "TMps" },
        { EMiconCommand.FinishTime,                "TMfn" },
    };

    //コマンドのレベル
    const int _defaultLevel = 0;
    const int _minLevel = 0;
    const int _maxLevel = 99;


    //levelは2桁の正の整数(特に指定がなければ書かなくていい)
    public void AddMessage(EMiconCommand command,int level= _defaultLevel)
    {
        if (!_commandToMessageMap.TryGetValue(command, out string message)) return;

        level = Mathf.Clamp(level, _minLevel, _maxLevel);

        //レベルを文字に変換してコマンドにくっつける
        //レベルは必ず2桁の数字
        message += level.ToString("00");

        
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
