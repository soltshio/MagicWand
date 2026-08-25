using UnityEngine;
using UnityEngine.UI;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンがホバー中(カーソルが合わせられている時)にホバー時間の割合をゲージで表示する
//ImageのScaleを操作するタイプ

public class ButtonHoveringViewUI_DeployMagicCircleUI : MonoBehaviour
{
    [SerializeField]
    HoverAutoClickButton _hoverAutoClickButton;

    [SerializeField]
    Image _hoverProgressGauge;

    [SerializeField]
    Animator _hoverAutoClickButtonAnimator;

    void OnEnable()
    {
        _hoverProgressGauge.transform.localScale = new Vector3(0f, 0f, 0f);

        _hoverAutoClickButton.OnStateChanged += OnButtonStateChanged;
    }

    void OnDisable()
    {
        _hoverAutoClickButton.OnStateChanged -= OnButtonStateChanged;
    }

    void Update()
    {
        if (_hoverAutoClickButton.CurrentState != HoverAutoClickButtonEState.Hovering) return;

        float gaugeAmount = _hoverAutoClickButton.HoveringTime / _hoverAutoClickButton.Parameter.HoverDurationToClick;

        _hoverProgressGauge.transform.localScale = new Vector3(gaugeAmount, gaugeAmount, gaugeAmount);
    }

    void OnButtonStateChanged(HoverAutoClickButtonEState state)
    {
        if(state == HoverAutoClickButtonEState.Clicked)
        {
            //だんだんUIが消えていくようにする
            _hoverAutoClickButtonAnimator.SetTrigger(DeployMagicCircleUIAnimatorProperty.HideTriggerName);
        }
    }
}
