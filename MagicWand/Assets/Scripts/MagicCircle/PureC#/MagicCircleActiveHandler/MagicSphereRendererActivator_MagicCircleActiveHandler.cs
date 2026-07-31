using UnityEngine;

//作成者:杉山
//MagicCircleActiveHandlerの魔法球の表示・非表示の処理をする

[System.Serializable]
public class MagicSphereRendererActivator_MagicCircleActiveHandler
{
    [Tooltip("魔法陣の球の当たり判定")] [SerializeField]
    Collider[] _magicSphereColliders;

    [SerializeField]
    MagicSpheresList _magicSphereList;

    public void Start()
    {
        for (int i = 0; i < _magicSphereColliders.Length; i++)
        {
            _magicSphereColliders[i].enabled = false;
        }
    }

    //球をrate(0～1)に合わせて、だんだん表示させる(1で完全に表示)
    public void ActivateMagicSphere(float rate)
    {
        SetAllMagicSpheresAlpha(rate);
    }

    //球をrate(0～1)に合わせて、だんだん非表示にさせる(1で完全に非表示)
    public void DeactivateMagicSphere(float rate)
    {
        SetAllMagicSpheresAlpha(1f - rate);
    }

    void SetAllMagicSpheresAlpha(float alpha)
    {
        var magicSpheres = _magicSphereList.GetComponentsArrayFromMagicSpheres<MagicSphereVer3>();

        foreach(var magicSphere in magicSpheres)
        {
            if (magicSphere == null) continue;

            magicSphere.SetAlpha(alpha);
        }
    }

    //球の当たり判定をオンにする
    public void MagicSphereCollidersSwitchEnable(bool isEnable)
    {
        for (int i = 0; i < _magicSphereColliders.Length; i++)
        {
            _magicSphereColliders[i].enabled = isEnable;
        }
    }
}
