using System;
using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//何個目の魔法球に触れたかによって、魔法球の当たり判定の大きさを変更する

public class MagicSphereColliderRadiusChanger_CastNum : MonoBehaviour
{
    [SerializeField]
    MagicCircleCastManager_New _magicCircleCastManager;

    [SerializeField]
    MagicCircleSelectManager _magicCircleSelectManager;

    [SerializeField]
    MagicSpheresList _magicSphereList;

    [SerializeField]
    float _defaultRadius=0.5f;

    [SerializeField]
    float _radiusMoreThanTwice=1f;

    private void OnEnable()
    {
        _magicCircleCastManager.OnSuccessToCast += ChangeRadius;
        _magicCircleSelectManager.OnSelectMagic += ResetRadius;
    }

    private void OnDisable()
    {
        _magicCircleCastManager.OnSuccessToCast -= ChangeRadius;
        _magicCircleSelectManager.OnSelectMagic -= ResetRadius;
    }

    void ResetRadius(EMagic magic)
    {
        SetAllMagicSphereRadius(_defaultRadius);
    }

    void ChangeRadius(EMagic magic,int magicSphereNum,int touchSphereCount)
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
