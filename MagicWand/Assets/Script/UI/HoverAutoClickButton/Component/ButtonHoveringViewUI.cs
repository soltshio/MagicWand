using UnityEngine;
using UnityEngine.UI;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンがホバー中(カーソルが合わせられている時)にホバー時間の割合をゲージで表示する

public class ButtonHoveringViewUI : MonoBehaviour
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
