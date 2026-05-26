using UnityEngine;

//作成者:杉山
//インゲームシーンの終了タイミング

public class GamePhaseStateTypeFinish_InGameScene : GamePhaseStateTypeBase
{
    public override void OnEnter(GamePhaseStateMachine stateMachine)
    {
        Debug.Log("Finish!");
    }

    public override void OnUpdate(GamePhaseStateMachine stateMachine)
    {
    }

    public override void OnExit(GamePhaseStateMachine stateMachine)
    {
        
    }
}
