using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//MagicCircleActiveHandlerの魔法球の表示・非表示の処理をする

[System.Serializable]
public class MagicSphereRendererActivator_MagicCircleActiveHandler
{
    [SerializeField]
    MagicSpheresList _magicSphereList;

    [SerializeField] [Range(0,1)]
    float _activeBaseAlphaClipThreshold = 0.354f;

    [SerializeField] [Range(0, 1)]
    float _deActiveBaseAlphaClipThreshold = 1f;

    public void Start()
    {
        MagicSphereCollidersSwitchEnable(false);
    }

    //球をrate(0～1)に合わせて、だんだん表示させる(1で完全に表示)
    public void ActivateMagicSphere(float rate)
    {
        float newBaseAlphaClipThreshold = Mathf.Lerp(_deActiveBaseAlphaClipThreshold, _activeBaseAlphaClipThreshold, rate);

        SetAllMagicSpheresBaseAlphaClipThreshold(newBaseAlphaClipThreshold);
    }

    //球をrate(0～1)に合わせて、だんだん非表示にさせる(1で完全に非表示)
    public void DeactivateMagicSphere(float rate)
    {
        float newBaseAlphaClipThreshold = Mathf.Lerp(_activeBaseAlphaClipThreshold, _deActiveBaseAlphaClipThreshold, rate);

        SetAllMagicSpheresBaseAlphaClipThreshold(newBaseAlphaClipThreshold);
    }

    void SetAllMagicSpheresBaseAlphaClipThreshold(float newBaseAlphaClipThreshold)
    {
        var magicSphereMaterialControllers = _magicSphereList.GetComponentsArrayFromMagicSpheres<MagicSphereMaterialController>();

        foreach(var magicSphere in magicSphereMaterialControllers)
        {
            if (magicSphere == null) continue;

            magicSphere.SetBaseAlphaClipThreshold(newBaseAlphaClipThreshold);
        }
    }

    //球の当たり判定をオンにする
    public void MagicSphereCollidersSwitchEnable(bool isEnable)
    {
        var _magicSphereColliders = _magicSphereList.GetComponentsArrayFromMagicSpheres<SphereCollider>();

        for (int i = 0; i < _magicSphereColliders.Length; i++)
        {
            _magicSphereColliders[i].enabled = isEnable;
        }
    }
}
