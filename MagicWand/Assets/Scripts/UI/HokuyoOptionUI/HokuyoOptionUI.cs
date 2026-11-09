using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

//作成者:杉山
//測距(北陽)レーザーのオプションUI

public class HokuyoOptionUI : MonoBehaviour
{
    [Tooltip("入力欄に入力できる最大文字数")] [SerializeField]
    int _maxInputLength = 7;

    [SerializeField]
    TMP_InputField _centerXInputField;

    [SerializeField]
    TMP_InputField _centerYInputField;

    [SerializeField]
    TMP_InputField _sizeXInputField;

    [SerializeField]
    TMP_InputField _sizeYInputField;


    [Tooltip("センサーからの値が受け取れない時に表示するUI")] [SerializeField]
    GameObject _noSignalUI;

    [Tooltip("調整UI(センサーからの値が受け取れている時に表示するUI)")] [SerializeField]
    GameObject _sensorOptionUI;

    [SerializeField]
    OptionResultCanvas _optionResultCanvas;


    [Header("不正な入力がされた時の設定")]

    [Tooltip("不正な入力がされた時に表示するテキスト内容")] [Multiline(2)] [SerializeField]
    string _textContent_InvalidInput;

    HokuyoDataReceiver _hokuyoDataReceiverInstance;
    HokuyoDataTransmitter _hokuyoDataTransmitterInstance;
    WindowsWarningPlayer _windowsWarningPlayerInstance;

    //中心Xへの入力
    public void OnEndEditCenterX(string input)
    {
        if (_hokuyoDataReceiverInstance == null) return;

        if (!IsValidInput(input, out float newCenterX)) return;

        Vector2 newCenter = _hokuyoDataReceiverInstance.CenterM;
        newCenter.x = newCenterX;

        SendCenterValue(newCenter);
    }

    //中心Yへの入力
    public void OnEndEditCenterY(string input)
    {
        if (_hokuyoDataReceiverInstance == null) return;

        if (!IsValidInput(input, out float newCenterY)) return;

        Vector2 newCenter = _hokuyoDataReceiverInstance.CenterM;
        newCenter.y = newCenterY;

        SendCenterValue(newCenter);

    }

    //サイズXへの入力
    public void OnEndEditSizeX(string input)
    {
        if (_hokuyoDataReceiverInstance == null) return;

        if (!IsValidInput(input, out float newSizeX)) return;

        Vector2 newSize = _hokuyoDataReceiverInstance.SizeM;
        newSize.x = newSizeX;

        SendSizeValue(newSize);
    }

    //サイズYへの入力
    public void OnEndEditSizeY(string input)
    {
        if (_hokuyoDataReceiverInstance == null) return;

        if (!IsValidInput(input, out float newSizeY)) return;

        Vector2 newSize = _hokuyoDataReceiverInstance.SizeM;
        newSize.y = newSizeY;

        SendSizeValue(newSize);
    }


    //入力が正常か
    bool IsValidInput(string input,out float value)
    {
        bool inputIsValid = float.TryParse(input, out value);

        //入力が数字でなかったら弾く
        if(!inputIsValid)
        {
            ShowRequestToInvalidInputPanel();
            return false;
        }

        return true;
    }

    async void OnEnable()
    {
        //インスタンスが取得できていなければ取得する
        if(_hokuyoDataReceiverInstance == null) _hokuyoDataReceiverInstance = await HokuyoDataReceiver.GetInstanceAsync(this.GetCancellationTokenOnDestroy());
        if(_hokuyoDataTransmitterInstance == null) _hokuyoDataTransmitterInstance = await HokuyoDataTransmitter.GetInstanceAsync(this.GetCancellationTokenOnDestroy());
        if(_windowsWarningPlayerInstance == null) _windowsWarningPlayerInstance = await WindowsWarningPlayer.GetInstanceAsync(this.GetCancellationTokenOnDestroy());

        _hokuyoDataReceiverInstance.OnSwitchIsRunning += SwitchNoSignalUI;
    }

    void OnDisable()
    {
        _hokuyoDataReceiverInstance.OnSwitchIsRunning -= SwitchNoSignalUI;
    }

    void Start()
    {
        //初期値を入力欄に表示しておく
        _centerXInputField.characterLimit = _maxInputLength;
        _centerYInputField.characterLimit = _maxInputLength;
        _sizeXInputField.characterLimit = _maxInputLength;
        _sizeYInputField.characterLimit = _maxInputLength;
    }

    //測距センサーからデータを受け取れていない場合は設定UIを表示させないようにする
    void SwitchNoSignalUI(bool isRunning)
    {
        _sensorOptionUI.SetActive(isRunning);
        _noSignalUI.SetActive(!isRunning);
    }

    void Update()
    {
        if (!_hokuyoDataReceiverInstance.IsRunning) return;

        //現在の中心位置を更新
        _centerXInputField.text = CurrentValueToInputFieldText(_hokuyoDataReceiverInstance.CenterM.x);
        _centerYInputField.text = CurrentValueToInputFieldText(_hokuyoDataReceiverInstance.CenterM.y);
        //現在のサイズを更新
        _sizeXInputField.text = CurrentValueToInputFieldText(_hokuyoDataReceiverInstance.SizeM.x);
        _sizeYInputField.text = CurrentValueToInputFieldText(_hokuyoDataReceiverInstance.SizeM.y);
    }

    //現在の値を入力欄に表示する文字列に変換する
    string CurrentValueToInputFieldText(float currentValue)
    {
        string currentValueText = currentValue.ToString();
        currentValueText = currentValueText.Substring(0, Mathf.Min(currentValueText.Length, _maxInputLength));
        return currentValueText;
    }

    //正常な入力するように要求する説明が書かれたパネルを表示
    void ShowRequestToInvalidInputPanel()
    {
        _windowsWarningPlayerInstance.PlayWarning();
        _optionResultCanvas.SetEnabledResultCanvas(true);

        _optionResultCanvas.SetActiveCloseButton(true);
        _optionResultCanvas.SetResultText(_textContent_InvalidInput);
    }

    void SendCenterValue(Vector2 center)
    {
        if (_hokuyoDataTransmitterInstance == null) return;

        _hokuyoDataTransmitterInstance.SendCenterM(center);//送信
    }

    void SendSizeValue(Vector2 size)
    {
        if (_hokuyoDataTransmitterInstance == null) return;

        _hokuyoDataTransmitterInstance.SendSizeM(size);//送信
    }
}
