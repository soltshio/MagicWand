using System.IO;
using UnityEngine;

//作成者:杉山
//セーブとロードをするシングルトンマネージャー

public class SaveLoadManager : MonoBehaviour
{
    //現在のデータ
    //一度ロード処理(ロードするデータが無い場合は_saveDataはnullになるのでその場合はデータを新しく作成する)をすれば、これが最新のデータになる
    SaveData _saveData;

    const string _savePathBelowPersistentDataPath = "/save.json";//データが保存してある場所、Application.persistentDataPath以下のどこに格納するか
    string _savePath;

    public static SaveLoadManager Instance
    {
        get;
        private set;
    }

    public SaveData SaveData
    {
        get { return _saveData; }
    }

    void Awake()
    {
        if (Instance == null)
        {
            //初期化処理
            Instance = this;
            //セーブ場所の初期化
            _savePath = Application.persistentDataPath + _savePathBelowPersistentDataPath;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //ロード
    public void Load()
    {
        //セーブデータが無かった場合
        if (!File.Exists(_savePath))
        {
            _saveData = null;
            return;
        }

        //セーブデータが見つかった場合
        string json = File.ReadAllText(_savePath);

        SaveData data = JsonUtility.FromJson<SaveData>(json);

        _saveData = data;
    }

    //(データが無かった場合に)データを作成する
    public void CreateNewData()
    {
        SaveData data = new();

        _saveData = data;

        Save(_saveData);
    }

    //セーブ
    public void Save(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(_savePath, json);
    }
}
