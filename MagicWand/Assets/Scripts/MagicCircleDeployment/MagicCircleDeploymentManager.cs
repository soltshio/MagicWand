using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//魔法陣の展開の処理をするクラス

public class MagicCircleDeploymentManager : MonoBehaviour
{
    [Tooltip("魔法陣のなぞった線を描画する機能")] [SerializeField]
    MagicSphereTrail _magicSphereTrail;

    [Tooltip("魔法陣の表示・非表示をする機能")] [SerializeField]
    MagicCircleActiveHandler _magicCircleActiveHandler;

    public async UniTask DeployAsync()
    {
        var token = this.GetCancellationTokenOnDestroy();

        //魔法陣の線を消す
        _magicSphereTrail.ResetTrail();

        //魔法陣を表示する
        await _magicCircleActiveHandler.ActivateMagicCircleAsync(token);
    }
}
