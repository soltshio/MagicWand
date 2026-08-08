using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//常にこちらに向くビルボードのスクリプト

[ExecuteAlways]
public class AlwaysLookToCamera_BillBoard : MonoBehaviour
{
    void LateUpdate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                Camera sceneCamera = sceneView.camera;

                // 球体の正面（Z+）をSceneビューのカメラへ向ける
                transform.LookAt(transform.position + sceneCamera.transform.forward,sceneCamera.transform.up);
            }
        }
        else
#endif
        {
            // Play中はMain Cameraを使う
            Camera cam = Camera.main;

            if (cam != null)
            {
                transform.LookAt(transform.position + cam.transform.forward,cam.transform.up);
            }
        }
    }
}
