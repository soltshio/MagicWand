using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
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

    public void ActiveLeadAsync(int? preActiveSphereIndex, List<(EMagic magic, int index)> activeSphereIndex_MagicList)
    {
        //誘導エフェクト効果音を流し始める
        PlaySound();

        //誘導エフェクト(パーティクル)の初期化
        _leadEffectController.CreateLeadEffect(preActiveSphereIndex, activeSphereIndex_MagicList,_activeDuration);

        //魔法球に色をつける
        _leadMagicSphereColorController.PaintMagicColorToMagicSphere(preActiveSphereIndex,activeSphereIndex_MagicList);
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
