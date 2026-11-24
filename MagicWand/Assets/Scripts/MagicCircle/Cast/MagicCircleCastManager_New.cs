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

    //発動手順が合っていたことの通知、第一引数に魔法の内容、第二引数に触れた球のインデックス、第三引数にこの1詠唱の中で何回目に球を触れたかを入れている
    public event Action<EMagic, int, int> OnSuccessToCast;
    public event Action OnStartToCast;//魔法の発動が始まったことの通知

    //魔法詠唱処理
    //selectedMagicは選んだ魔法の種類、afterTheFirstCastOrderは最初の番号以降の発動手順
    public async UniTask CastAsync(EMagic selectedMagic,int[] afterTheFirstCastOrder,CancellationToken ct)
    {
       
    }
}
