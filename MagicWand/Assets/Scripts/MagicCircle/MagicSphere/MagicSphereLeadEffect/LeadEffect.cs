using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

//作成者:杉山
//誘導エフェクト

public class LeadEffect : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _leadParticle;

    [SerializeField]
    float _leadEffectLifeTimeAfterFinishMove = 5f;

    float _moveDuration;
    Vector3 _start;
    Vector3 _end;

    static readonly int _emissionColorID = Shader.PropertyToID("_EmissionColor");

    public void Initialize(float moveDuration, Vector3 start,Vector3 end,Color leadEffectEmissionColor)
    {
        _start = start;
        _end = end;
        _moveDuration = moveDuration;

        SetLeadEffectEmissionColor(leadEffectEmissionColor);

        transform.position = start;
    }

    async void Start()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        try
        {
            ProgressTimer _progressTimer = new(_moveDuration);

            while (!_progressTimer.IsFinished)
            {
                _progressTimer.Tick();

                float progress = _progressTimer.CalcProgress();

                SetPos(progress);

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_leadEffectLifeTimeAfterFinishMove), cancellationToken: ct);
        }
        finally
        {
            Destroy(gameObject);
        }
    }

    //progressが0の時はstart、1の時はendの位置になるように移動させる
    void SetPos(float progress)
    {
        transform.position = Vector3.Lerp(_start, _end, progress);
    }

    void SetLeadEffectEmissionColor(Color leadEffectEmissionColor)
    {
        var renderer = _leadParticle.GetComponent<ParticleSystemRenderer>();

        if (renderer == null) return;

        Material mat = renderer.material;

        mat.SetColor(_emissionColorID, leadEffectEmissionColor);
    }
}
