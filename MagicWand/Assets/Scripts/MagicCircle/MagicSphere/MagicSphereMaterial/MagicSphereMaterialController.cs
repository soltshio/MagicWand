using UnityEngine;

//作成者:杉山
//魔法球のマテリアルの制御をするクラス

public class MagicSphereMaterialController : MonoBehaviour
{
    [SerializeField]
    Renderer _renderer;

    [SerializeField]
    DefaultMagicSphereMaterialProperty _defaultMaterialProperty;

    [SerializeField] [Range(0, 1)]
    float _defaultBaseAlphaClipThreshold;

    [SerializeField] [Range(0, 1)]
    float _defaultMarkAlphaClipThreshold;

    Material _sphereMat;

    //色関係
    static readonly int _markTextureID = Shader.PropertyToID("_MarkTexture");
    static readonly int _baseInEmissionColorID = Shader.PropertyToID("_BaseInEmissionColor");
    static readonly int _baseOutEmissionColorID = Shader.PropertyToID("_BaseOutEmissionColor");
    static readonly int _markEmissionColorID = Shader.PropertyToID("_MarkEmissionColor");
    //alphaClipThreshold関係
    static readonly int _baseAlphaClipThresholdID = Shader.PropertyToID("_BaseAlphaClipThreshold");
    static readonly int _markAlphaClipThresholdID = Shader.PropertyToID("_MarkAlphaClipThreshold");

    //プロパティの取得
    //色関係
    public Texture CurrentMarkTexture { get { return _sphereMat.GetTexture(_markTextureID); } }
    public Color CurrentBaseInEmissionColor { get { return _sphereMat.GetColor(_baseInEmissionColorID); } }
    public Color CurrentBaseOutEmissionColor { get { return _sphereMat.GetColor(_baseOutEmissionColorID); } }
    public Color CurrentMarkEmissionColor { get { return _sphereMat.GetColor(_markEmissionColorID); } }
    //alphaClipThreshold関係
    public float CurrentBaseAlphaClipThreshold { get { return _sphereMat.GetFloat(_baseAlphaClipThresholdID); } }
    public float CurrentMarkAlphaClipThreshold { get { return _sphereMat.GetFloat(_markAlphaClipThresholdID); } }


    //本体部分のalphaClipThresholdの変更
    //newBaseAlphaClipThresholdは0～1
    public void SetBaseAlphaClipThreshold(float newBaseAlphaClipThreshold)
    {
        _sphereMat.SetFloat(_baseAlphaClipThresholdID, newBaseAlphaClipThreshold);
    }

    //マーク部分のalphaClipThresholdの変更
    //newMarkAlphaClipThresholdは0～1
    public void SetMarkAlphaClipThreshold(float newMarkAlphaClipThreshold)
    {
        _sphereMat.SetFloat(_markAlphaClipThresholdID, newMarkAlphaClipThreshold);
    }

    //newPropertyのテクスチャの部分だけを変更
    public void SetTexture(MagicSphereMaterialProperty newProperty)
    {
        _sphereMat.SetTexture(_markTextureID, newProperty.markTexture);
    }

    //newPropertyの色の部分だけを変更
    public void SetColor(MagicSphereMaterialProperty newProperty)
    {
        _sphereMat.SetColor(_baseInEmissionColorID, newProperty.baseInEmissionColor);
        _sphereMat.SetColor(_baseOutEmissionColorID, newProperty.baseOutEmissionColor);
        _sphereMat.SetColor(_markEmissionColorID, newProperty.markEmissionColor);
    }

    void Awake()
    {
        _sphereMat = _renderer.material;
    }

    void Start()
    {
        SetColor(_defaultMaterialProperty.Property);
        SetTexture(_defaultMaterialProperty.Property);
        SetBaseAlphaClipThreshold(_defaultBaseAlphaClipThreshold);
        SetMarkAlphaClipThreshold(_defaultMarkAlphaClipThreshold);
    }
}
