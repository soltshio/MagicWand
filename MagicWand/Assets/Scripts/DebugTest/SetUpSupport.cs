using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SetUpSupport : MonoBehaviour
{
    [SerializeField]
    InputActionReference _showAction;

    [SerializeField]
    Image setUpImage;


    private void Awake()
    {
        _showAction.action.performed += SwitchShow;


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setUpImage.enabled = false;

    }

    void SwitchShow(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        setUpImage.enabled = !setUpImage.enabled;
    }

}
