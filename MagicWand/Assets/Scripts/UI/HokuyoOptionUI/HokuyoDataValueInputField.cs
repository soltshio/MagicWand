using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

//作成者:杉山
//測距センサーの設定UIの入力欄

public class HokuyoDataValueInputField : MonoBehaviour
{
    [Tooltip("入力欄に入力できる最大文字数")] [SerializeField]
    int _maxInputLength = 7;

    [SerializeField]
    TMP_InputField _inputField;

    [SerializeField]
    OptionResultCanvas _optionResultCanvas;

    [Tooltip("不正な入力がされた時に表示するテキスト内容")] [Multiline(2)] [SerializeField]
    string _textContent_InvalidInput="数字を入力してください。";

    WindowsWarningPlayer _windowsWarningPlayerInstance;

    bool _isSelected = false;

    public event Action<float> OnInputtedValue;

    public void OnSelect()
    {
        _isSelected = true;
    }

    public void OnDeselect()
    {
        _isSelected = false;
    }

    public void OnEndEditValue(string input)
    {
        bool inputIsValid = float.TryParse(input, out float value);

        //不正な入力がされた場合は、正常な入力をするように警告を出す
        if (!inputIsValid)
        {
            ShowRequestToInvalidInputPanel();
        }
        else//正常な入力であればfloat型に変換してイベントを発火
        {
            OnInputtedValue?.Invoke(value);
        }
    }

    public void SetInputFieldText(float value)
    {
        //選択状態では更新をしないようにする
        if (_isSelected) return;

        string valueStr = value.ToString();
        valueStr = valueStr.Substring(0, Mathf.Min(valueStr.Length, _maxInputLength));
        _inputField.text = valueStr;
    }

    async void OnEnable()
    {
        //インスタンスが取得できていなければ取得する
        if (_windowsWarningPlayerInstance == null) _windowsWarningPlayerInstance = await WindowsWarningPlayer.GetInstanceAsync(this.GetCancellationTokenOnDestroy());
    }

    void Start()
    {
        _inputField.characterLimit = _maxInputLength;
    }

    //正常な入力するように要求する説明が書かれたパネルを表示
    void ShowRequestToInvalidInputPanel()
    {
        _windowsWarningPlayerInstance.PlayWarning();
        _optionResultCanvas.SetEnabledResultCanvas(true);

        _optionResultCanvas.SetActiveCloseButton(true);
        _optionResultCanvas.SetResultText(_textContent_InvalidInput);
    }
}
