using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

//作成者:杉山
//魔法陣の展開の処理をするクラス

public class MagicCircleDeploymentManager : MonoBehaviour
{
    [Tooltip("魔法陣のなぞった線を描画する機能")] [SerializeField]
    MagicSphereTrail _magicSphereTrail;

    [Tooltip("魔法陣の表示・非表示をする機能")] [SerializeField]
    MagicCircleActiveHandler _magicCircleActiveHandler;

    [SerializeField]
    DeployMagicCircleTrigger _deployMagicCircleTrigger;

    public async UniTask DeployAsync()
    {
        var token = this.GetCancellationTokenOnDestroy();

        //魔法陣展開のトリガーが押されるまで待つ
        await _deployMagicCircleTrigger.WaitForSubmitAsync();

        //魔法陣を表示(展開)する
        await DeployMagicCircleAsync(token);
    }

    async UniTask DeployMagicCircleAsync(CancellationToken ct)
    {
        //魔法陣の線を消す
        _magicSphereTrail.ResetTrail();

        //魔法陣を表示する
        await _magicCircleActiveHandler.ActivateMagicCircleAsync(ct);
    }
}
