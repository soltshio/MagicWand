using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

//作成者:杉山
//大きな生物の身体表面の土コントローラー

public class BigCreatureSoilController : MonoBehaviour
{
    [SerializeField]
    MeshRenderer[] _bigCreatureBodyMeshRenderers;

    [SerializeField]
    float _minSoilValue = -1.83f;

    [SerializeField]
    [Min(0)]
    float _rangeSoilValue = 2.37f;

    [Tooltip("でか生き物の初期の土の量(0を最低値、1を最大値として設定する)")] [SerializeField] [Range(0,1)]
    float _defaultSoilValueRate = 0.25f;

    Material[] _materials;

    CancellationTokenSource _cts;

    static readonly int _soilBoundaryHeightID = Shader.PropertyToID("_SoilBoundaryHeight");

    private float MaxSoilValue { get { return _minSoilValue + _rangeSoilValue; } }

    public float CurrentSoilValueRate { get { return FromSoilValueToRate(_materials[0].GetFloat(_soilBoundaryHeightID));} }

    //でか生き物の土の量を変化させる(duration秒かけてtoAlphaRateまで土の量が変わっていく。)
    //toAlphaRateは0を最低値、1を最大値として設定する
    public async UniTask SetSoilValueAsync(float toSoilValueRate, float duration)
    {
        toSoilValueRate = Mathf.Clamp01(toSoilValueRate);

        CancelRunningUniTask();

        var ct = CreateLinkedToken(this.GetCancellationTokenOnDestroy());

        float toSoilValue = FromRateToSoilValue(toSoilValueRate);

        List<UniTask> runningTasks = new();

        for(int i=0; i<_materials.Length ;i++)
        {
            runningTasks.Add(_materials[i].DOFloat(toSoilValue, _soilBoundaryHeightID, duration).ToUniTask(cancellationToken: ct));
        }

        await UniTask.WhenAll(runningTasks);
    }

    void Awake()
    {
        GetBodyMaterial();
    }

    void Start()
    {
        float newValue = FromRateToSoilValue(_defaultSoilValueRate);

        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat(_soilBoundaryHeightID, newValue);
        }
    }

    //レンダラーから各部のマテリアルを取得
    void GetBodyMaterial()
    {
        _materials = new Material[_bigCreatureBodyMeshRenderers.Length];

        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i] = _bigCreatureBodyMeshRenderers[i].material;
        }
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

    float FromRateToSoilValue(float rate)
    {
        return Mathf.Lerp(_minSoilValue, MaxSoilValue, rate);
    }

    float FromSoilValueToRate(float value)
    {
        return Mathf.InverseLerp(_minSoilValue, MaxSoilValue, value);
    }
}
