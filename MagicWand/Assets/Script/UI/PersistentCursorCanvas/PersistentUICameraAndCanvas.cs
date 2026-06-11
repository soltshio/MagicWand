using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

//作成者:杉山
//シーンが変わるごとに3Dオブジェクトをキャンバス上に表示するためのUIカメラとキャンバスを更新する機能

public class PersistentUICameraAndCanvas : MonoBehaviour
{
    [SerializeField]
    Canvas _canvas;

    [SerializeField]
    Camera _uiCameraPrefab;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        var uiCamera = InitUICamera();

        if (uiCamera == null) return;

        UpdateRenderCamera(uiCamera);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var uiCamera = InitUICamera();

        if (uiCamera == null) return;

        UpdateRenderCamera(uiCamera);
    }

    Camera InitUICamera()
    {
        //UIカメラが無い時は無理やり作り、メインカメラのStackに入れる
        GameObject uiCameraObj = GameObject.FindWithTag("UICamera");

        if (uiCameraObj == null)
        {
#if UNITY_EDITOR
            Debug.Log("UICameraタグのオブジェクトが見つかりませんでした");
#endif
            return MakeUICamera();
        }

        Camera uiCamera = uiCameraObj.GetComponent<Camera>();

        if (uiCamera == null)
        {
#if UNITY_EDITOR
            Debug.Log("UICameraタグのオブジェクトにCameraが付いていません");
#endif
            return MakeUICamera();
        }

        return uiCamera;
    }

    Camera MakeUICamera()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
#if UNITY_EDITOR
            Debug.LogError("Main Cameraが見つかりません");
#endif
            return null;
        }

        //UIカメラを作る
        var uiCamera = Instantiate(_uiCameraPrefab, Vector3.zero, Quaternion.identity);

        var mainData = mainCamera.GetUniversalAdditionalCameraData();

        if (!mainData.cameraStack.Contains(uiCamera))
        {
            mainData.cameraStack.Add(uiCamera);
        }

        return uiCamera;
    }

    void UpdateRenderCamera(Camera uiCamera)
    {
        _canvas.worldCamera = uiCamera;
    }
}
