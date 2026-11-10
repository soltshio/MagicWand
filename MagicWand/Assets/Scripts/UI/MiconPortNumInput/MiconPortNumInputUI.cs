using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//作成者:杉山
//マイコンポート番号の入力UI

public class MiconPortNumInputUI : MonoBehaviour
{
    [SerializeField]
    OptionResultCanvas _optionResultCanvas;

    [SerializeField]
    TMP_InputField _inputField;


    [Header("不正な入力がされた時の設定")]

    [Tooltip("不正な入力がされた時に表示するテキスト内容")] [Multiline(2)] [SerializeField]
    string _textContent_InvalidInput;


    [Header("既に繋がっているポートに接続しようとした場合の設定")]

    [Tooltip("既に繋がっているポートに接続しようとした場合に表示するテキスト内容")] [Multiline(2)] [SerializeField]
    string _textContent_AlreadyConnected;


    [Header("マイコンとの接続を始めた場合の設定")]

    [Tooltip("接続を一時的に閉じている時のテキスト内容")] [Multiline(2)] [SerializeField]
    string _textContent_IsClosingConnect;

    [Tooltip("接続中のテキスト内容")] [Multiline(2)] [SerializeField]
    string _textContent_IsConnecting;

    [Tooltip("接続成功のテキスト内容")] [Multiline(2)] [SerializeField]
    string _textContent_SuccessToConnect;

    [Tooltip("接続失敗のテキスト内容")] [Multiline(2)] [SerializeField]
    string _textContent_FailToConnect;


    const int _minPortNum = 1;
    const int _maxPortNum = 16;
    const string _currentPortNumOnDisconnectMicon = "-";//マイコンと接続されていない時に表示する現在接続されているマイコンのポート番号

    SerialHandler _serialHandlerInstance;
    OptionDataSaveLoadManager _optionDataSaveLoadManagerInstance;
    WindowsWarningPlayer _windowsWarningPlayerInstance;
    SingleTaskCancellation _singleTaskCancellation = new();

    //パネルを閉じるボタンが押された時に現在のポート番号をポート番号指定欄に書いておく
    public void OnInputtedCloseButton()
    {
        ShowCurrentPortNum();
    }

    //入力終了時に呼ぶ
    public void OnEndEditPortNum(string input)
    {
        if (_serialHandlerInstance == null) return;
        if (_optionDataSaveLoadManagerInstance == null) return;
        if (_windowsWarningPlayerInstance == null) return;

        _singleTaskCancellation.Cancel();

        bool inputIsValid = int.TryParse(input, out int portNum) && MathfExtension.IsInRange(portNum, _minPortNum, _maxPortNum);

        
        if (!inputIsValid)//不正な入力がされた場合、弾く
        {
            ShowRequestToInvalidInputPanel();
        }
        else if(portNum == _serialHandlerInstance.CurrentPortNum)//入力したポート番号が既に接続中のポート番号と同じ場合、弾く
        {
            ShowRequestToSelectOtherPortPanel();
        }
        else//入力が正常であった場合、接続を試みる
        {
            var newCt = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());
            TryMiconPortConnectAsync(newCt, portNum).Forget();
        }
    }

    async UniTask TryMiconPortConnectAsync(CancellationToken ct,int portNum)
    {
        //パネルを表示しておく
        _optionResultCanvas.SetEnabledResultCanvas(true);
        _optionResultCanvas.SetActiveCloseButton(false);//通信が終了するまで閉じるボタンは隠しておく

        //通信中なら一度通信を閉じる
        if (_serialHandlerInstance.IsRunning)
        {
            _serialHandlerInstance.Close();
            _optionResultCanvas.SetResultText(_textContent_IsClosingConnect);
        }

        //通信が閉じるまで待つ
        await UniTask.WaitUntil(() => !_serialHandlerInstance.IsRunning, cancellationToken: ct);

        //念のため1フレーム待つ
        await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);

        //通信接続を開始
        _optionResultCanvas.SetResultText(_textContent_IsConnecting);
        bool isSuccessToConnect = _serialHandlerInstance.Open(portNum);

        //通信に成功
        if (isSuccessToConnect)
        {
            //オプションデータにセーブ
            _optionDataSaveLoadManagerInstance.SaveMiconPortNumber(portNum);

            _optionResultCanvas.SetResultText(_textContent_SuccessToConnect);
        }
        //通信に失敗
        else
        {
            _windowsWarningPlayerInstance.PlayWarning();

            _optionResultCanvas.SetResultText(_textContent_FailToConnect);
        }

        //通信接続が終了

        ShowCurrentPortNum();
        _optionResultCanvas.SetActiveCloseButton(true);//閉じるボタンを表示
    }

    //正常な入力するように要求する説明が書かれたパネルを表示
    void ShowRequestToInvalidInputPanel()
    {
        _windowsWarningPlayerInstance.PlayWarning();
        _optionResultCanvas.SetEnabledResultCanvas(true);

        _optionResultCanvas.SetActiveCloseButton(true);
        _optionResultCanvas.SetResultText(_textContent_InvalidInput);
    }

    //既に接続中のポート番号と同じポート番号を入力した場合に表示するパネル(別のポートを選択するように促す)
    void ShowRequestToSelectOtherPortPanel()
    {
        _windowsWarningPlayerInstance.PlayWarning();
        _optionResultCanvas.SetEnabledResultCanvas(true);

        _optionResultCanvas.SetActiveCloseButton(true);
        _optionResultCanvas.SetResultText(_textContent_AlreadyConnected);
    }

    async void Start()
    {
        // SerialHandlerのインスタンスを取得
        _serialHandlerInstance = await SerialHandler.GetInstanceAsync(this.GetCancellationTokenOnDestroy());

        //OptionDataSaveLoadManagerのインスタンスを取得
        _optionDataSaveLoadManagerInstance = await OptionDataSaveLoadManager.GetInstanceAsync(this.GetCancellationTokenOnDestroy());

        //WindowsWarningPlayerのインスタンスを取得
        _windowsWarningPlayerInstance = await WindowsWarningPlayer.GetInstanceAsync(this.GetCancellationTokenOnDestroy());

        if(_serialHandlerInstance == null)
        {
            Debug.LogError("SerialHandlerのインスタンスが取得できませんでした。");
            return;
        }

        ShowCurrentPortNum();
    }

    //現在のポート番号の表示
    void ShowCurrentPortNum()
    {
        if (_serialHandlerInstance == null) return;

        //現在接続中のポート番号を表示する
        if (_serialHandlerInstance.IsRunning)
        {
            int currentPortNum = _serialHandlerInstance.CurrentPortNum;
            _inputField.text = currentPortNum.ToString();
        }
        else//接続されていない場合
        {
            _inputField.text = _currentPortNumOnDisconnectMicon;
        }
    }
}
