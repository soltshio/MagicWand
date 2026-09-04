using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//魔法陣を閉じる処理をするクラス

public class MagicCircleCloseManager : MonoBehaviour
{
    [Tooltip("魔法陣のなぞった線を描画する機能")] [SerializeField]
    MagicSphereTrail _magicSphereTrail;

    [Tooltip("魔法陣の表示・非表示をする機能")] [SerializeField]
    MagicCircleActiveHandler _magicCircleActiveHandler;

    public async UniTask CloseAsync(EMagic invokedMagic)
    {
        var token = this.GetCancellationTokenOnDestroy();

        //発動した魔法のアイコンを表示する

        //少し遅らせる

        //魔法陣の線を目立たせる
        _magicSphereTrail.Activate();

        //魔法陣と魔法陣の線を非表示にする
        await _magicCircleActiveHandler.DeActivateMagicCircleAsync(token);
    }
}
