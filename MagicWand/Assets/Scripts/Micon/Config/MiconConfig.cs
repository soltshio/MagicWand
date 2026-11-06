using UnityEngine;

//作成者:杉山
//マイコンの設定

[CreateAssetMenu(fileName = "MiconConfig", menuName = "ScriptableObjects/Create MiconConfig")]
public class MiconConfig : ScriptableObject
{
    [Tooltip("初期ポート番号")] [SerializeField]
    int _defaultPortNumber;

    public int DefaultPortNumber { get { return _defaultPortNumber; } }
}
