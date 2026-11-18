using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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

    //魔法陣を閉じ始める際に呼ばれるコールバック(発動する魔法も引数に入れる)
    public event Action<EMagic> OnStartClosingMagicCircle;

    public async UniTask CloseAsync(EMagic invokedMagic,CancellationToken ct)
    {
        //魔法陣を閉じ始めるコールバックを呼び出す
        OnStartClosingMagicCircle?.Invoke(invokedMagic);

        //発動した魔法のアイコンを表示する
        _invokedMagicIconEffect.PlayAsync(invokedMagic,ct).Forget();

        //少し遅らせる
        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationFromInvokedMagicIconEffectFromHideMagicCircle), cancellationToken: ct);

        //魔法陣の線を目立たせる
        _magicSphereTrail.Activate();

        //魔法陣と魔法陣の線を非表示にする
        await _magicCircleActiveHandler.DeActivateMagicCircleAsync(ct);
    }
}
