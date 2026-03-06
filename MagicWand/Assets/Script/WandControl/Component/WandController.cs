using UnityEngine;
using UnityEngine.InputSystem;

//ì¬Ò:™R
//ñ‚ğ§Œä‚·‚éƒNƒ‰ƒX

public class WandController : MonoBehaviour
{
    [SerializeField]
    MovingAveragedJoyconOrientation _movingAveragedJoyconOrientation;

    [Tooltip("ñ(“®‚©‚·‘ÎÛ)")] [SerializeField]
    Transform _wand;

    Quaternion _originJoyconOrientation = Quaternion.identity;

    Quaternion _currentRot=Quaternion.identity;

    public void ResetPos(InputAction.CallbackContext context)//‰ñ“]‚ğƒŠƒZƒbƒg
    {
        if (!context.performed) return;

        _originJoyconOrientation= _movingAveragedJoyconOrientation.SmoothedOrientation * Quaternion.AngleAxis(90f,Vector3.right);
    }

    private void Awake()
    {
        _currentRot = Quaternion.identity;
    }

    private void Update()
    {
        Quaternion newRot;

        var joyconOrientation = _movingAveragedJoyconOrientation.SmoothedOrientation;

        //Šî€‚Ì‰ñ“]‚Æ‚ÌŒvZ
        joyconOrientation = Quaternion.Inverse(_originJoyconOrientation) * joyconOrientation;

        //y²‰ñ“]‚Æz²‰ñ“]‚ğ“ü‚ê‘Ö‚¦‚é
        Quaternion c = Quaternion.AngleAxis(90f, Vector3.right);

        newRot = c * joyconOrientation * Quaternion.Inverse(c);

        //ñ‚ğ‰ñ“]‚³‚¹‚é
        _currentRot = newRot;

        _wand.localRotation = _currentRot;
    }
}
