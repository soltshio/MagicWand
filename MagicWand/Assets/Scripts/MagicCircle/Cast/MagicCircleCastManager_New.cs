using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//魔法の発動

public class MagicCircleCastManager_New : MonoBehaviour
{
    //魔法詠唱処理
    //selectedMagicは選んだ魔法の種類、afterTheFirstCastOrderは最初の番号以降の発動手順
    public async UniTask CastAsync(EMagic selectedMagic,int[] afterTheFirstCastOrder,CancellationToken ct)
    {
       
    }
}
