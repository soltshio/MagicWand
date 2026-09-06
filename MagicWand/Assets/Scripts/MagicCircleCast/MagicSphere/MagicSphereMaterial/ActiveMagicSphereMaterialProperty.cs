using UnityEngine;

//作成者:杉山
//なぞる誘導が出た際の魔法球のマテリアルのプロパティ

public class ActiveMagicSphereMaterialProperty : MonoBehaviour
{
    [SerializeField]
    MagicSphereMaterialProperty _activeMagicSphereMaterialProperty;

    public MagicSphereMaterialProperty ActiveMaterialProperty { get { return _activeMagicSphereMaterialProperty; } }//魔法球の色
}
