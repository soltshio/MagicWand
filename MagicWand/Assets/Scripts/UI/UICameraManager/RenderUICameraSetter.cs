using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

//作成者:杉山
//自動でCanvasのRenderCameraにUICameraを設定する機能

public class RenderUICameraSetter : MonoBehaviour
{
    SingleTaskCancellation _singleTaskCancellation = new();

    void Start()
    {
        SetUICameraToCanvasesRenderCameraAsync();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetUICameraToCanvasesRenderCameraAsync();
    }


    void SetUICameraToCanvasesRenderCameraAsync()
    {
        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        //var uiCamera = await GetUICameraAsync(ct);

        var uiCamera = UICameraManager.Instance.UICamera;

        //全てのCanvasを取得
        var canvasObjects = GameObject.FindGameObjectsWithTag(TagNameList.Canvas);

        //UICameraをセットしていく
        for(int i=0; i< canvasObjects.Length ;i++)
        {
            var canvas = canvasObjects[i].GetComponent<Canvas>();

            if (canvas == null) continue;

            if (canvas.worldCamera == uiCamera) continue;

            canvas.worldCamera = uiCamera;
        }
    }

    async UniTask<Camera> GetUICameraAsync(CancellationToken ct)
    {
        await UniTask.WaitUntil(() => (UICameraManager.Instance != null), cancellationToken: ct);

        var uiCameraManager = UICameraManager.Instance;

        await UniTask.WaitUntil(() => (uiCameraManager.UICamera != null), cancellationToken: ct);

        return uiCameraManager.UICamera;
    }
}
