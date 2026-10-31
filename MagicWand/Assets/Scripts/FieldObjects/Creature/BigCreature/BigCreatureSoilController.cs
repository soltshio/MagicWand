using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//大きな生物の身体表面の土コントローラー

public class BigCreatureSoilController : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRendererReference _bigCreatureBodyMeshRenderer;

    [SerializeField]
    float _minSoilValue = -1.83f;

    [SerializeField]
    [Min(0)]
    float _rangeSoilValue = 2.37f;

    [Tooltip("でか生き物の初期の土の量(0を最低値、1を最大値として設定する)")] [SerializeField] [Range(0,1)]
    float _defaultSoilValueRate = 0.25f;

    Material _skinMaterial;

    SingleTaskCancellation _singleTaskCancellation = new SingleTaskCancellation();

    static readonly int _soilBoundaryHeightID = Shader.PropertyToID("_SoilBoundaryHeight");

    private float MaxSoilValue { get { return _minSoilValue + _rangeSoilValue; } }

    public float CurrentSoilValueRate { get { return FromSoilValueToRate(_skinMaterial.GetFloat(_soilBoundaryHeightID));} }

    //でか生き物の土の量を変化させる(duration秒かけてtoAlphaRateまで土の量が変わっていく。)
    //toAlphaRateは0を最低値、1を最大値として設定する
    public async UniTask SetSoilValueAsync(float toSoilValueRate, float duration)
    {
        toSoilValueRate = Mathf.Clamp01(toSoilValueRate);

        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        float toSoilValue = FromRateToSoilValue(toSoilValueRate);

        List<UniTask> runningTasks = new();

        runningTasks.Add(_skinMaterial.DOFloat(toSoilValue, _soilBoundaryHeightID, duration).ToUniTask(cancellationToken: ct));

        await UniTask.WhenAll(runningTasks);
    }

    void Awake()
    {
        GetBodyMaterial();
    }

    void Start()
    {
        float newValue = FromRateToSoilValue(_defaultSoilValueRate);

        _skinMaterial.SetFloat(_soilBoundaryHeightID, newValue);
    }

    //レンダラーから各部のマテリアルを取得
    void GetBodyMaterial()
    {
        _skinMaterial = _bigCreatureBodyMeshRenderer.SkinnedMeshRenderer.material;
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
