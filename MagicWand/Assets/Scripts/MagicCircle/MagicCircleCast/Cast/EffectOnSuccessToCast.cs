using Unity.VisualScripting;
using UnityEngine;

public class EffectOnSuccessToCast : MonoBehaviour
{
    [SerializeField]
    MagicCircleCastManager _magicCircleCastManager;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    AudioClip _castSE;

    void Awake()
    {
        _magicCircleCastManager.OnSuccessToCast += CastEffect;
    }

    void CastEffect(EMagic castMagic,int touchedMagicSphereindex)
    {
        _audioSource.PlayOneShot(_castSE);
    }
}
