using UnityEngine;

//作成者:杉山
//デフォルトの魔法球のマテリアルのプロパティ

[CreateAssetMenu(fileName = "DefaultMagicSphereMaterialProperty", menuName = "ScriptableObjects/Create DefaultMagicSphereMaterialProperty")]
public class DefaultMagicSphereMaterialProperty : ScriptableObject
{
    [SerializeField]
    MagicSphereMaterialProperty _property;

    public MagicSphereMaterialProperty Property { get { return _property; } }
}
