using UnityEngine;

public class MagicSphereMaterialController_1 : MonoBehaviour
{
    [SerializeField]
    MeshRenderer _magicSphereMeshRenderer;

    static readonly int _markTextureID = Shader.PropertyToID("_MarkTexture");
    static readonly int _inMarkGaugeFillAmountID = Shader.PropertyToID("_InMarkGaugeFillAmount");

    public void SetMarkTexture(Texture texture)
    {
        _magicSphereMeshRenderer.material.SetTexture(_markTextureID, texture);
    }

    //valueは0～1
    public void SetInMarkGaugeFillAmount(float value)
    {
        value = Mathf.Clamp01(value);

        _magicSphereMeshRenderer.material.SetFloat(_inMarkGaugeFillAmountID, value);
    }
}
