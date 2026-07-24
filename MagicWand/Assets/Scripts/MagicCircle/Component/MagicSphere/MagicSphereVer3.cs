using UnityEngine;

//作成者：杉山
//魔法陣上の球

public class MagicSphereVer3 : MonoBehaviour
{
    [SerializeField]
    Renderer _renderer;

    [SerializeField]
    Color _deactiveColor;

    [SerializeField] [Range(0, 1)]
    float _defaultAlpha=0;

    Material _sphereMat;

    bool _isActive;

    static readonly int _baseColorID = Shader.PropertyToID("_BaseColor");
    static readonly int _alphaID = Shader.PropertyToID("_Alpha");

    public bool IsActive { get => _isActive; }

    public void ToActive(Color activeColor)
    {
        if (_isActive == true) return;
        
        _isActive = true;
        _sphereMat.SetColor(_baseColorID, activeColor);
    }

    public void ToDeactive()
    {
        if (_isActive == false) return;
        
        _isActive = false;
        _sphereMat.SetColor(_baseColorID, _deactiveColor);
    }

    public void SetAlpha(float alpha)
    {
        alpha = Mathf.Clamp01(alpha);
        _sphereMat.SetFloat(_alphaID, alpha);
    }

    void Awake()
    {
        _isActive = false;

        _sphereMat = _renderer.material;
    }

    void Start()
    {
        _sphereMat.SetColor(_baseColorID, _deactiveColor);
        _sphereMat.SetFloat(_alphaID, _defaultAlpha);
    }
}
