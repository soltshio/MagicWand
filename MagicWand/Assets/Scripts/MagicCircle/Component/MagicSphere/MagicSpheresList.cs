using UnityEngine;

//作成者:杉山
//魔法陣上の魔法球を一括管理するクラス

public class MagicSpheresList : MonoBehaviour
{
    [Tooltip("12時の方向から時計回りに入れるようにしてください")] [SerializeField]
    GameObject[] _magicSphereObjs;

    [Tooltip("12時の方向から時計回りに入れるようにしてください")] [SerializeField]
    MagicSphereVer3[] _magicSpheres; //魔法陣上の球の配列

    ComponentCache[] _magicSphereComponentCaches;

    //public MagicSphereVer3[] MagicSpheres { get { return _magicSpheres; } }
    //public MagicSphereVer3 this[int index] { get { return _magicSpheres[index]; } }

    void Awake()
    {
        _magicSphereComponentCaches = new ComponentCache[_magicSphereObjs.Length];

        for(int i=0; i<_magicSphereComponentCaches.Length ;i++)
        {
            if (_magicSphereObjs[i] == null) continue;

            _magicSphereComponentCaches[i] = new(_magicSphereObjs[i]);
        }
    }
}