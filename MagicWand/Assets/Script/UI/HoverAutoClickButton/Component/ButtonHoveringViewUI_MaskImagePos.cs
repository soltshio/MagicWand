using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンがホバー中(カーソルが合わせられている時)にホバー時間の割合をゲージで表示する
//Maskを利用し、Imageの位置を操作することで下から上に色が変わっていくようにする

public class ButtonHoveringViewUI_MaskImagePos : MonoBehaviour
{
    [SerializeField]
    HoverAutoClickButton _hoverAutoClickButton;

    [Tooltip("色のImageの位置情報")] [SerializeField]
    RectTransform _colorImagePos;

    [Tooltip("色が完全に変わった時の色のImageの位置")] [SerializeField]
    float _upPos;

    [Tooltip("色が全く染まっていない時の色のImageの位置")] [SerializeField]
    float _downPos;

    bool _isHovering = false;

    void Start()
    {
        //色の位置を一番下にしておく
        _colorImagePos.localPosition = new Vector3(_colorImagePos.localPosition.x, _downPos, _colorImagePos.localPosition.z);
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
        if (!_isHovering) return;

        float colorImageRate = _hoverAutoClickButton.HoveringTime / _hoverAutoClickButton.Parameter.HoverDurationToClick;

        _colorImagePos.localPosition = new Vector3(_colorImagePos.localPosition.x, Mathf.Lerp(_downPos, _upPos, colorImageRate), _colorImagePos.localPosition.z);
    }

    void OnButtonStateChanged(HoverAutoClickButtonEState state)
    {
        if (state == HoverAutoClickButtonEState.Hovering)
        {
            _isHovering = true;
        }
        else
        {
            _isHovering = false;
            //色の位置を一番下に戻しておく
            _colorImagePos.localPosition = new Vector3(_colorImagePos.localPosition.x, _downPos, _colorImagePos.localPosition.z);
        }
    }
}
