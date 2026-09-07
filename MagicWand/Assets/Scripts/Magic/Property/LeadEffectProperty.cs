using UnityEngine;

//作成者:杉山
//誘導エフェクトのプロパティ

public class LeadEffectProperty : MonoBehaviour
{
    [SerializeField] [ColorUsage(true, true)]
    Color _leadEffectEmissionColor;

    public Color LeadEffectEmissionColor { get { return _leadEffectEmissionColor; } }//誘導エフェクトの色
}
