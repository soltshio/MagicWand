using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

//作成者:杉山
//魔法陣展開のトリガー(ボタン)

public class DeployMagicCircleTrigger : MonoBehaviour
{
    [SerializeField]
    HoverTouchedReceiver _hoverTouchedReceiver;

    [SerializeField]
    Animator _triggerGaugeAnimator;

    [SerializeField]
    AudioSource _hoverAudioSource;

    [SerializeField]
    DeployMagicCircleTriggerHoverGauge _deployMagicCircleTriggerHoverGauge;

    [Tooltip("ゲージを非表示アニメーションを開始してから完全に非表示にするまでの時間")] [SerializeField]
    float _waitDurationToDeactiveGauge = 1.5f;

    bool _isSubmitted = false;

    //展開操作が決定されるまで待つ
    public async UniTask WaitForSubmitAsync(CancellationToken ct)
    {
        //初期化
        Initialize();

        EnableHoverTouchReceiver();

        //トリガーを表示
        ShowTriggerGauge();

        //決定されるまで待つ
        await UniTask.WaitUntil(() => _isSubmitted, cancellationToken: ct);

        DisableHoverTouchReceiver();

        //以前と同じ挙動にするため、決定されたらゲージの溜まり具合をリセットする
        ResetGauge();

        //トリガーを隠す
        HideTriggerGaugeAsync(ct).Forget();
    }

    void EnableHoverTouchReceiver()
    {
        _hoverTouchedReceiver.enabled = true;

        _hoverTouchedReceiver.OnTouchedEnter += OnEnter;
        _hoverTouchedReceiver.OnTouchedExit += OnExit;
        _hoverTouchedReceiver.OnSubmitted += OnSubmitted;
    }

    void DisableHoverTouchReceiver()
    {
        _hoverTouchedReceiver.enabled = false;

        _hoverTouchedReceiver.OnTouchedEnter -= OnEnter;
        _hoverTouchedReceiver.OnTouchedExit -= OnExit;
        _hoverTouchedReceiver.OnSubmitted -= OnSubmitted;
    }

    //カーソルがHoverTouchedReceiverと合わさった瞬間
    void OnEnter()
    {
        _hoverAudioSource.Play();
    }

    //カーソルがHoverTouchedreceiverから離れた瞬間
    void OnExit()
    {
        _hoverAudioSource.Stop();
    }

    //HoverTouchedReceiverが決定通知を出した時
    void OnSubmitted()
    {
        _isSubmitted = true;
    }

    

    void ShowTriggerGauge()
    {
        _triggerGaugeAnimator.gameObject.SetActive(true);
        _triggerGaugeAnimator.SetTrigger(DeployTriggerGaugeAnimatorProperty.ShowTriggerName);
    }

    async UniTask HideTriggerGaugeAsync(CancellationToken ct)
    {
        _triggerGaugeAnimator.SetTrigger(DeployTriggerGaugeAnimatorProperty.HideTriggerName);

        //少し待ってからゲージを完全に非表示にする
        await UniTask.Delay(TimeSpan.FromSeconds(_waitDurationToDeactiveGauge), cancellationToken: ct);

        _triggerGaugeAnimator.gameObject.SetActive(false);
    }

    //初期化
    void Initialize()
    {
        _isSubmitted = false;
        ResetGauge();
    }

    //ゲージをリセット
    void ResetGauge()
    {
        _deployMagicCircleTriggerHoverGauge.SetGauge(0);
    }

    //ゲージの更新
    void UpdateGauge()
    {
        //決定されているなら更新処理は行わない
        if (_isSubmitted) return;

        _deployMagicCircleTriggerHoverGauge.SetGauge(_hoverTouchedReceiver.HoverProgress);
    }

    void Start()
    {
        _triggerGaugeAnimator.gameObject.SetActive(false);
        _hoverTouchedReceiver.enabled = false;
    }

    void Update()
    {
        UpdateGauge();
    }
}
