using Cysharp.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

//作成者:杉山
//Windowsの警告音を鳴らすプレイヤー

public class WindowsWarningPlayer : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern bool MessageBeep(uint uType);

    //警告音を出す
    public void PlayWarning()
    {
        MessageBeep(0x00000030);
    }

    //インスタンスの取得
    //インスタンスを取得する(まだ生成されていなかった場合待ってから取得する)
    public static async UniTask<WindowsWarningPlayer> GetInstanceAsync(CancellationToken ct)
    {
        if (Instance != null) return Instance;

        await UniTask.WaitUntil(() => Instance != null, cancellationToken: ct);

        return Instance;
    }

    //シングルトン関係の処理
    public static WindowsWarningPlayer Instance
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
