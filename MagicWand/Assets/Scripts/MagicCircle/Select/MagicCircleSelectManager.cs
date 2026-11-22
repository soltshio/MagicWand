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

    //魔法選択処理(選択した魔法球の番号と魔法の種類を返す)
    //magicFirstTraceIndexArrayはそれぞれの魔法の最初になぞる魔法球の番号配列(要素番号がそのまま最初になぞる魔法球の番号となっている)
    public async UniTask<(int,EMagic)> SelectAsync(EMagic[] magicFirstTraceIndexArray, CancellationToken ct)
    {
        SwitchEnableSelectableMagicSphereComponent(magicFirstTraceIndexArray,true);//なぞれるものだけ杖で差した時に反応があるようにする
        //誘導演出

        //いずれかが決定されるまで待つ

        SwitchEnableSelectableMagicSphereComponent(magicFirstTraceIndexArray, false);//杖で指しても反応がないようにする

        //決定された球番号と魔法の種類を返す
        return (0,EMagic.None);
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


}
