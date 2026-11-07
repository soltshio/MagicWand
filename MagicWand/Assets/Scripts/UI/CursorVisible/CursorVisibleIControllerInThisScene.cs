using UnityEngine;
using UnityEngine.SceneManagement;

//作成者:杉山
//現在のシーンのみカーソルを表示させる機能

public class CursorVisibleIControllerInThisScene : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += SetCursorInvisible;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= SetCursorInvisible;
    }

    //別シーンに遷移したらカーソルを非表示にする
    private void SetCursorInvisible(Scene previous, Scene next)
    {
        Cursor.visible = false;
    }
}
