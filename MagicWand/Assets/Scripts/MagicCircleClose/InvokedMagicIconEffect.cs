using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

//作成者:杉山
//発動した魔法のアイコンの制御

public class InvokedMagicIconEffect : MonoBehaviour
{
    [SerializeField]
    MagicList _magicList;

    [SerializeField]
    Animator _iconAnimator;

    [SerializeField]
    SpriteRenderer _iconRenderer;

    [SerializeField]
    float _activeDuration=5f;

    public async UniTask PlayAsync(EMagic invokedMagic)
    {
        var ct = this.GetCancellationTokenOnDestroy();

        if (!TryGetIconEffectProperty(invokedMagic, out var property)) return;

        //アイコンの色を変えておく
        SetIconColor(property);

        _iconAnimator.gameObject.SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(_activeDuration), cancellationToken:ct);

        _iconAnimator.gameObject.SetActive(false);
    }

    void SetIconColor(InvokedMagicIconEffectProperty property)
    {
        _iconRenderer.color = property.IconColor;
        _iconRenderer.sprite = property.IconSprite;
    }

    //アイコンエフェクトのプロパティを取得、失敗したらfalseを返す
    bool TryGetIconEffectProperty(EMagic invokedMagic,out InvokedMagicIconEffectProperty property)
    {
        property = _magicList.GetComponentFromMagic<InvokedMagicIconEffectProperty>(invokedMagic);

        return property != null;
    }

    void Start()
    {
        _iconAnimator.gameObject.SetActive(false);
    }
}
