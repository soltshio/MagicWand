using UnityEngine;

//作成者:杉山
//魔法球のマテリアルのプロパティ

[System.Serializable]
public struct MagicSphereMaterialProperty
{
    [Header("テクスチャ関係")]

    public Texture markTexture;

    [Header("色関係")]

    [ColorUsage(true, true)]
    public Color baseInEmissionColor;

    [ColorUsage(true, true)]
    public Color baseOutEmissionColor;

    [ColorUsage(true, true)]
    public Color markEmissionColor;
}
