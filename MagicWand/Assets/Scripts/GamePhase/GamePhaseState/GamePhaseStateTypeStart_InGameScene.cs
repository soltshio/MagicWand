using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

//作成者:杉山
//インゲームシーンの開始タイミング

public class GamePhaseStateTypeStart_InGameScene : GamePhaseStateTypeBase
{
    [SerializeField]
    FadeInOutPanel _fadeInOutPanel;

    [SerializeField]
    float _waitDurationFromFadeInToTutorial = 1.5f;

    [SerializeField]
    TutorialManager _tutorialManager;

    public override void OnEnter(GamePhaseStateMachine stateMachine)
    {
        StartAsync(this.GetCancellationTokenOnDestroy(), stateMachine).Forget();
    }

    public override void OnUpdate(GamePhaseStateMachine stateMachine)
    {
        
    }

    public override void OnExit(GamePhaseStateMachine stateMachine)
    {
       
    }

    async UniTask StartAsync(CancellationToken ct, GamePhaseStateMachine stateMachine)
    {
        _fadeInOutPanel.FadeTrigger(FadeInOutPanel.FadeEType.FadeIn);

        //フェードインが完了するまで待つ
        await UniTask.WaitUntil(() => (_fadeInOutPanel.FadeState == FadeInOutEState.CompleteFadeIn), cancellationToken: ct);

        await UniTask.Delay(TimeSpan.FromSeconds(_waitDurationFromFadeInToTutorial), cancellationToken: ct);

        //チュートリアルを始める
        await _tutorialManager.PlayTutorialAsync();

        stateMachine.ChangeState(EGamePhaseState.Game_InGameScene);
    }
}
