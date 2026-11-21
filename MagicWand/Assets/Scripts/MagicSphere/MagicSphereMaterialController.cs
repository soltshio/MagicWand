using UnityEngine;

//作成者:杉山
//魔法球のマテリアルを操作する機能
//マークのテクスチャとゲージの表示度の変更操作は完全にスクリプトで行い、それ以外はアニメーションで行うようにする

public class MagicSphereMaterialController : MonoBehaviour
{
    [SerializeField]
    MeshRenderer _magicSphereMeshRenderer;

    Material _magicSphereMat;

    static readonly int _markTextureID = Shader.PropertyToID("_MarkTexture");
    static readonly int _inMarkGaugeFillAmountID = Shader.PropertyToID("_InMarkGaugeFillAmount");

    void Awake()
    {
        _magicSphereMat = _magicSphereMeshRenderer.material;
    }

    public void SetMarkTexture(Texture texture)
    {
        _magicSphereMat.SetTexture(_markTextureID, texture);
    }

    //valueは0～1
    public void SetInMarkGaugeFillAmount(float value)
    {
        value = Mathf.Clamp01(value);

        _magicSphereMat.SetFloat(_inMarkGaugeFillAmountID, value);
    }
}
