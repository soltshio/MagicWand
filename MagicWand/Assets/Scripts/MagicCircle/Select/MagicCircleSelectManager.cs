using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//魔法の選択

public class MagicCircleSelectManager : MonoBehaviour
{
    [SerializeField]
    MagicSpheresList _magicSpheresList;

    [SerializeField]
    CastHistory _castHistory;

    [SerializeField]
    MagicSphereLeadEffectController _magicSphereLeadEffectController;

    EMagic[] _dummyPreTraceSelectableMagics;

    //魔法選択処理(選択した魔法球の番号と魔法の種類を返す)
    //magicFirstTraceIndexArrayはそれぞれの魔法の最初になぞる魔法球の番号配列(要素番号がそのまま最初になぞる魔法球の番号となっている)
    public async UniTask<(int,EMagic)> SelectAsync(EMagic[] magicFirstTraceIndexArray, CancellationToken ct)
    {
        //なぞれるものだけ杖で差した時に反応があるようにする
        SwitchEnableSelectableMagicSphereComponent(magicFirstTraceIndexArray,true);

        //誘導演出
        _magicSphereLeadEffectController.ActiveLeadEffect(null, _dummyPreTraceSelectableMagics, magicFirstTraceIndexArray);

        //いずれかが決定されるまで待つ
        MagicSphereHoverTouchChecker magicSphereHoverTouchChecker = new(_magicSpheresList);
        int selectedMagocSphereindex = await magicSphereHoverTouchChecker.WaitUntilSubmitAnyMagicSphere(ct);

        //詠唱の履歴に追加
        _castHistory.CreateNewCastInfo();
        _castHistory.AddNewTraceInfo(magicFirstTraceIndexArray, selectedMagocSphereindex);

        //杖で指しても反応がないようにする
        SwitchEnableSelectableMagicSphereComponent(magicFirstTraceIndexArray, false);

        //決定された球番号と魔法の種類を返す
        EMagic selectedMagic = magicFirstTraceIndexArray[selectedMagocSphereindex];
        return (selectedMagocSphereindex, selectedMagic);
    }

    //選択可能な魔法球の選択時に必要な機能のオンオフを切り替える
    void SwitchEnableSelectableMagicSphereComponent(EMagic[] magicFirstTraceIndexArray,bool isEnabled)
    {
        for(int i=0; i<magicFirstTraceIndexArray.Length ;i++)
        {
            if (magicFirstTraceIndexArray[i] == EMagic.None) continue;

            var hoverTouchReceiver = _magicSpheresList.GetComponentFromMagicSphere<HoverTouchedReceiver>(i);
            var selectMagicSphereEffect = _magicSpheresList.GetComponentFromMagicSphere<SelectMagicSphereEffect>(i);

            hoverTouchReceiver.enabled = isEnabled;
            selectMagicSphereEffect.enabled = isEnabled;
        }
    }

    void Awake()
    {
        InitializePreTraceSelectableMagics();
    }

    //誘導演出の前回のなぞりで選択可能な魔法の種類の引数に入れる用のダミーデータの初期化を行う(1詠唱の一番最初のなぞりなので当然、全てNoneに設定しておく)
    void InitializePreTraceSelectableMagics()
    {
        
        _dummyPreTraceSelectableMagics = new EMagic[_magicSpheresList.MagicSphereObjects.Length];

        for(int sphereIndex =0;sphereIndex<_dummyPreTraceSelectableMagics.Length ;sphereIndex++)
        {
            _dummyPreTraceSelectableMagics[sphereIndex] = EMagic.None;
        }
    }
}
