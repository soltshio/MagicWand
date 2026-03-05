using UnityEngine;

//ì¬Ò:™R
//ñ‚ğ§Œä‚·‚éƒNƒ‰ƒX

public class WandController : MonoBehaviour
{
    [SerializeField] 
    JoyconInputManager _joyconInputManager;

    [Tooltip("ñ(“®‚©‚·‘ÎÛ)")] [SerializeField]
    Transform _wand;

    Quaternion originRot = Quaternion.identity;//Šî€‚Ì•ûŒü

    private void Update()
    {
        var orientation = _joyconInputManager.Orientation;

        //y²‰ñ“]‚Æz²‰ñ“]‚ğ“ü‚ê‘Ö‚¦‚é
        Quaternion c = Quaternion.AngleAxis(90f, Vector3.right);

        _wand.rotation = c * orientation * Quaternion.Inverse(c);

        //Šî€‚Ì•ûŒü‚É‡‚í‚¹‚é
        _wand.rotation = _wand.rotation * originRot;
    }
}
