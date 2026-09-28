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
    MagicCircleCastManager _magicCircleCastManager;

    [SerializeField]
    MagicCircleCloseManager _magicCircleCloseManager;

    [SerializeField]
    BigCreatureSleepPlayer _bigCreatureSleepPlayer;

    [SerializeField]
    MagicInvoker _magicInvoker;

    [Tooltip("巨大生物のステータス")] [SerializeField]
    BigCreatureStatus _bigCreatureStatus;

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

            //TODO:チュートリアルの内容を実装する

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
        var castPatterns = _castPatternForTutorialManager.DecideActiveOrderIndexs();

        //魔法陣展開
        await _magicCircleDeploymentManager.DeployAsync(ct);

        //魔法陣をなぞる(詠唱)
        var invokableMagic = await _magicCircleCastManager.MagicCircleAsync(castPatterns, ct);

        //魔法陣を閉じる
        await _magicCircleCloseManager.CloseAsync(invokableMagic, ct);

        //一度、巨大生物の睡眠演出を止める
        _bigCreatureSleepPlayer.Stop();

        //魔法を発動
        await _magicInvoker.InvokeMagicAsync(invokableMagic);

        //巨大生物の睡眠演出を再度流す
        if (!_bigCreatureStatus.IsWakeUp) _bigCreatureSleepPlayer.Play();

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
