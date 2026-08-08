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

    public MagicSphereMaterialProperty(Texture markTexture, Color baseInEmissionColor, Color baseOutEmissionColor, Color markEmissionColor)
    {
        this.markTexture = markTexture;
        this.baseInEmissionColor = baseInEmissionColor;
        this.baseOutEmissionColor = baseOutEmissionColor;
        this.markEmissionColor = markEmissionColor;
    }

    //色のみの補間
    public static MagicSphereMaterialProperty ColorLerp(MagicSphereMaterialProperty a, MagicSphereMaterialProperty b, float t)
    {
        return new MagicSphereMaterialProperty(
            a.markTexture,
            Color.Lerp(a.baseInEmissionColor, b.baseInEmissionColor, t),
            Color.Lerp(a.baseOutEmissionColor, b.baseOutEmissionColor, t),
            Color.Lerp(a.markEmissionColor, b.markEmissionColor, t)
        );
    }
}
