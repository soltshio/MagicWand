using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

//作成者:杉山
//カーソルの位置を調整するシーンの制御

public class CursorAdjustSceneManager : MonoBehaviour
{
    [SerializeField]
    Start_CursorAdjust _start_CursorAdjust;

    [SerializeField]
    Finish_CursorAdjust _finish_CursorAdjust;

    async void Start()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        //補正開始
        await _start_CursorAdjust.InformStartAsync(ct);

        //中心に合わせる

        //画面の中心補正処理を行う

        //右上に合わせる
        //右下に合わせる
        //左下に合わせる

        //画面の大きさ補正処理を行う

        //補正終了
        await _finish_CursorAdjust.InformFinishAsync(ct);

        //タイトルシーンに遷移
        LoadTitleScene();
    }

    void LoadTitleScene()
    {
        SceneManager.LoadScene(SceneNameList.TitleScene);
    }
}
