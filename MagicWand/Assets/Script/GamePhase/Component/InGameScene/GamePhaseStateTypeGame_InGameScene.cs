using System;
using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//インゲームシーンのゲーム中

public class GamePhaseStateTypeGame_InGameScene : GamePhaseStateTypeBase
{
    [SerializeField]
    int _clearCount = 3;

    [SerializeField]
    MagicCircleManagerVer3 _magicCircleManager;

    int _currentCount = 0;

    GamePhaseStateMachine _stateMachine;

    void OnMagicActived(EMagic magic)
    {
        _currentCount++;

        if (_currentCount >= _clearCount)
        {
            _stateMachine.ChangeState(EGamePhaseState.Finish);
        }
    }   

    public override void OnEnter(GamePhaseStateMachine stateMachine)
    {
        _magicCircleManager.OnMagicActived += OnMagicActived;
        _currentCount = 0;
        _stateMachine = stateMachine;
    }

    public override void OnUpdate(GamePhaseStateMachine stateMachine)
    {
        
    }

    public override void OnExit(GamePhaseStateMachine stateMachine)
    {
        _magicCircleManager.OnMagicActived -= OnMagicActived;
    }
}
