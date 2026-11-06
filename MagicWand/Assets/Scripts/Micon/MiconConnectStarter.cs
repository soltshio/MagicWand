using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//ゲーム起動時にマイコンと接続処理を呼ぶ機能

public class MiconConnectStarter : MonoBehaviour
{
    [SerializeField]
    MiconConfig _miconConfig;

    OptionDataSaveLoadManager _optionDataSaveLoadManagerInstance;

    async void Start()
    {
        //OptionDataSaveLoadManagerのインスタンスを取得
        await UniTask.WaitUntil(() => OptionDataSaveLoadManager.Instance != null, cancellationToken: this.GetCancellationTokenOnDestroy());
        _optionDataSaveLoadManagerInstance = OptionDataSaveLoadManager.Instance;

        //ポート番号を取得して、マイコンと接続する
        int portNumber = GetPortNumber();
        SerialHandler.Instance.Open(portNumber);
    }

    int GetPortNumber()
    {
        if (_optionDataSaveLoadManagerInstance.LoadMiconPortNumber(out int portNumber))//ポート番号の設定データがセーブされていれば、その値を使用
        {
            return portNumber;
        }
        else//なければ、デフォルトの物を使用してその値をセーブしておく
        {
            _optionDataSaveLoadManagerInstance.SaveMiconPortNumber(_miconConfig.DefaultPortNumber);
            return _miconConfig.DefaultPortNumber;
        }
    }
}
