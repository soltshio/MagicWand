using UnityEngine;

//作成者:杉山
//MagicCircleActiveHandlerの魔法陣の表示・非表示の処理をする

[System.Serializable]
public class MagicCircleRendererActivator_MagicCircleActiveHandler
{
    [Tooltip("魔法陣の描画機能")] [SerializeField]
    SpriteRenderer _magicCircleRenderer;

    [Tooltip("表示時の透明度")] [Range(0f, 1f)] [SerializeField]
    float _magicCircleAlpha_Active=0.3f;

    //初期状態では非表示にしておく
    public void Start()
    {
        _magicCircleRenderer.enabled = false;
    }

    public void MagicCircleSwitchEnable(bool isEnable)
    {
        _magicCircleRenderer.enabled = isEnable;
    }

    //☆表示関係

    //魔法陣をrate(0～1)に合わせて、だんだん表示させる(1で完全に表示)
    public void ActivateMagicCircle(float rate)
    {
        float magicCircleAlpha = Mathf.Lerp(0f, _magicCircleAlpha_Active, rate);
        SetMagicCircleAlpha(magicCircleAlpha);
    }

    //☆非表示関係

    //魔法陣をrate(0～1)に合わせて、だんだん非表示にさせる(1で完全に非表示)
    public void DeactivateMagicCircle(float rate)
    {
        float magicCircleAlpha = Mathf.Lerp(_magicCircleAlpha_Active, 0f, rate);
        SetMagicCircleAlpha(magicCircleAlpha);
    }

    //魔法陣の透明度をセットする
    void SetMagicCircleAlpha(float alpha)
    {
        var magicCircleColor = _magicCircleRenderer.color;
        magicCircleColor.a = alpha;
        _magicCircleRenderer.color = magicCircleColor;
    }
}
