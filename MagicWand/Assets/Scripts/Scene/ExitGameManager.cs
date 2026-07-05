using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

//作成者:杉山
//ゲームの終了処理

public class ExitGameManager : MonoBehaviour
{
    [SerializeField]
    float _exitDelayDuration = 0.5f;

    bool _isExiting = false;

    public void ExitGame()
    {
        if (_isExiting) return;

        ExitGameAsync().Forget();
    }

    async UniTask ExitGameAsync()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _isExiting = true;

        await UniTask.Delay(TimeSpan.FromSeconds(_exitDelayDuration), cancellationToken: ct);

        //ゲーム終了処理
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
