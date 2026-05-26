using UnityEngine;

//作成者:杉山
//インゲームシーンの終了タイミング

public class GamePhaseStateTypeFinish_InGameScene : GamePhaseStateTypeBase
{
    [SerializeField]
    Renderer[] _renderers;

    public override void OnEnter(GamePhaseStateMachine stateMachine)
    {
        foreach (var renderer in _renderers)
        {
            renderer.enabled = false;
        }

        Debug.Log("Finish!");
    }

    public override void OnUpdate(GamePhaseStateMachine stateMachine)
    {
    }

    public override void OnExit(GamePhaseStateMachine stateMachine)
    {
        
    }
}
