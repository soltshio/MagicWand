using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverUI : MonoBehaviour
{
    [SerializeField]
    HoverAutoClickButton_Test _hoverAutoClickButton;

    [SerializeField]
    Image _hoverProgressGauge;

    void Start()
    {
        _hoverProgressGauge.enabled = false;
    }

    void OnEnable()
    {
        _hoverAutoClickButton.OnAutoClick += HideGauge;
        _hoverAutoClickButton.OnCursorExit += HideGauge;
        _hoverAutoClickButton.OnCursorEnter += ShowGauge;
    }

    void OnDisable()
    {
        _hoverAutoClickButton.OnAutoClick -= HideGauge;
        _hoverAutoClickButton.OnCursorExit -= HideGauge;
        _hoverAutoClickButton.OnCursorEnter -= ShowGauge;
    }

    void Update()
    {
        if (!_hoverProgressGauge.enabled) return;

        float gaugeAmount = _hoverAutoClickButton.HoveringTime / _hoverAutoClickButton.HoverDurationToClick;

        _hoverProgressGauge.fillAmount = gaugeAmount;
    }

    void ShowGauge()
    {
        _hoverProgressGauge.enabled = true;
    }

    void HideGauge()
    {
        _hoverProgressGauge.enabled = false;
    }
}
