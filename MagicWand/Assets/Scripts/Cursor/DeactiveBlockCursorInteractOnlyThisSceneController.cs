using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

//作成者:杉山
//このコンポーネントが存在するシーンのみ、ブロックカーソルインタラクト機能を無効にする機能

public class DeactiveBlockCursorInteractOnlyThisSceneController : MonoBehaviour
{
    BlockCursorInteractSingletonPanel _blockCursorInteractSingletonPanel;
    BlockCursorInteractManagerOnHokuyoExit _blockCursorInteractManagerOnHokuyoExit;

    async void Start()
    {
        _blockCursorInteractManagerOnHokuyoExit = await BlockCursorInteractManagerOnHokuyoExit.GetInstanceAsync(this.GetCancellationTokenOnDestroy());
        _blockCursorInteractSingletonPanel = await BlockCursorInteractSingletonPanel.GetInstanceAsync(this.GetCancellationTokenOnDestroy());

        //ブロックカーソルインタラクト機能を無効にする
        if (_blockCursorInteractManagerOnHokuyoExit != null)
        {
            _blockCursorInteractManagerOnHokuyoExit.enabled = false;
        }
        if(_blockCursorInteractSingletonPanel != null)
        {
            _blockCursorInteractSingletonPanel.SetBlockRaycastPanelEnabled(false);
        }
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += ActiveBlockCursorInteract;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= ActiveBlockCursorInteract;
    }

    //別シーンに遷移したらブロックカーソルインタラクト機能を有効にする
    private void ActiveBlockCursorInteract(Scene previous, Scene next)
    {
        if (_blockCursorInteractManagerOnHokuyoExit == null) return;
        
        _blockCursorInteractManagerOnHokuyoExit.enabled = true;
    }
}
