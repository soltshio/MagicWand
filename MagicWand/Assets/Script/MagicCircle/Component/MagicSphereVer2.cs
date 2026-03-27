using UnityEngine;

public class MagicSphereVer2 : MonoBehaviour
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

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(TagNameList.Wand)) return;
        if (!_isActive) return;

        //アクティブ状態時に杖が自分に当たったら、自分を非アクティブにする
        ToDeactive();
    }
}
