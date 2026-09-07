using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
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

        //アイコンの色を変えておく
        SetIconColor(invokedMagic);

        _iconAnimator.gameObject.SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(_activeDuration), cancellationToken:ct);

        _iconAnimator.gameObject.SetActive(false);
    }

    void SetIconColor(EMagic invokedMagic)
    {
        //アイコンの色やテクスチャなどを取得
        var invokedMagicIconEffectProperty = _magicList.GetComponentFromMagic<InvokedMagicIconEffectProperty>(invokedMagic);

        if (invokedMagicIconEffectProperty == null) return;

        _iconRenderer.color = invokedMagicIconEffectProperty.IconColor;
        _iconRenderer.sprite = invokedMagicIconEffectProperty.IconSprite;
    }

    void Start()
    {
        _iconAnimator.gameObject.SetActive(false);
    }
}
