using Unity.VisualScripting;
using UnityEngine;

public class EffectOnSuccessToCast : MonoBehaviour
{
    [SerializeField]
    MagicCircleManagerVer3 _magicCircleManager;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    AudioClip _castSE;

    void Awake()
    {
        _magicCircleManager.OnSuccessToCast += CastEffect;
    }

    void CastEffect(EMagic castMagic,int touchedMagicSphereindex)
    {
        _audioSource.PlayOneShot(_castSE);
    }
}
