using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//作成者:杉山
//インゲームシーンのゲーム中

public class GamePhaseStateTypeGame_InGameScene : GamePhaseStateTypeBase
{
    [SerializeField]
    CastPatternManager _castPatternManager;

    [SerializeField]
    MagicCircleDeploymentManager _magicCircleDeploymentManager;

    [SerializeField]
    MagicCircleSelectManager _magicCircleSelectManager;

    [SerializeField]
    MagicCircleCastManager _magicCircleCastManager;

    [SerializeField]
    MagicCircleCloseManager _magicCircleCloseManager;

    [SerializeField]
    MagicInvoker _magicInvoker;

    [Tooltip("巨大生物のステータス")] [SerializeField]
    BigCreatureStatus _bigCreatureStatus;

    [SerializeField]
    BigCreatureSleepPlayer _bigCreatureSleepPlayer;

    [Tooltip("巨大生物の退く演出")] [SerializeField]
    BigCreatureMoveOutPlayer _bigCreatureMoveOut;

    [Tooltip("巨大生物の二度寝をする演出")] [SerializeField]
    BigCreatureGoBackToSleepPlayer _bigCreatureGoBackToSleep;

    [SerializeField]
    float _delayDuration=0.5f;

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
        _bigCreatureSleepPlayer.Play();

        while (true)
        {
            //詠唱パターン
            var castPatterns = _castPatternManager.AllMagicCastPatterns();
            var magicFirstTraceIndexArray = _castPatternManager.MagicFirstTraceIndexArray();

            //魔法陣を展開
            await _magicCircleDeploymentManager.DeployAsync(token);

            //魔法を選択
            await _magicCircleSelectManager.SelectAsync(magicFirstTraceIndexArray, token);

            //魔法陣をなぞる
            var invokableMagic = await _magicCircleCastManager.MagicCircleAsync(castPatterns, token);

            //魔法陣を閉じる
            await _magicCircleCloseManager.CloseAsync(invokableMagic,token);

            //一度、巨大生物の睡眠演出を止める
            _bigCreatureSleepPlayer.Stop();

            //巨大生物のステータス更新
            _bigCreatureStatus.UpdateStatus(invokableMagic);

            //魔法を発動
            await _magicInvoker.InvokeMagicAsync(invokableMagic);

            //巨大生物が起きた時は退く演出を行う
            if (_bigCreatureStatus.IsWakeUp)
            {
                await _bigCreatureMoveOut.TakeReactionAsync();
            }
            //まだ起きない場合は巨大生物の睡眠演出を再度流す
            else
            {
                await _bigCreatureGoBackToSleep.TakeReactionAsync();
                _bigCreatureSleepPlayer.Play();
            }

            //一応少しだけ待つ
            await UniTask.Delay(TimeSpan.FromSeconds(_delayDuration), cancellationToken: token);

            //でか生物がどいたらゲーム終了
            if (_bigCreatureStatus.IsWakeUp) break;
        }

        stateMachine.ChangeState(EGamePhaseState.Finish);
    }
}
