using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//魔法球をなぞった時のエフェクト

public class EffectOnTraceMagicSphere : MonoBehaviour
{
    [SerializeField]
    MagicCircleCastManager_New _magicCircleCastManager;

    [SerializeField]
    MagicCircleSelectManager _magicCircleSelectManager;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    AudioClip _castSE;

    void OnEnable()
    {
        _magicCircleCastManager.OnTraceMagicSphere += CastEffect;
        _magicCircleSelectManager.OnSelectMagic += CastEffect;
    }

    void OnDisable()
    {
        _magicCircleCastManager.OnTraceMagicSphere -= CastEffect;
        _magicCircleSelectManager.OnSelectMagic -= CastEffect;
    }

    void CastEffect(EMagic magic,int touchedMagicSphereindex,int touchSphereCount)
    {
        _audioSource.PlayOneShot(_castSE);
    }

    void CastEffect(EMagic selectedMagic, int selectedSphereIndex)
    {
        _audioSource.PlayOneShot(_castSE);
    }
}
