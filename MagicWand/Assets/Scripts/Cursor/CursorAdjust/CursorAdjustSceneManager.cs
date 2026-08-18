using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//カーソルの位置を調整するシーンの制御

public class CursorAdjustSceneManager : MonoBehaviour
{
    [SerializeField]
    Start_CursorAdjust _start_CursorAdjust;

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

        //タイトルシーンに遷移
    }
}
