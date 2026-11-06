using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//音量の設定
//音の種類ごとに音量の範囲や初期値といった音量設定を設定可能

[CreateAssetMenu(fileName = "SoundConfig", menuName = "ScriptableObjects/Create SoundConfig")]
public class SoundConfigs : ScriptableObject
{
    [Tooltip("マスター音量の設定")] [SerializeField]
    A_SoundVolumeConfig _masterVolumeConfig;

    [Tooltip("SE音量の設定")] [SerializeField]
    A_SoundVolumeConfig _seVolumeConfig;

    [Tooltip("BGM音量の設定")] [SerializeField]
    A_SoundVolumeConfig _bgmVolumeConfig;

    const float _errorValue = 0;//エラー時にとりあえず返す値

    Dictionary<ESoundType, A_SoundVolumeConfig> _soundVolumeConfigsDic;



    //最小音量
    public float MinVolume(ESoundType soundType)
    {
        if (_soundVolumeConfigsDic == null) Initialize();
        if (!IsValidSoundType(soundType, out var config)) return _errorValue;

        return config.MinVolume;
    }

    //最大音量
    public float MaxVolume(ESoundType soundType)
    {
        if (_soundVolumeConfigsDic == null) Initialize();
        if (!IsValidSoundType(soundType, out var config)) return _errorValue;

        return config.MaxVolume;
    }

    //初期音量
    public float DefaultVolume(ESoundType soundType)
    {
        if (_soundVolumeConfigsDic == null) Initialize();
        if (!IsValidSoundType(soundType, out var config)) return _errorValue;

        return config.DefaultVolume;
    }

    //private
    bool IsValidSoundType(ESoundType soundType, out A_SoundVolumeConfig config)//存在する音の種類か見る(なかったら警告)
    {
        if (!_soundVolumeConfigsDic.TryGetValue(soundType, out config))
        {
            Debug.Log("処理に失敗");
            return false;
        }

        return true;
    }

    //ScriptableObjectだとゲーム起動時にAwakeやStartが呼ばれないので、OnEnableで初期化する
    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        _soundVolumeConfigsDic = new Dictionary<ESoundType, A_SoundVolumeConfig>()
        {
            { ESoundType.Master, _masterVolumeConfig },
            { ESoundType.SE, _seVolumeConfig },
            { ESoundType.BGM, _bgmVolumeConfig },
        };
    }

    private void OnValidate()
    {
        _masterVolumeConfig.OnValidate();
        _seVolumeConfig.OnValidate();
        _bgmVolumeConfig.OnValidate();
    }
}
