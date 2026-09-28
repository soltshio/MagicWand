using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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
            _tutorialCanvas.enabled = false;

            await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: ct);//確認のためにも２秒ほど待ってみる

            _tutorialCanvas.enabled = true;

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

    public void Skip()
    {
        _singleTaskCancellation.Cancel();
    }

    void Start()
    {
        _tutorialCanvas.enabled = false;
    }
}
