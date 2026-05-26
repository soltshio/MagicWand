using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

//作成者:杉山
//インゲームシーンの開始タイミング

public class GamePhaseStateTypeStart_InGameScene : GamePhaseStateTypeBase
{
    [SerializeField]
    Renderer[] _renderers;

    public override void OnEnter(GamePhaseStateMachine stateMachine)
    {
        foreach (var renderer in _renderers)
        {
            renderer.enabled = false;
        }

        WaitForMouseMoveAsync(stateMachine).Forget();
    }

    public override void OnUpdate(GamePhaseStateMachine stateMachine)
    {
        
    }

    async UniTask WaitForMouseMoveAsync(GamePhaseStateMachine stateMachine)
    {
        await UniTask.Yield();
        await UniTask.Yield();
        await UniTask.Yield();

        while (true)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();

            if (delta != Vector2.zero)
            {
                Debug.Log(delta);
                stateMachine.ChangeState(EGamePhaseState.Game_InGameScene);
                Debug.Log("Start!");
                break;
            }

            await UniTask.Yield();
        }
    }

    public override void OnExit(GamePhaseStateMachine stateMachine)
    {
        foreach (var renderer in _renderers)
        {
            renderer.enabled = true;
        }
    }
}
