using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverUI : MonoBehaviour
{
    [SerializeField]
    HoverAutoClickButton _hoverAutoClickButton;

    [SerializeField]
    Image _hoverProgressGauge;

    void Start()
    {
        _hoverProgressGauge.enabled = false;
    }

    void OnEnable()
    {
        _hoverAutoClickButton.OnStateChanged += OnButtonStateChanged;
    }

    void OnDisable()
    {
        _hoverAutoClickButton.OnStateChanged -= OnButtonStateChanged;
    }

    void Update()
    {
        if (!_hoverProgressGauge.enabled) return;

        float gaugeAmount = _hoverAutoClickButton.HoveringTime / _hoverAutoClickButton.Parameter.HoverDurationToClick;

        _hoverProgressGauge.fillAmount = gaugeAmount;
    }

    void OnButtonStateChanged(HoverAutoClickButtonEState state)
    {
        if(state == HoverAutoClickButtonEState.Hovering)
        {
            _hoverProgressGauge.enabled = true;
        }
        else
        {
            _hoverProgressGauge.enabled = false;
        }
    }
}
