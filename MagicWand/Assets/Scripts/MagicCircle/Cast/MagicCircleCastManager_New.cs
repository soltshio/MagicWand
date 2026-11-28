using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//魔法の発動

public class MagicCircleCastManager_New : MonoBehaviour
{
    [SerializeField]
    MagicSpheresList _magicSpheresList;

    [Tooltip("魔法陣のなぞった線を描画する機能")] [SerializeField]
    MagicSphereTrail _magicSphereTrail;

    [Tooltip("誘導エフェクトをコントロールする機能")] [SerializeField]
    MagicSphereLeadEffectController _magicSphereLeadEffectController;

    [SerializeField]
    CastHistory _castHistory;

    //なぞって発動手順を１つ進めたことを通知、第一引数に魔法の内容、第二引数に触れた球のインデックス、第三引数にこの1詠唱の中で何回目に球を触れたかを入れている
    public event Action<EMagic, int, int> OnTraceMagicSphere;

    //魔法詠唱処理
    //selectedMagicは選んだ魔法の種類、afterTheFirstCastOrderは最初の番号以降の発動手順
    public async UniTask CastAsync(EMagic selectedMagic,int[] afterTheFirstCastOrder,CancellationToken ct)
    {
        while(true)
        {
            //この１詠唱で球をなぞった回数を取得(最初の魔法の選択分は除く)
            int currentTouchCount = GetCurrentTouchCount();

            //もうなぞるべき球が無い、もしくは球をなぞった回数の取得に失敗した場合、脱出
            if (!MathfExtension.IsInRange(currentTouchCount,0, afterTheFirstCastOrder.Length-1)) break;

            //なぞるべき魔法球の番号(なぞるべき魔法球は１なぞりにつき１つまで)
            int touchableMagicSphereIndex = afterTheFirstCastOrder[currentTouchCount];

            //どの魔法球がどの魔法に対応しているのかの情報を取得する
            var selectableMagicsBySphere = GetSelectableMagicsBySphere(selectedMagic, touchableMagicSphereIndex);

            //TODO:全ての魔法球のマークの表示度を最大にしておく

            //なぞるべき魔法球だけアクティブ状態にしておく
            SwitchEnableTouchableMagicSphereComponent(touchableMagicSphereIndex, true);

            //誘導演出
            ActiveLeadEffct(selectableMagicsBySphere);



            //杖がアクティブ状態のいずれかの球に触れるまで待つ
            MagicSphereTouchChecker magicSphereTouchChecker = new(_magicSpheresList);
            await magicSphereTouchChecker.WaitUntilTouchAnyMagicSphere(ct);//決定された球番号



            //アクティブにしていた球を非アクティブ状態に戻す
            SwitchEnableTouchableMagicSphereComponent(touchableMagicSphereIndex, false);

            //何回目のなぞりでどの魔法球に触れたかを通知する
            OnTraceMagicSphere?.Invoke(selectedMagic, touchableMagicSphereIndex, currentTouchCount + 1);

            //詠唱の履歴に追加
            _castHistory.AddNewTraceInfo(selectableMagicsBySphere, touchableMagicSphereIndex);

            //なぞった球の位置を魔法陣の線の描画機能に伝える
            _magicSphereTrail.Add(_magicSpheresList.MagicSphereObjects[touchableMagicSphereIndex].transform.localPosition);
        }
    }

    //誘導演出
    void ActiveLeadEffct(EMagic[] selectableMagicsBySphere)
    {
        //前回のなぞりで選べた魔法とそれに対応する球の番号を取得(失敗したら誘導演出はしないようにする)
        if (!_castHistory.TryGetLatestTraceInfo(out var latestTraceInfo)) return;
        var preSelectableMagicsBySphere = latestTraceInfo.SelectableMagicsBySphere;

        //前回のなぞりで、どの球をなぞったか
        var preTouchedSphereIndex = GetPreTouchedSphereIndex();

        //誘導演出開始
        _magicSphereLeadEffectController.ActiveLeadEffect(preTouchedSphereIndex,preSelectableMagicsBySphere , selectableMagicsBySphere);
    }

    //どの魔法球がどの魔法に対応しているのかの情報を取得する
    EMagic[] GetSelectableMagicsBySphere(EMagic selectedMagic,int touchableSphereIndex)
    {
        EMagic[] selectableMagic_SphereIndexArray = new EMagic[_magicSpheresList.MagicSphereObjects.Length];

        for(int i=0;i<selectableMagic_SphereIndexArray.Length ;i++)
        {
            selectableMagic_SphereIndexArray[i] = (i == touchableSphereIndex) ? selectedMagic : EMagic.None;
        }

        return selectableMagic_SphereIndexArray;
    }

    //選択可能な魔法球の選択時に必要な機能のオンオフを切り替える
    void SwitchEnableTouchableMagicSphereComponent(int touchableMagicSphereIndex, bool isEnabled)
    {
        var touchReceiver = _magicSpheresList.GetComponentFromMagicSphere<TouchedReceiver>(touchableMagicSphereIndex);
        touchReceiver.enabled = isEnabled;
    }

    //この１詠唱で球をなぞった回数を取得(最初の魔法の選択分は除く)
    int GetCurrentTouchCount()
    {
        if (!_castHistory.TryGetLatestCastInfo(out var latestCastInfo)) return -1;

        //魔法選択時の最初の1回が含まれてしまっているため、その分は引いておく
        return latestCastInfo.TraceInfos.Count - 1;
    }

    int? GetPreTouchedSphereIndex()
    {
        if (!_castHistory.TryGetLatestTraceInfo(out var latestTraceInfo)) return null;

        return (int)latestTraceInfo.SelectedSphereIndex;
    }
}
