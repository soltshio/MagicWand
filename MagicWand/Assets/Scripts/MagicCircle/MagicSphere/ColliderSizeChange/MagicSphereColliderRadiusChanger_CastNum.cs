using System;
using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//何個目の魔法球に触れたかによって、魔法球の当たり判定の大きさを変更する

public class MagicSphereColliderRadiusChanger_CastNum : MonoBehaviour
{
    [SerializeField]
    MagicCircleCastManager _magicCircleCastManager;

    [SerializeField]
    MagicSpheresList _magicSphereList;

    [SerializeField]
    float _defaultRadius=0.5f;

    [SerializeField]
    float _radiusMoreThanTwice=1f;

    private void OnEnable()
    {
        _magicCircleCastManager.OnSuccessToCast += ChangeRadius;
        _magicCircleCastManager.OnStartToCast += ResetRadius;
    }

    private void OnDisable()
    {
        _magicCircleCastManager.OnSuccessToCast -= ChangeRadius;
        _magicCircleCastManager.OnStartToCast -= ResetRadius;
    }

    void ResetRadius()
    {
        SetAllMagicSphereRadius(_defaultRadius);
    }

    void ChangeRadius(EMagic magic,int magicSphereNum)
    {
        SetAllMagicSphereRadius(_radiusMoreThanTwice);
    }

    void SetAllMagicSphereRadius(float radius)
    {
        var _magicSphereColliders = _magicSphereList.GetComponentsArrayFromMagicSpheres<SphereCollider>();

        for (int i = 0; i < _magicSphereColliders.Length; i++)
        {
            _magicSphereColliders[i].radius = radius;
        }
    }
}
