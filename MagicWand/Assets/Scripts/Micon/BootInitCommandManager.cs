using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//起動時に初期化コマンドをマイコンに送る機能

public class BootInitCommandManager : MonoBehaviour
{
    SendManager _sendManagerInstance;

    async void Start()
    {
        //SendManagerのインスタンスを取得する(まで待つ)
        await UniTask.WaitUntil(() => SendManager.Instance != null, cancellationToken: this.GetCancellationTokenOnDestroy());
        _sendManagerInstance = SendManager.Instance;

        //初期化コマンドを送る
        _sendManagerInstance.AddMessage(EMiconCommand.Initialize);
    }
}
