using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

//作成者:杉山
//カーソル調整シーンのロード

public class CursorAdjustSceneLoader : MonoBehaviour
{
    [SerializeField]
    float _startLoadDelayDuration = 0.5f;

    [SerializeField]
    SceneNameList _sceneNameList;

    bool _isLoading = false;

    public void StartLoad()
    {
        //既にロードが始まってたら弾く
        if (_isLoading) return;

        LoadSceneAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    async UniTask LoadSceneAsync(CancellationToken ct)
    {
        if (!_sceneNameList.TryGetSceneIndex(ESceneName.CursorAdjustScene, out int cursorAdjustSceneIndex))
        {
            Debug.LogError("CursorAdjustSceneのインデックスが取得できませんでした。");
            return;
        }

        _isLoading = true;

        await UniTask.Delay(TimeSpan.FromSeconds(_startLoadDelayDuration), cancellationToken: ct);

        await SceneManager.LoadSceneAsync(cursorAdjustSceneIndex).ToUniTask(cancellationToken: ct);

        _isLoading = false;
    }
}
