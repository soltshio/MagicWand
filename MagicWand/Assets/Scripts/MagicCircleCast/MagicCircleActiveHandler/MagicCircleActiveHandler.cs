using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//魔法陣の表示・非表示をする

public class MagicCircleActiveHandler : MonoBehaviour
{
    [Tooltip("表示・非表示にかける時間")] [SerializeField]
    float _fadeDuration = 1f;

    [Header("魔法陣関係")] [SerializeField]
    MagicCircleRendererActivator_MagicCircleActiveHandler _magicCircleRendererActivator;

    [Header("魔法陣の球関係")] [SerializeField]
    MagicSphereRendererActivator_MagicCircleActiveHandler _magicSphereRendererActivator;

    [Header("魔法陣の線関係")] [SerializeField]
    MagicTrailRendererActivator_MagicCircleActiveHandler _magicTrailRendererActivator;

    bool _isProcessing = false;

    void Start()
    {
        //魔法陣は最初に非表示
        _magicCircleRendererActivator.Start();

        //魔法陣の球
        _magicSphereRendererActivator.Start();
    }

    //魔法陣の表示
    public async UniTask ActivateMagicCircleAsync(CancellationToken ct)
    {
        if (_isProcessing) return;
        _isProcessing = true;

        //魔法陣を表示
        _magicCircleRendererActivator.Show();

        ProgressTimer progressTimer = new(_fadeDuration);

        while(!progressTimer.IsFinished)
        {
            progressTimer.Tick();
            float progress = progressTimer.CalcProgress();

            //球の表示
            _magicSphereRendererActivator.ActivateMagicSphere(progress);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }

        //球の当たり判定をオンにする
        _magicSphereRendererActivator.MagicSphereCollidersSwitchEnable(true);

        _isProcessing = false;
    }

    //魔法陣の非表示
    public async UniTask DeActivateMagicCircleAsync(CancellationToken ct)
    {
        if (_isProcessing) return;
        _isProcessing = true;

        //魔法陣の非表示アニメーションを開始する
        _magicCircleRendererActivator.StartHide();

        //球の当たり判定をオフにする
        _magicSphereRendererActivator.MagicSphereCollidersSwitchEnable(false);

        //魔法陣の線を非表示にし始める
        _magicTrailRendererActivator.HideMagicTrail(_fadeDuration);

        ProgressTimer progressTimer = new(_fadeDuration);

        while (!progressTimer.IsFinished)
        {
            progressTimer.Tick();
            float progress = progressTimer.CalcProgress();

            //魔法陣の球
            _magicSphereRendererActivator.DeactivateMagicSphere(progress);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }

        //魔法陣を完全に非表示にする
        _magicCircleRendererActivator.CompleteHide();

        _isProcessing = false;
    }
}
