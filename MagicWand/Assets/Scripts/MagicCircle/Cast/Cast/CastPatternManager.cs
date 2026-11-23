using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//魔法の発動パターンの管理
//最初の番号が被らないようにしている


public class CastPatternManager : MonoBehaviour
{
    [System.Serializable]
    struct CastOrderProperty
    {
        [Tooltip("魔法の種類")]
        public EMagic magic;

        [Tooltip("最初の球以降のなぞる順番\n要素数は球の数-1に合わせてください")]
        public int[] afterTheFirstCastOrder;
    }

    [Tooltip("最初になぞる球とそれに対応する魔法\n要素数は球の数に合わせてその要素番号を最初になぞるべき球の番号として設定してください")] [SerializeField]
    CastOrderProperty[] _castPattern;

    //それぞれの魔法の最初になぞる魔法球の番号配列(要素番号がそのまま最初になぞる魔法球の番号となっている)
    public EMagic[] MagicFirstTraceIndexArray()
    {
        EMagic[] ret = new EMagic[_castPattern.Length];

        for(int i=0; i<_castPattern.Length ;i++)
        {
            ret[i] = _castPattern[i].magic;
        }

        return ret;
    }

    //指定した魔法の種類の最初の番号以降の発動手順を取得する(指定した魔法の要素がなければnullを返す)
    public int[] AfterTheFirstCastOrder(EMagic magic)
    {
        for(int i=0; i<_castPattern.Length ;i++)
        {
            if (_castPattern[i].magic != magic) continue;
            return _castPattern[i].afterTheFirstCastOrder;
        }

        return null;
    }

    //全ての魔法の発動手順を作成して取得(最初の番号は絶対に被らないようになっている)
    public Dictionary<EMagic, int[]> AllMagicCastPatterns()
    {
        Dictionary<EMagic, int[]> retCastPatterns = new();

        for(int i=0;i<_castPattern.Length ;i++)
        {
            var castOrderProperty = _castPattern[i];

            //魔法が設定されていなければであれば無視
            if (castOrderProperty.magic == EMagic.None) continue;

            int[] castOrder = CreateCastOrder(i, castOrderProperty.afterTheFirstCastOrder);
            retCastPatterns.Add(castOrderProperty.magic, castOrder);
        }

        return retCastPatterns;
    }

    int[] CreateCastOrder(int firstIndex, int[] afterTheFirstCastOrder)
    {
        //順番を作成
        int[] castOrder = new int[afterTheFirstCastOrder.Length + 1];

        //最初になぞるべき番号を設定
        castOrder[0] = firstIndex;

        //最初以降の番号を設定
        for(int i=0; i< afterTheFirstCastOrder.Length; i++)
        {
            castOrder[i + 1] = afterTheFirstCastOrder[i];
        }

        return castOrder;
    }
}
