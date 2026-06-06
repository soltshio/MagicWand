using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

//作成者:杉山
//インゲームシーンの開始タイミング

public class GamePhaseStateTypeStart_InGameScene : GamePhaseStateTypeBase
{
    [SerializeField]
    FadeInOutPanel _fadeInOutPanel;

    public override void OnEnter(GamePhaseStateMachine stateMachine)
    {
        _fadeInOutPanel.FadeTrigger(FadeInOutPanel.FadeEType.FadeIn);
    }

    public override void OnUpdate(GamePhaseStateMachine stateMachine)
    {
        if(_fadeInOutPanel.FadeState == FadeInOutEState.CompleteFadeIn)
        {
            stateMachine.ChangeState(EGamePhaseState.Game_InGameScene);
        }
    }

    public override void OnExit(GamePhaseStateMachine stateMachine)
    {
       
    }
}
