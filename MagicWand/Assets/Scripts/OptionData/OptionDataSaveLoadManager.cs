using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//オプションデータのセーブ・ロードをするクラス

public class OptionDataSaveLoadManager : MonoBehaviour
{
    //＝＝＝＝＝＝＝＝＝＝
    //マイコンのポート番号
    //＝＝＝＝＝＝＝＝＝＝
    const string MiconPortNumberKey = "MICON_PORT_NUMBER";

    //マイコンのポート番号をロードする(データが無かったらfalseを返す)
    public bool LoadMiconPortNumber(out int portNumber)
    {
        if(PlayerPrefs.HasKey(MiconPortNumberKey))
        {
            portNumber = PlayerPrefs.GetInt(MiconPortNumberKey, 0);
            return true;
        }

        portNumber = 0;
        return false;
    }

    //マイコンのポート番号をセーブする
    public void SaveMiconPortNumber(int portNumber)
    {
        PlayerPrefs.SetInt(MiconPortNumberKey, portNumber);
    }




    //＝＝＝＝
    //音量関係
    //＝＝＝＝
    readonly Dictionary<ESoundType, string> _soundTypeToKeyMap = new Dictionary<ESoundType, string>()
    {
        { ESoundType.Master,   "MASTER_VOLUME" },//マスター
        { ESoundType.SE,       "SE_VOLUME"},//SE
        { ESoundType.BGM,      "BGM_VOLUME"}//BGM
    };

    public bool LoadVolume(ESoundType soundType, out float volume)
    {
        if (!_soundTypeToKeyMap.TryGetValue(soundType, out var key))
        {
            Debug.Log("音量の種類が不正です");
            volume = 1.0f;
            return false;
        }
        if (PlayerPrefs.HasKey(key))
        {
            volume = PlayerPrefs.GetFloat(key, 1.0f);
            return true;
        }
        volume = 1.0f;
        return false;
    }

    public void SaveVolume(ESoundType soundType, float volume)
    {
        if (!_soundTypeToKeyMap.TryGetValue(soundType, out var key))
        {
            Debug.Log("音量の種類が不正です");
            return;
        }
        PlayerPrefs.SetFloat(key, volume);
    }




    //シングルトン関係の処理
    public static OptionDataSaveLoadManager Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
