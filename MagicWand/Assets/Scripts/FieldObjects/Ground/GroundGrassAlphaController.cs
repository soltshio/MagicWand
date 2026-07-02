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

    [SerializeField]
    float _minAlpha;

    [SerializeField]
    float _rangeAlpha;

    [SerializeField]
    float _defaultAlpha;

    Material _groundMat;

    CancellationTokenSource _cts;

    static readonly int _grassAlphaID = Shader.PropertyToID("_GrassAlpha");

    public float MinAlpha { get { return _minAlpha; } }
    public float MaxAlpha { get { return _minAlpha + _rangeAlpha; } }
    public float CurrentAlpha { get { return _groundMat.GetFloat(_grassAlphaID); } }

    //草の量を変更する(今のalphaからtoAlphaまで、duration秒かけて変わっていく。)
    public async UniTask SetGrassAlphaAsync(float toAlpha,float duration)
    {
        toAlpha = ClampAlpha(toAlpha);

        CancelRunningUniTask();

        var ct = CreateLinkedToken(this.GetCancellationTokenOnDestroy());

        await _groundMat.DOFloat(toAlpha, _grassAlphaID, duration).ToUniTask(cancellationToken: ct);
    }

    void Awake()
    {
        _groundMat = _groundRenderer.material;
    }

    void Start()
    {
        float newGrassAlpha = ClampAlpha(_defaultAlpha);

        _groundMat.SetFloat(_grassAlphaID, newGrassAlpha);
    }

    float ClampAlpha(float value)
    {
        return Mathf.Clamp(value, MinAlpha, MaxAlpha);
    }

    void CancelRunningUniTask()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    CancellationToken CreateLinkedToken(CancellationToken ct)
    {
        _cts = new CancellationTokenSource();

        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, ct);

        return linkedCts.Token;
    }
}
