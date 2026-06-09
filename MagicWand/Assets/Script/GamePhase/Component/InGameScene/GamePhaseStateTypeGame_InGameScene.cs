using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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

    public override void OnEnter(GamePhaseStateMachine stateMachine)
    {
        GameStateAsync(stateMachine, this.GetCancellationTokenOnDestroy()).Forget();
    }

    public override void OnUpdate(GamePhaseStateMachine stateMachine)
    {
        
    }

    public override void OnExit(GamePhaseStateMachine stateMachine)
    {
        
    }

    async UniTask GameStateAsync(GamePhaseStateMachine stateMachine,CancellationToken ct)
    {
        for(int i=0; i<_clearCount ;i++)
        {
            //魔法陣を起動
            await _magicCircleManager.MagicCircleAsync();

            //魔法を発動

            //数秒待つ
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
        }

        stateMachine.ChangeState(EGamePhaseState.Finish);
    }
}
