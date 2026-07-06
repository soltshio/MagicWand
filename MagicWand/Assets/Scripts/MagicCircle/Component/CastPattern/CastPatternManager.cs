using UnityEngine;
using System.Collections.Generic;

//作成者:杉山
//魔法の発動パターンの管理

public class CastPatternManager : MonoBehaviour
{
    [Tooltip("球のアクティブ化の順番(魔法の発動手順)\n発動パターンを何種類か設定可能\nMagicSpheresの要素番号(0～配列の要素数-1)を入力してください")] [SerializeField]
    SerializableDictionary<EMagic, int[]>[] _castPatternsArray;

    [SerializeField]
    ECastPatternDecideMode _castPatternDecideMode;

    [SerializeField]
    MagicSpheresList _magicSpheresList;

    //魔法の発動手順を決定(選択)して返す
    public Dictionary<EMagic, int[]> DecideActiveOrderIndexs()
    {
        int patternNum = Random.Range(0, _castPatternsArray.Length);

        Dictionary<EMagic, int[]> retCastPatterns = new(_castPatternsArray[patternNum]);

        if(_castPatternDecideMode==ECastPatternDecideMode.Shifting)//Shiftingモードの場合は番号をいくつかずらすようにする
        {
            ShiftIndex(ref retCastPatterns);
        }

        return retCastPatterns;
    }

    void ShiftIndex(ref Dictionary<EMagic, int[]> castPatterns)
    {
        int magicSpheresLength = _magicSpheresList.MagicSpheres.Length;

        int shiftNum = Random.Range(0,magicSpheresLength);

        if (shiftNum == 0) return;//ずらさなくていい場合

        foreach(var orderIndexs in castPatterns.Values)
        {
            for(int i=0; i<orderIndexs.Length; i++)
            {
                orderIndexs[i] += shiftNum;
                orderIndexs[i] %= magicSpheresLength;
            }
        }
    }
}
