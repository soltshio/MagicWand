using TMPro;
using UnityEngine;
using UnityEngine.UI;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンがホバー中(カーソルが合わせられている時)にホバー時間の割合をゲージで表示する
//Maskを利用し、Imageの位置を操作することで下から上に色が変わっていくようにする

public class ButtonHoveringViewUI_MaskImagePos : MonoBehaviour
{
    [SerializeField]
    HoverAutoClickButton _hoverAutoClickButton;

    [SerializeField]
    TextMeshProUGUI _buttonText;

    [SerializeField]
    Color _fillColor;

    bool _isHovering = false;

    Material _material;

    static readonly int FillAmountID = Shader.PropertyToID("_FillAmount");

    static readonly int FillColorID = Shader.PropertyToID("_FillColor");

    void Awake()
    {
        // インスタンス化されたマテリアルを取得
        _material = _buttonText.fontMaterial;
    }

    void Start()
    {
        //色を変えておく
        _material.SetColor(FillColorID, _fillColor);
    }

    void OnValidate()
    {
        //実行時以外は無視
        if (!Application.isPlaying) return;
        
        if (_buttonText == null) return;
        if (_material == null) return;

        _material.SetColor(FillColorID, _fillColor);
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

        _material.SetFloat(FillAmountID, colorImageRate);
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
            _material.SetFloat(FillAmountID, 0f);
        }
    }
}
