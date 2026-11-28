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
    MagicCircleCastManager_New _magicCircleCastManager;

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

    async UniTask GameStateAsync(GamePhaseStateMachine stateMachine,CancellationToken ct)
    {
        _bigCreatureSleepPlayer.Play();

        while (true)
        {
            //詠唱パターン
            var firstTraceMagicBySphereIndex = _castPatternManager.FirstTraceMagicBySphereIndex();

            //魔法陣を展開
            await _magicCircleDeploymentManager.DeployAsync(ct);

            //魔法を選択(選んだ魔法の種類を取得)
            var selectedMagic = await _magicCircleSelectManager.SelectAsync(firstTraceMagicBySphereIndex, ct);

            //選んだ魔法の最初のなぞり以降の詠唱手順を取得
            var afterTheFirstCastOrder = _castPatternManager.AfterTheFirstCastOrder(selectedMagic);

            //魔法陣をなぞる
            await _magicCircleCastManager.CastAsync(selectedMagic,afterTheFirstCastOrder, ct);

            //魔法陣を閉じる
            await _magicCircleCloseManager.CloseAsync(selectedMagic, ct);

            //一度、巨大生物の睡眠演出を止める
            _bigCreatureSleepPlayer.Stop();

            //巨大生物のステータス更新
            _bigCreatureStatus.UpdateStatus(selectedMagic);

            //魔法を発動
            await _magicInvoker.InvokeMagicAsync(selectedMagic);

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
            await UniTask.Delay(TimeSpan.FromSeconds(_delayDuration), cancellationToken: ct);

            //でか生物がどいたらゲーム終了
            if (_bigCreatureStatus.IsWakeUp) break;
        }

        stateMachine.ChangeState(EGamePhaseState.Finish);
    }
}
