using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

//作成者:杉山
//地面の草の量をフェードで変化させるコントローラー

public class GroundGrassAlphaController : MonoBehaviour
{
    [SerializeField]
    MeshRenderer _groundRenderer;

    [SerializeField] [Min(0)]
    float _minAlpha;

    [SerializeField] [Min(0)]
    float _rangeAlpha;

    [Tooltip("草の量の初期値(0を最低値、1を最大値として設定する)")] [SerializeField] [Range(0,1)]
    float _defaultAlphaRate;

    Material _groundMat;

    SingleTaskCancellation _singleTaskCancellation = new();

    static readonly int _grassAlphaID = Shader.PropertyToID("_GrassAlpha");

    float MaxAlpha { get { return _minAlpha + _rangeAlpha; } }

    public float CurrentAlphaRate { get { return FromAlphaToRate(_groundMat.GetFloat(_grassAlphaID)); } }

    //草の量を変化させる(duration秒かけてtoAlphaRateまで草の量が変わっていく。)
    //toAlphaRateは0を最低値、1を最大値として設定する
    public async UniTask SetGrassAlphaAsync(float toAlphaRate,float duration)
    {
        toAlphaRate = Mathf.Clamp01(toAlphaRate);

        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        float toAlpha = FromRateToAlpha(toAlphaRate);
        await _groundMat.DOFloat(toAlpha, _grassAlphaID, duration).ToUniTask(cancellationToken: ct);
    }

    void Awake()
    {
        _groundMat = _groundRenderer.material;
    }

    void Start()
    {
        float newGrassAlpha = FromRateToAlpha(_defaultAlphaRate);

        _groundMat.SetFloat(_grassAlphaID, newGrassAlpha);
    }

    float FromRateToAlpha(float rate)
    {
        return Mathf.Lerp(_minAlpha, MaxAlpha, rate);
    }

    float FromAlphaToRate(float alpha)
    {
        return Mathf.InverseLerp(_minAlpha, MaxAlpha, alpha);
    }
}
