using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//作成者:杉山
//土の量を変化させる

[System.Serializable]
public class ShifterBigCreatureSoilMaterial
{
    [SerializeField]
    MeshRenderer[] _bigCreatureBodyMeshRenderers;

    [SerializeField]
    float _minValue_SoilBoundaryHeight;

    [SerializeField] [Min(0)]
    float _range_SoilBoundaryHeight;

    [SerializeField]
    float _defaultValue_SoilBoundaryHeight;

    [SerializeField]
    float _delta;

    [SerializeField]
    float _shiftDuration;

    Material[] _materials;
    float _currentValue;

    CancellationTokenSource _cts;

    static readonly int _soilBoundaryHeightID = Shader.PropertyToID("_SoilBoundaryHeight");

    private float MaxValue { get { return _minValue_SoilBoundaryHeight + _range_SoilBoundaryHeight; } }

    public void Start()
    {
        _materials = new Material[_bigCreatureBodyMeshRenderers.Length];
        
        for(int i=0; i<_materials.Length ;i++)
        {
            _materials[i] = _bigCreatureBodyMeshRenderers[i].material;
        }

        float value = ClampValue(_defaultValue_SoilBoundaryHeight);

        SetSoilFillAmount(value);

        _currentValue = value;
    }

    public void AddSoil(CancellationToken ct)
    {
        float targetValue = ClampValue(_currentValue - _delta);

        CancelRunningUniTask();

        var token = CreateLinkedToken(ct);

        ShiftAsync(token, _shiftDuration,_currentValue,targetValue).Forget();
    }

    public void RemoveSoil(CancellationToken ct)
    {
        float targetValue = ClampValue(_currentValue + _delta);

        CancelRunningUniTask();

        var token = CreateLinkedToken(ct);

        ShiftAsync(token, _shiftDuration, _currentValue, targetValue).Forget();
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

    async UniTask ShiftAsync(CancellationToken ct,float duration,float from,float to)
    {
        _currentValue = to;

        try
        {
            float elapsed = 0;

            while (true)
            {
                elapsed += Time.deltaTime;

                float rate = elapsed / duration;

                float value = Mathf.Lerp(from, to, rate);

                SetSoilFillAmount(value);

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);

                if (elapsed > duration) break;
            }
        }
        catch
        {

        }
    }

    float ClampValue(float value)
    {
        return Mathf.Clamp(value, _minValue_SoilBoundaryHeight, MaxValue);
    }

    void SetSoilFillAmount(float value)
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat(_soilBoundaryHeightID, value);
        }
    }
}
