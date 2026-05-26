using UnityEngine;

//作成者:杉山
//インゲームシーンのゲーム中

public class GamePhaseStateTypeGame_InGameScene : GamePhaseStateTypeBase
{
    public override void OnEnter(GamePhaseStateMachine stateMachine)
    {
        
    }

    public override void OnUpdate(GamePhaseStateMachine stateMachine)
    {
        //3回魔法を唱えたらゲーム終了
    }

    public override void OnExit(GamePhaseStateMachine stateMachine)
    {
        
    }
}
