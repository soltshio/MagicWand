using UnityEngine;

//作成者:杉山
//Animatorの参照(参照が無くなっても入れ直すのを楽にするためのもの)

public class SkinnedMeshRendererReference : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRenderer _skinnedMeshRenderer;

    public SkinnedMeshRenderer SkinnedMeshRenderer
    {
        get { return _skinnedMeshRenderer; }
    }
}
