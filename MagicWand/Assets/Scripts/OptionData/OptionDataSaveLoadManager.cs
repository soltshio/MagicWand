using UnityEngine;

//作成者:杉山
//オプションデータのセーブ・ロードをするクラス

public class OptionDataSaveLoadManager : MonoBehaviour
{
    //マイコンのポート番号
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


    //TODO:音量関係は列挙型で音の種類を指定出来るようにする
    //マスターの音量
    const string MasterVolumeKey = "MASTER_VOLUME";

    //マスター音量をロードする(データが無かったらfalseを返す)
    public bool LoadMasterVolume(out float volume)
    {
        if(PlayerPrefs.HasKey(MasterVolumeKey))
        {
            volume = PlayerPrefs.GetFloat(MasterVolumeKey, 1.0f);
            return true;
        }

        volume = 1.0f;
        return false;
    }

    //マスター音量をセーブする
    public void SaveMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat(MasterVolumeKey, volume);
    }



    //BGMの音量
    const string BGMVolumeKey = "BGM_VOLUME";

    //BGM音量をロードする(データが無かったらfalseを返す)
    public bool LoadBGMVolume(out float volume)
    {
        if (PlayerPrefs.HasKey(BGMVolumeKey))
        {
            volume = PlayerPrefs.GetFloat(BGMVolumeKey, 1.0f);
            return true;
        }
        volume = 1.0f;
        return false;
    }

    //BGM音量をセーブする
    public void SaveBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat(BGMVolumeKey, volume);
    }



    //効果音の音量
    const string SEVolumeKey = "SE_VOLUME";

    //効果音音量をロードする(データが無かったらfalseを返す)
    public bool LoadSEVolume(out float volume)
    {
        if (PlayerPrefs.HasKey(SEVolumeKey))
        {
            volume = PlayerPrefs.GetFloat(SEVolumeKey, 1.0f);
            return true;
        }
        volume = 1.0f;
        return false;
    }

    //効果音の音量をセーブする
    public void SaveSEVolume(float volume)
    {
        PlayerPrefs.SetFloat(SEVolumeKey, volume);
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
