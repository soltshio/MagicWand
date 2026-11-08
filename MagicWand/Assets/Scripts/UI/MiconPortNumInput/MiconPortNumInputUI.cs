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
    Canvas _miconPortResultCanvas;

    [SerializeField]
    TextMeshProUGUI _resultText;

    [SerializeField]
    Button _closeButton;

    [Header("不正な入力がされた時の設定")]

    [Tooltip("不正な入力がされた時に表示するテキスト内容")] [SerializeField]
    string _textContext_InvalidInput;

    SerialHandler _serialHandlerInstance;
    SingleTaskCancellation _singleTaskCancellation = new();

    public void OnEndEditPortNum(string input)
    {
        if (_serialHandlerInstance == null) return;

        _singleTaskCancellation.Cancel();

        bool inputIsValid = int.TryParse(input, out int portNum);

        //不正な入力がされた場合、弾く
        if (!inputIsValid)
        {
            ShowRequestToInvalidInputPanel();
        }
        //入力が正常であった場合、接続を試みる
        else
        {
            var newCt = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());
        }
    }

    async UniTask TryMiconPortConnectAsync(CancellationToken ct)
    {

    }

    //正常な入力するように要求する説明が書かれたパネルを表示
    void ShowRequestToInvalidInputPanel()
    {
        _miconPortResultCanvas.enabled = true;
        
        _closeButton.gameObject.SetActive(true);
        _resultText.text = _textContext_InvalidInput;
    }

    async void Start()
    {
        //リザルトパネルのキャンバスは非表示にしておく
        _miconPortResultCanvas.enabled = false;

        // SerialHandlerのインスタンスを取得
        _serialHandlerInstance = await SerialHandler.GetInstanceAsync(this.GetCancellationTokenOnDestroy());
    }
}
