using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FromClearToTitleSceneLoader : MonoBehaviour
{
    [SerializeField]
    float _delayToLoad = 0.5f;

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

        await UniTask.Delay(TimeSpan.FromSeconds(_delayToLoad), cancellationToken: ct);

        await SceneManager.LoadSceneAsync(SceneNameList.TitleScene).ToUniTask(cancellationToken: ct);

        _isLoading = false;
    }
}
