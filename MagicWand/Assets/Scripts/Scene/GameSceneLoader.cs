using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

//作成者:杉山
//ゲームシーンのロード

public class GameSceneLoader : MonoBehaviour
{
    [Tooltip("フェードイン・アウトをするパネル")] [SerializeField]
    FadeInOutPanel _fadeInOutPanel;

    bool _isLoading = false;

    public void StartLoad()
    {
        //既にロードが始まってたら弾く
        if (_isLoading) return;

        LoadSceneAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    async UniTask LoadSceneAsync(CancellationToken ct)
    {
        _isLoading = true;

        //フェードアウトをしきってから、シーンのロードを始める
        _fadeInOutPanel.FadeTrigger(FadeInOutPanel.FadeEType.FadeOut);

        await UniTask.WaitUntil(() => (_fadeInOutPanel.FadeState == FadeInOutEState.CompleteFadeOut), cancellationToken: ct);

        await SceneManager.LoadSceneAsync(SceneNameList.GameScene).ToUniTask(cancellationToken: ct);

        _isLoading = false;
    }
}
