using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

//作成者:杉山
//UI用のカメラの管理
//シングルトンを利用して、UI用のカメラを管理するクラス

public class UICameraManager : MonoBehaviour
{
    [SerializeField]
    Camera _uiCameraPrefab;

    Camera _uiCameraInstance;

    public Camera UICamera
    {
        get { return _uiCameraInstance; }
        private set { _uiCameraInstance = value; }
    }

    public static UICameraManager Instance
    {
        get; 
        private set; 
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CreateUICamera();//UI用のカメラをDontDestroyOnLoadに作成する
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void CreateUICamera()
    {
        var cameraInstance = Instantiate(_uiCameraPrefab);

        UICamera = cameraInstance;

        DontDestroyOnLoad(cameraInstance);
    }

    void Start()
    {
        DeactiveOtherUICameras();
        SetUICameraToMainCameraStack();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DeactiveOtherUICameras();
        SetUICameraToMainCameraStack();
    }

    //UICameraManagerに設定されている以外のUICameraを無効化する
    void DeactiveOtherUICameras()
    {
         var uiCameraObjects = GameObject.FindGameObjectsWithTag(TagNameList.UICamera);

        for(int i=0; i<uiCameraObjects.Length ;i++)
        {
            bool isUICameraOfManager = (uiCameraObjects[i] == UICamera.gameObject);

            if (isUICameraOfManager) continue;

            uiCameraObjects[i].SetActive(false);
        }
    }

    //MainCameraのstackにUI用のカメラをセット
    void SetUICameraToMainCameraStack()
    {
        Camera mainCamera = Camera.main;

        var mainData = mainCamera.GetUniversalAdditionalCameraData();

        if (mainData.cameraStack.Contains(UICamera)) return;
        
        mainData.cameraStack.Add(UICamera);
    }
}
