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

    void OnEnable()
    {
        _magicCircleCastManager.OnSuccessToCast += CastEffect;
    }

    void OnDisable()
    {
        _magicCircleCastManager.OnSuccessToCast -= CastEffect;
    }

    void CastEffect(EMagic castMagic,int touchedMagicSphereindex,int touchSphereCount)
    {
        _audioSource.PlayOneShot(_castSE);
    }
}
