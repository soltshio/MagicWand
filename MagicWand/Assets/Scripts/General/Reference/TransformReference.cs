using UnityEngine;

//作成者:杉山
//Transformの参照(参照が無くなっても入れ直すのを楽にするためのもの)

public class TransformReference : MonoBehaviour
{
    [SerializeField]
    Transform _trs;

    public Transform Transform
    {
        get { return _trs; }
    }
}
