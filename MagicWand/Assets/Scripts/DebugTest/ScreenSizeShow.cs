using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenSizeShow : MonoBehaviour
{
    [SerializeField]
    InputActionReference _showAction;

    [SerializeField]
    TextMeshProUGUI _text;

    bool _isShow = false;

    void Start()
    {
        _text.enabled = false;
    }

    void OnEnable()
    {
        _showAction.action.performed += Show;
        _showAction.action.canceled += Hide;
        _showAction.action.Enable();
    }

    private void OnDisable()
    {
        _showAction.action.performed -= Show;
        _showAction.action.canceled += Hide;
        _showAction.action.Disable();
    }

    void Show(InputAction.CallbackContext context)
    {
        _isShow = true;
        _text.enabled = true;
    }

    void Hide(InputAction.CallbackContext context)
    {
        _isShow = false;
        _text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isShow) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        string textCont = Screen.width + ":" + Screen.height+"\n"+mouseScreenPos.x.ToString("0") +":"+mouseScreenPos.y.ToString("0");
        _text.text = textCont;
    }
}
