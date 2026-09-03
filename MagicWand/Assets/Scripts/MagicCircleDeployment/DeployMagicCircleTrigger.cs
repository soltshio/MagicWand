using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//魔法陣展開のトリガー(ボタン)

public class DeployMagicCircleTrigger : MonoBehaviour
{
    [SerializeField]
    TouchedReceiver _touchedReceiver;

    [SerializeField]
    GameObject _triggerGaugeMesh;

    [SerializeField]
    DeployMagicCircleTriggerHoverGauge _deployMagicCircleTriggerHoverGauge;

    [Tooltip("展開までにカーソルを合わせ続けるボタン")] [SerializeField]
    float _hoverDurationToDeploy=2f;

    float _progress = 0f;
    const float _minProgress = 0f;
    const float _maxProgress = 1f;

    SingleTaskCancellation _singleTaskCancellation = new();

    //展開操作が決定されるまで待つ
    public async UniTask WaitForSubmitAsync()
    {
        //初期化&トリガーを表示
        UpdateProgress(_minProgress);

        _touchedReceiver.OnTouchedEnter += OnEnter;
        _touchedReceiver.OnTouchedExit += OnExit;

        _triggerGaugeMesh.SetActive(true);

        //決定されるまで待つ
        var ct = this.GetCancellationTokenOnDestroy();

        await UniTask.WaitUntil(() => IsSubmitted(), cancellationToken: ct);

        //トリガーを隠す
        _touchedReceiver.OnTouchedEnter -= OnEnter;
        _touchedReceiver.OnTouchedExit -= OnExit;

        _triggerGaugeMesh.SetActive(false);
    }

    void OnEnter()
    {
        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        HoverAsync(ct).Forget();
    }

    void OnExit()
    {
        _singleTaskCancellation.Cancel();
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
        _triggerGaugeMesh.SetActive(false);
    }
}
