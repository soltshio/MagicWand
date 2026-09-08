using UnityEngine;

//作成者:杉山
//発動した魔法のアイコンエフェクト関連のプロパティ

public class InvokedMagicIconEffectProperty : MonoBehaviour
{
    [SerializeField]
    Color _iconColor;

    [SerializeField]
    Sprite _iconSprite;

    public Color IconColor { get { return _iconColor; } }
    public Sprite IconSprite { get { return _iconSprite; } }
}
