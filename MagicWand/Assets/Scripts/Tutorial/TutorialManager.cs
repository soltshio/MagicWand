using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//チュートリアルのマネージャー

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    Canvas _tutorialCanvas;

    SingleTaskCancellation _singleTaskCancellation = new();

    public async UniTask PlayTutorialAsync()
    {
        try
        {
            _tutorialCanvas.gameObject.SetActive(true);//チュートリアル関係のUIを表示

            var ct = InitSkipButtonAndCreateToken();

            //チュートリアルのセリフを流す
        }
        finally
        {
            //チュートリアル用のUIを非表示にする
            if (_tutorialCanvas != null) _tutorialCanvas.gameObject.SetActive(false);
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
        _tutorialCanvas.gameObject.SetActive(false);
    }
}
