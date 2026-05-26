using UnityEngine;

//作成者:杉山
//インゲームシーンの開始タイミング

public class GamePhaseStateTypeStart_InGameScene : GamePhaseStateTypeBase
{
    public override void OnEnter(GamePhaseStateMachine stateMachine)
    {
        stateMachine.ChangeState(EGamePhaseState.Game_InGameScene);
        Debug.Log("Start!");
    }

    public override void OnUpdate(GamePhaseStateMachine stateMachine)
    {
    }

    public override void OnExit(GamePhaseStateMachine stateMachine)
    {
        
    }
}
