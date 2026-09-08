using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

//作成者:杉山
//魔法陣を閉じる処理をするクラス

public class MagicCircleCloseManager : MonoBehaviour
{
    [Tooltip("魔法陣のなぞった線を描画する機能")] [SerializeField]
    MagicSphereTrail _magicSphereTrail;

    [Tooltip("魔法陣の表示・非表示をする機能")] [SerializeField]
    MagicCircleActiveHandler _magicCircleActiveHandler;

    [SerializeField]
    InvokedMagicIconEffect _invokedMagicIconEffect;

    [SerializeField]
    float _delayDurationFromInvokedMagicIconEffectFromHideMagicCircle = 1f;

    public async UniTask CloseAsync(EMagic invokedMagic)
    {
        var token = this.GetCancellationTokenOnDestroy();

        //発動した魔法のアイコンを表示する
        _invokedMagicIconEffect.PlayAsync(invokedMagic).Forget();

        //少し遅らせる
        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationFromInvokedMagicIconEffectFromHideMagicCircle), cancellationToken: token);

        //魔法陣の線を目立たせる
        _magicSphereTrail.Activate();

        //魔法陣と魔法陣の線を非表示にする
        await _magicCircleActiveHandler.DeActivateMagicCircleAsync(token);
    }
}
