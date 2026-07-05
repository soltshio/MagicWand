using UnityEngine;

//作成者:杉山
//魔法の発動パターンの選出モード

public enum ECastPatternDecideMode
{
    Normal,//ノーマル(番号をずらしたりはしない)
    Shifting,//パターンを選出した後、全ての番号をずらして返すモード
}
