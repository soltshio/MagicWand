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

    MagicInvoker _magicInvoker;

    static readonly int _emissionColorID = Shader.PropertyToID("_EmissionColor");

    public void Initialize(float moveDuration, Vector3 start,Vector3 end,Color leadEffectEmissionColor,MagicInvoker magicInvoker)
    {
        //変数の初期化
        _start = start;
        _end = end;
        _moveDuration = moveDuration;
        _magicInvoker = magicInvoker;

        SetLeadEffectEmissionColor(leadEffectEmissionColor);

        //位置の初期化
        transform.position = start;

        //魔法発動時にエフェクトが見えなくなるようにする
        _magicInvoker.OnMagicInvoked += HideEffect;
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
        catch (OperationCanceledException)
        {
            // キャンセルされた場合は何もしない
        }
        finally
        {
            if (this != null)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnDestroy()
    {
        _magicInvoker.OnMagicInvoked -= HideEffect;
    }

    void HideEffect()
    {
        _leadParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
