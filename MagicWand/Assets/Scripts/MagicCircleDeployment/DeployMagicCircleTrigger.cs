using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//作成者:杉山
//魔法陣展開のトリガー(ボタン)

public class DeployMagicCircleTrigger : MonoBehaviour
{
    [SerializeField]
    TouchedReceiver _touchedReceiver;

    [SerializeField]
    Animator _triggerGaugeAnimator;

    [SerializeField]
    AudioSource _hoverAudioSource;

    [SerializeField]
    DeployMagicCircleTriggerHoverGauge _deployMagicCircleTriggerHoverGauge;

    [Tooltip("展開までにカーソルを合わせ続ける秒数")] [SerializeField]
    float _hoverDurationToDeploy=2f;

    [Tooltip("ゲージを非表示アニメーションを開始してから完全に非表示にするまでの時間")] [SerializeField]
    float _waitDurationToDeactiveGauge = 1.5f;

    float _progress = 0f;
    const float _minProgress = 0f;
    const float _maxProgress = 1f;

    SingleTaskCancellation _singleTaskCancellation = new();

    //展開操作が決定されるまで待つ
    public async UniTask WaitForSubmitAsync()
    {
        //初期化
        UpdateProgress(_minProgress);

        _touchedReceiver.enabled = true;
        _touchedReceiver.OnTouchedEnter += OnEnter;
        _touchedReceiver.OnTouchedExit += OnExit;

        //トリガーを表示
        ShowGauge();

        //決定されるまで待つ
        var ct = this.GetCancellationTokenOnDestroy();

        await UniTask.WaitUntil(() => IsSubmitted(), cancellationToken: ct);

        _touchedReceiver.enabled = false;
        _touchedReceiver.OnTouchedEnter -= OnEnter;
        _touchedReceiver.OnTouchedExit -= OnExit;

        //トリガーを隠す
        HideGaugeAsync(ct).Forget();
    }

    void OnEnter()
    {
        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        _hoverAudioSource.Play();

        HoverAsync(ct).Forget();
    }

    void OnExit()
    {
        _singleTaskCancellation.Cancel();

        _hoverAudioSource.Stop();
        UpdateProgress(_minProgress);
    }

    async UniTask HoverAsync(CancellationToken ct)
    {
        ProgressTimer progressTimer = new(_hoverDurationToDeploy);

        while(!progressTimer.IsFinished)
        {
            progressTimer.Tick();
            float progress = progressTimer.CalcProgress();

            UpdateProgress(progress);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }

        UpdateProgress(_maxProgress);
        _hoverAudioSource.Stop();
    }

    void ShowGauge()
    {
        _triggerGaugeAnimator.gameObject.SetActive(true);
        _triggerGaugeAnimator.SetTrigger(DeployTriggerGaugeAnimatorProperty.ShowTriggerName);
    }

    async UniTask HideGaugeAsync(CancellationToken ct)
    {
        _triggerGaugeAnimator.SetTrigger(DeployTriggerGaugeAnimatorProperty.HideTriggerName);

        //少し待ってからゲージを完全に非表示にする
        await UniTask.Delay(TimeSpan.FromSeconds(_waitDurationToDeactiveGauge), cancellationToken: ct);

        _triggerGaugeAnimator.gameObject.SetActive(false);
    }

    void UpdateProgress(float value)
    {
        value = Mathf.Clamp01(value);

        _progress = value;
        _deployMagicCircleTriggerHoverGauge.SetGauge(value);
    }

    bool IsSubmitted()
    {
        return _progress >= _maxProgress;
    }

    void Start()
    {
        _triggerGaugeAnimator.gameObject.SetActive(false);
        _touchedReceiver.enabled = false;
    }
}
