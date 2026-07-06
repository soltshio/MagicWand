using UnityEngine;

//作成者:杉山
//魔法陣上の魔法球を一括管理するクラス

public class MagicSpheresList : MonoBehaviour
{
    [Tooltip("12時の方向から時計回りに入れるようにしてください")] [SerializeField]
    MagicSphereVer3[] _magicSpheres; //魔法陣上の球の配列

    public MagicSphereVer3[] MagicSpheres { get { return _magicSpheres; } }
    public MagicSphereVer3 this[int index] { get { return _magicSpheres[index]; } }

    //球を全て非アクティブにする
    public void AllMagicSpheresToDeactive()
    {
        foreach (var magicSphere in _magicSpheres)
        {
            magicSphere.ToDeactive();
        }
    }
}
