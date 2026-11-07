using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

//作成者:杉山
//フェードアウト無しのシーン遷移をする汎用シーンローダー

public class GenericSceneLoader_NoFadeOut : MonoBehaviour
{
    [SerializeField]
    float _delayToLoad = 0.5f;

    [SerializeField]
    SceneNameList _sceneNameList;

    [SerializeField]
    ESceneName _loadSceneName;

    bool _isLoading = false;

    public void StartLoad()
    {
        //既にロードが始まってたら弾く
        if (_isLoading) return;

        LoadSceneAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    async UniTask LoadSceneAsync(CancellationToken ct)
    {
        if (!_sceneNameList.TryGetSceneIndex(_loadSceneName, out int sceneIndex))
        {
            Debug.LogError("Sceneのインデックスが取得できませんでした。");
            return;
        }

        _isLoading = true;

        await UniTask.Delay(TimeSpan.FromSeconds(_delayToLoad), cancellationToken: ct);

        await SceneManager.LoadSceneAsync(sceneIndex).ToUniTask(cancellationToken: ct);

        _isLoading = false;
    }
}
