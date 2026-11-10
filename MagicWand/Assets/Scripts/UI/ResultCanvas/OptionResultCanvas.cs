using TMPro;
using UnityEngine;
using UnityEngine.UI;

//作成者:杉山
//オプションの設定結果の表示Canvas

public class OptionResultCanvas : MonoBehaviour
{
    [SerializeField]
    Canvas _miconPortResultCanvas;

    [SerializeField]
    TextMeshProUGUI _resultText;

    [SerializeField]
    Button _closeButton;

    public void SetActiveCloseButton(bool isActive)
    {
        _closeButton.gameObject.SetActive(isActive);
    }

    public void SetResultText(string text)
    {
        _resultText.text = text;
    }

    public void SetEnabledResultCanvas(bool isEnabled)
    {
        _miconPortResultCanvas.enabled = isEnabled;
    }

    void Start()
    {
        //リザルトパネルのキャンバスは非表示にしておく
        SetEnabledResultCanvas(false);
    }
}
