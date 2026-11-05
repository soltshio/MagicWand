using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//AudioMixerの音量の種類ごとの名前

public class AudioMixerName : MonoBehaviour
{
    readonly static Dictionary<ESoundType,string> _audioMixerSoundNameDic = new Dictionary<ESoundType, string>()
    {
        { ESoundType.Master,   "Master" },//マスター
        { ESoundType.SE,       "SE"},//SE
        { ESoundType.BGM,      "BGM"}//BGM
    };

    public static string SoundName(ESoundType type)
    {
        if(!_audioMixerSoundNameDic.TryGetValue(type,out var name))
        {
            Debug.Log("取得に失敗しました");
            return null;
        }

        return name;
    }
}
