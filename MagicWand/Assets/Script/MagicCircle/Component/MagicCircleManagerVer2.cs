using UnityEngine;

//作成者:杉山
//魔法陣の本体(次にどの球をアクティブにするかなどを決めるマネージャー)
//違う魔法が出せるようにしたver

public class MagicCircleManagerVer2 : MonoBehaviour
{
    [Tooltip("12時の方向から時計回りに入れるようにしてください")] [SerializeField]
    MagicSphere[] _magicSpheres; //魔法陣上の球の配列
}
