using UnityEngine;

//作成者:杉山
//魔法球のマテリアルの制御をするクラス

public class MagicSphereMaterialController : MonoBehaviour
{
    [SerializeField]
    Renderer _renderer;

    [SerializeField]
    Color _defaultColor;

    [SerializeField] [Range(0, 1)]
    float _defaultAlpha = 0;

    Material _sphereMat;

    static readonly int _baseColorID = Shader.PropertyToID("_BaseColor");
    static readonly int _alphaID = Shader.PropertyToID("_Alpha");

    public void SetAlpha(float alpha)
    {
        alpha = Mathf.Clamp01(alpha);
        _sphereMat.SetFloat(_alphaID, alpha);
    }

    public void SetColor(Color color)
    {
        _sphereMat.SetColor(_baseColorID, color);
    }

    void Awake()
    {
        _sphereMat = _renderer.material;
    }

    void Start()
    {
        _sphereMat.SetColor(_baseColorID, _defaultColor);
        _sphereMat.SetFloat(_alphaID, _defaultAlpha);
    }
}
