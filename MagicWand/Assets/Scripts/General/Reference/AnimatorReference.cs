using UnityEngine;

//作成者:杉山
//Animatorの参照(参照が無くなっても入れ直すのを楽にするためのもの)

public class AnimatorReference : MonoBehaviour
{
    [SerializeField]
    Animator _animator;

    public Animator Animator
    {
        get { return _animator; }
    }
}
