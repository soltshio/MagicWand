using Cysharp.Threading.Tasks;
using System;
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

    SingleTaskCancellation _singleTaskCancellation = new();

    public async UniTask PlayTutorialAsync()
    {
        try
        {
            _tutorialCanvas.enabled = true;//チュートリアル関係のUIを表示

            var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

            //チュートリアルのセリフを流す
            await _tutorialTextPlayer.PlayTextAsync(ct,_lineContents);


            //TODO:チュートリアルの内容を実装する
            //セリフを流す
            //杖を受け取る(光ったりする？)
            //セリフを流す
            //振ってみる
            //セリフを流す
            //魔法陣の展開
            //セリフを流す
            //日魔法だけを詠唱させる(日魔法発動まで)
            //セリフを流す
        }
        catch (OperationCanceledException)
        {
            //発生した例外をSkip()によるものとして扱い終了する(そうすることによって呼び出し元の方で例外が発生してその後の処理がされなくなるということを防ぐ)
            //TODO:画面暗転した後に初期化した後、画面を明転させて本編を始める
            //行わなければいけない初期化処理
            //1.魔法陣の線のリセット
            //2.球を全て非アクティブに
            //3.魔法陣の非表示
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
