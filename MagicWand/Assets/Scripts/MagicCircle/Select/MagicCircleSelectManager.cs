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

    public async UniTask<int> SelectAsync(Dictionary<EMagic, int[]> castPatterns, CancellationToken ct)
    {
        //受け取った詠唱パターンから最初に選べる魔法リストを作成

        //なぞれるものだけ杖で差した時に反応があるようにする(HoverTouchReceiverとSelectMagicSphereEffectをONにする)
        //誘導演出

        //いずれかが決定されるまで待つ

        //杖で指しても反応がないようにする(HoverTouchReceiverとSelectMagicSphereEffectをOFFにする)

        //決定された球番号を返す

        return 0;
    }


}
