using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//チュートリアルのマネージャー

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

            var ct = InitSkipButtonAndCreateToken();

            //チュートリアルのセリフを流す
            await _tutorialTextPlayer.PlayTextAsync(ct,_lineContents);
        }
        finally
        {
            //チュートリアル用のUIを非表示にする
            if (_tutorialCanvas != null) _tutorialCanvas.enabled =false;
        }
    }

    //スキップボタンの初期化処理(スキップボタンを押した時にチュートリアルがスキップ出来るようにトークンを生成する)
    CancellationToken InitSkipButtonAndCreateToken()
    {
        //トークンを作成しておく
        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        return ct;
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
