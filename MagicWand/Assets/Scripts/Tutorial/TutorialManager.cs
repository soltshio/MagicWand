using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

//作成者:杉山
//チュートリアルのマネージャー
//TODO:チュートリアル内容の実装

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    Canvas _tutorialCanvas;

    [SerializeField]
    TutorialTextPlayer _tutorialTextPlayer;

    [Tooltip("チュートリアル用の魔法詠唱パターン")] [SerializeField]
    CastPatternManager _castPatternForTutorialManager;

    [SerializeField]
    MagicCircleDeploymentManager _magicCircleDeploymentManager;

    [SerializeField]
    MagicCircleSelectManager _magicCircleSelectManager;

    [SerializeField]
    MagicCircleCastManager_New _magicCircleCastManager;

    [SerializeField]
    MagicCircleCloseManager _magicCircleCloseManager;

    [SerializeField]
    BigCreatureSleepPlayer _bigCreatureSleepPlayer;

    [SerializeField]
    MagicInvoker _magicInvoker;

    [Tooltip("巨大生物のステータス")] [SerializeField]
    BigCreatureStatus _bigCreatureStatus;

    [Tooltip("巨大生物の退く演出")] [SerializeField]
    BigCreatureMoveOutPlayer _bigCreatureMoveOut;

    [Tooltip("巨大生物の二度寝をする演出")] [SerializeField]
    BigCreatureGoBackToSleepPlayer _bigCreatureGoBackToSleep;

    [Tooltip("魔法が発動した後に次のセリフが流れるまで待つ時間")] [SerializeField]
    float _delayDuration = 2f;

    [TextArea(2, 10)]
    [SerializeField]
    string[] _lineContents;

    [TextArea(2, 10)] [SerializeField]
    string[] _startLineContents;

    [TextArea(2, 10)] [SerializeField]
    string[] _finishLineContents;

    SingleTaskCancellation _singleTaskCancellation = new();

    public async UniTask PlayTutorialAsync()
    {
        try
        {
            _tutorialCanvas.enabled = true;//チュートリアル関係のUIを表示

            var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

            //チュートリアル開始のセリフを流す
            await _tutorialTextPlayer.PlayTextAsync(ct, _startLineContents);

            //杖を振って、魔法を発動させるまでのチュートリアルを開始(これが行われている間はスキップ不可にする)
            await CastMagicTutorialAsync(ct);

            //チュートリアル終了のセリフを流す
            await _tutorialTextPlayer.PlayTextAsync(ct, _finishLineContents);
        }
        catch (OperationCanceledException)
        {
            //発生した例外をSkip()によるものとして扱い終了する(そうすることによって呼び出し元の方で例外が発生してその後の処理がされなくなるということを防ぐ)
        }
        finally
        {
            //チュートリアル用のUIを非表示にする
            if (_tutorialCanvas != null) _tutorialCanvas.enabled = false;
        }
    }

    async UniTask CastMagicTutorialAsync(CancellationToken ct)
    {
        _tutorialCanvas.enabled = false;

        _bigCreatureSleepPlayer.Play();

        //発動可能な魔法を日魔法だけにする
        var castPatterns = _castPatternForTutorialManager.AllMagicCastPatterns();
        var firstTraceMagicBySphereIndex = _castPatternForTutorialManager.FirstTraceMagicBySphereIndex();

        //魔法陣を展開
        await _magicCircleDeploymentManager.DeployAsync(ct);

        //魔法を選択(選んだ魔法の種類を取得)
        var selectedMagic = await _magicCircleSelectManager.SelectAsync(firstTraceMagicBySphereIndex, ct);

        //選んだ魔法の最初のなぞり以降の詠唱手順を取得
        var afterTheFirstCastOrder = _castPatternForTutorialManager.AfterTheFirstCastOrder(selectedMagic);

        //魔法陣をなぞる
        await _magicCircleCastManager.CastAsync(selectedMagic, afterTheFirstCastOrder, ct);

        //魔法陣を閉じる
        await _magicCircleCloseManager.CloseAsync(selectedMagic, ct);

        //一度、巨大生物の睡眠演出を止める
        _bigCreatureSleepPlayer.Stop();

        //巨大生物のステータス更新
        _bigCreatureStatus.UpdateStatus(selectedMagic);

        //魔法を発動
        await _magicInvoker.InvokeMagicAsync(selectedMagic);

        //巨大生物が起きた時は退く演出を行う
        if(_bigCreatureStatus.IsWakeUp)
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

        //巨大生物の睡眠演出を止める
        _bigCreatureSleepPlayer.Stop();

        _tutorialCanvas.enabled = true;
    }

    public void Skip()
    {
        _singleTaskCancellation.Cancel();
    }

    void Start()
    {
        _tutorialCanvas.enabled = false;
    }
}
