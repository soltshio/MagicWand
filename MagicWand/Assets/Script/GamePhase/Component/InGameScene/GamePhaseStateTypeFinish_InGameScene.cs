using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

//作成者:杉山
//インゲームシーンの終了タイミング

public class GamePhaseStateTypeFinish_InGameScene : GamePhaseStateTypeBase
{
    [SerializeField]
    FadeInOutPanel _fadeInOutPanel;

    public override void OnEnter(GamePhaseStateMachine stateMachine)
    {
        ClearSceneLoadAsync(this.GetCancellationTokenOnDestroy()).Forget();

        Debug.Log("Finish!");
    }

    public override void OnUpdate(GamePhaseStateMachine stateMachine)
    {
    }

    public override void OnExit(GamePhaseStateMachine stateMachine)
    {
        
    }

    async UniTask ClearSceneLoadAsync(CancellationToken ct)
    {
        //フェードアウトをしきってから、シーンのロードを始める
        _fadeInOutPanel.FadeTrigger(FadeInOutPanel.FadeEType.FadeOut);

        await UniTask.WaitUntil(() => (_fadeInOutPanel.FadeState == FadeInOutEState.CompleteFadeOut), cancellationToken: ct);

        await SceneManager.LoadSceneAsync(SceneNameList.ClearScene).ToUniTask(cancellationToken: ct);
    }
}
