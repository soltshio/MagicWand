using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

//作成者：杉山
//地面の乾き度を操作する機能

public class GroundDryController : MonoBehaviour
{
    [Tooltip("乾き度の初期値")] [SerializeField] [Range(0,1)]
    float _defaultDryRate;

    [Tooltip("湿る際にどれだけ乾き度を変化させるか(減少させるか)")] [SerializeField] [Range(0, 1)]
    float _getWetDelta;

    [Tooltip("乾く際にどれだけ渇き度を変化させるか(増加させるか)")] [SerializeField] [Range(0, 1)]
    float _getDryDelta;

    [SerializeField]
    float _wetShiftDuration;

    [SerializeField]
    float _dryShiftDuration;

    [SerializeField]
    MeshRenderer _groundRenderer;

    Material _groundMaterial;

    static readonly int _dryRateID = Shader.PropertyToID("_DryRate");

    SingleTaskCancellation _singleTaskCancellation = new();

    public async UniTask SetGroundWetAsync()
    {
        //現在の値を取得
        float currentDryRate = _groundMaterial.GetFloat(_dryRateID);

        //目標の値を設定
        float targetDryRate = Mathf.Clamp01(currentDryRate - _getWetDelta);

        //目標の値までだんだんと変化させる
        await SetGroundAsync(targetDryRate, _wetShiftDuration);
    }

    public async UniTask SetGroundDryAsync()
    {
        //現在の値を取得
        float currentDryRate = _groundMaterial.GetFloat(_dryRateID);

        //目標の値を設定
        float targetDryRate = Mathf.Clamp01(currentDryRate + _getDryDelta);

        //目標の値までだんだんと変化させる
        await SetGroundAsync(targetDryRate, _dryShiftDuration);
    }


    async UniTask SetGroundAsync(float targetDryRate,float _shiftDuration)
    {
        //トークンを作成
        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        try
        {
            //目標の値までだんだんと変化させる
            await _groundMaterial.DOFloat(targetDryRate, _dryRateID, _shiftDuration).WithCancellation(ct);
        }
        finally//途中でキャンセルされても必ず目標の値に設定する
        {
            if (_groundMaterial != null)
            {
                _groundMaterial.SetFloat(_dryRateID, targetDryRate);
            }
        }
    }

    void Start()
    {
        _groundMaterial.SetFloat(_dryRateID, _defaultDryRate);
    }

    void Awake()
    {
        _groundMaterial = _groundRenderer.material;
    }
}
