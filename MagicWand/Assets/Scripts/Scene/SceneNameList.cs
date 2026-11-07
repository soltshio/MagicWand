using UnityEngine;

//作成者:杉山
//シーンの名前リスト

[CreateAssetMenu(fileName = "SceneNameList", menuName = "ScriptableObjects/Create SceneNameList")]
public class SceneNameList : ScriptableObject
{
    [Tooltip("注意点" +
        "\n1.インデックスにはBuild時のSceneListからシーン名に対応した番号を設定してください" +
        "\n2.番号は被らせないようにしてください")]
    [SerializeField]
    SerializableDictionary<ESceneName, int> _sceneDic;

    public bool TryGetSceneIndex(ESceneName sceneName, out int sceneIndex)
    {
        return _sceneDic.TryGetValue(sceneName, out sceneIndex);
    }
}
