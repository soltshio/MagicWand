using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchScreenModeManager : MonoBehaviour
{
    [SerializeField]
    InputActionReference _switchAction;

    void OnEnable()
    {
        _switchAction.action.performed += SwitchScreenMode;
        _switchAction.action.Enable();
    }

    private void OnDisable()
    {
        _switchAction.action.performed -= SwitchScreenMode;
        _switchAction.action.Disable();
    }

    void SwitchScreenMode(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
}
