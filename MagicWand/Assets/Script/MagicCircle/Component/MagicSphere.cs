using UnityEngine;

//作成者:杉山
//魔法陣上の球(杖との当たり判定を取ったり、アクティブ状態にする)

public class MagicSphere : MonoBehaviour
{
    [SerializeField]
    Renderer _renderer;

    [SerializeField]
    Material _activeMat;

    [SerializeField]
    Material _deactiveMat;

    bool _isActive;

    public bool IsActive { get => _isActive; }

    public void SwitchActive(bool active)
    {
        if(_isActive == active) return;

        _isActive = active;

        _renderer.material = active ? _activeMat : _deactiveMat;
    }

    void Awake()
    {
        _isActive = false;
        _renderer.material = _deactiveMat;
    }

    private void Start()
    {
        SwitchActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(TagNameList.Wand)) return;
        if(!_isActive) return;

        //アクティブ状態時に杖が自分に当たったら、自分を非アクティブにする
        SwitchActive(false);
    }
}
