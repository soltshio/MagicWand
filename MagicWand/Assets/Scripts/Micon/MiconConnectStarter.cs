using UnityEngine;

//ゲーム起動時にマイコンと接続処理を呼ぶ機能

public class MiconConnectStarter : MonoBehaviour
{
    [SerializeField]
    int _defaultPortNum = 3;

    void Start()
    {
        SerialHandler.Instance.Open(_defaultPortNum);
    }
}
