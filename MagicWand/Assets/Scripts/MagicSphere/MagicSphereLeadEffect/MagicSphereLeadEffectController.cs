using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//魔法球の誘導演出

public partial class MagicSphereLeadEffectController : MonoBehaviour
{
    [SerializeField]
    LeadEffectController _leadEffectController;

    [SerializeField]
    float _activeDuration=1f;

    [SerializeField]
    LeadMagicSphereColorController _leadMagicSphereColorController;

    [SerializeField]
    MagicSpheresList _magicSpheresList;

    [SerializeField]
    MagicList _magicList;

    [SerializeField]
    AudioSource _leadEffectAudioSource;

    [SerializeField]
    AudioClip _leadSE;

    //誘導演出を行う
    //preSelectableMagicsが前のなぞりでの選択可能な魔法、 currentSelectableMagicsが今回のなぞりでの選択可能な魔法(どちらも要素番号は魔法球の番号と対応している)
    public void ActiveLeadAsync(int? preActiveSphereIndex, EMagic[] preSelectableMagics, EMagic[] currentSelectableMagics)
    {
        //誘導エフェクト効果音を流し始める
        PlaySound();

        //誘導エフェクト(パーティクル)の設定＆作成
        _leadEffectController.CreateLeadEffect(preActiveSphereIndex, currentSelectableMagics, _activeDuration);

        //魔法球に色をつける
        _leadMagicSphereColorController.PaintMagicColorToMagicSphere(preSelectableMagics,currentSelectableMagics);
    }

    public void DeactiveLeadAsync()
    {
        //全ての魔法球を元に戻す
        _leadMagicSphereColorController.PaintDefaultColorToAllMagicSphere();
    }

    void Awake()
    {
        _leadMagicSphereColorController.Awake(_magicSpheresList);
        _leadEffectController.Awake(_magicSpheresList, _magicList);
    }

    public void PlaySound()
    {
        _leadEffectAudioSource.PlayOneShot(_leadSE);
    }
}
