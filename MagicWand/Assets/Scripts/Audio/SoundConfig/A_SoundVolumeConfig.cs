using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//音量設定(音量の範囲、初期値を設定可能)

[System.Serializable]
public class A_SoundVolumeConfig
{
    [Tooltip("最小音量")] [SerializeField]
    float _minVolume;

    [Tooltip("音量の範囲")] [SerializeField]
    float _volumeRange;

    [Tooltip("初期音量")] [SerializeField]
    float _defaultVolume;

    public void OnValidate()
    {
        //初期音量を範囲内に収めるようにする
        _defaultVolume = Mathf.Clamp(_defaultVolume, _minVolume, MaxVolume);
    }

    //ゲッター

    public float MinVolume { get { return _minVolume; } }
    public float MaxVolume { get { return _minVolume+_volumeRange; } }
    public float DefaultVolume { get { return _defaultVolume; } }
}
