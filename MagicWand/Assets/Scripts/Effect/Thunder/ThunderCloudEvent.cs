using System;
using UnityEngine;

//作成者:杉山
//雷雲の演出

public class ThunderCloudEvent : MonoBehaviour
{
    [SerializeField]
    AudioSource _audioSource;

    [Tooltip("遠雷の効果音")] [SerializeField]
    AudioClip _distantThunderSE;

    public void CauseThunderCloud()
    {
        _audioSource.PlayOneShot(_distantThunderSE);
    }
}
