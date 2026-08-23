using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//地面の草の量を変化させる機能

public class GroundGrassSetter : MonoBehaviour
{
    [SerializeField]
    GroundGrassAlphaController _groundGrassAlphaController;

    [Tooltip("草の量の変化量(0を最低値、1を最大値として設定する)")] [SerializeField] [Range(0, 1)]
    float _alphaDeltaRate;

    [SerializeField]
    float _shiftGrassAmountDuration;

    //地面に草を生やす
    public async UniTask GrowGrassOnGroundAsync()
    {
        float newAlphaRate = CalcNewGrassAlpha();
        await _groundGrassAlphaController.SetGrassAlphaAsync(newAlphaRate, _shiftGrassAmountDuration);
    }

    float CalcNewGrassAlpha()
    {
        float newAlphaRate = _groundGrassAlphaController.CurrentAlphaRate + _alphaDeltaRate;
        return Mathf.Clamp01(newAlphaRate);
    }
}
