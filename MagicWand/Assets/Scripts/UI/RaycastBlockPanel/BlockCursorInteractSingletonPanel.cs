using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//作成者:杉山
//北陽レーザー検知範囲内にオブジェクトが無い時にUIが反応しないようにする機能

public class BlockCursorInteractSingletonPanel : MonoBehaviour
{
    [SerializeField]
    Image _blockRaycastPanel;

    bool _isEnabled;

    //インスタンスを取得する(まだ生成されていなかった場合待ってから取得する)
    public static async UniTask<BlockCursorInteractSingletonPanel> GetInstanceAsync(CancellationToken ct)
    {
        if (Instance != null) return Instance;

        await UniTask.WaitUntil(() => Instance != null, cancellationToken: ct);

        return Instance;
    }

    public void SetBlockRaycastPanelEnabled(bool isEnabled)
    {
        _blockRaycastPanel.enabled = isEnabled;
        _isEnabled = isEnabled;
    }

    //シングルトン関係の処理
    public static BlockCursorInteractSingletonPanel Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _isEnabled = _blockRaycastPanel.enabled;
            //DontDestroyOnLoad(gameObject);//Canvasの下に入れないといけないため、この処理は呼ばなくていい
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
