using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

//作成者:杉山
//インゲームシーンのゲーム中

public class GamePhaseStateTypeGame_InGameScene : GamePhaseStateTypeBase
{
    [SerializeField]
    MagicCircleManagerVer3 _magicCircleManager;

    [SerializeField]
    MagicInvoker _magicInvoker;

    [SerializeField]
    BigCreature _bigCreature;

    public override void OnEnter(GamePhaseStateMachine stateMachine)
    {
        GameStateAsync(stateMachine,this.GetCancellationTokenOnDestroy()).Forget();
    }

    public override void OnUpdate(GamePhaseStateMachine stateMachine)
    {
        
    }

    public override void OnExit(GamePhaseStateMachine stateMachine)
    {
        
    }

    async UniTask GameStateAsync(GamePhaseStateMachine stateMachine,CancellationToken token)
    {
        while(true)
        {
            //魔法陣を起動
            var invokableMagics = await _magicCircleManager.MagicCircleAsync();

            //魔法を発動
            await _magicInvoker.InvokeMagicAsync(invokableMagics);

            //一応少しだけ待つ
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);

            //でか生物がどいたらゲーム終了
            if (_bigCreature._isWakeUp) break;
        }

        stateMachine.ChangeState(EGamePhaseState.Finish);
    }
}
