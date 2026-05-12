using UnityEngine;

//作成者：杉山
//魔法陣上の球

public class MagicSphereVer3 : MonoBehaviour
{
    [SerializeField]
    Renderer _renderer;

    [SerializeField]
    Material _deactiveMat;

    bool _isActive;

    public bool IsActive { get => _isActive; }

    public void ToActive(Material material)
    {
        if (_isActive == true) return;
        
        _isActive = true;
        _renderer.material = material;
    }

    public void ToDeactive()
    {
        if (_isActive == false) return;
        
        _isActive = false;
        _renderer.material = _deactiveMat;
    }

    void Awake()
    {
        _isActive = false;
        _renderer.material = _deactiveMat;
    }

}
