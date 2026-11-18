using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//作成者:杉山
//魔法陣の表示・非表示をする

public class MagicCircleActiveHandler : MonoBehaviour
{
    [Tooltip("表示・非表示にかける時間")] [SerializeField]
    float _fadeDuration = 1.5f;

    [Header("魔法陣関係")] [SerializeField]
    MagicCircleActiveHandler_MagicCircle _magicCircle;

    [Header("魔法陣の球関係")] [SerializeField]
    MagicCircleActiveHandler_MagicSphere _magicSphere;

    [Header("魔法陣の線関係")] [SerializeField]
    MagicCircleActiveHandler_MagicTrail _magicTrail;

    bool _isProcessing = false;

    void Start()
    {
        _magicCircle.Start();

        _magicSphere.Start();
    }

    public async UniTask ActivateMagicCircleAsync(CancellationToken ct)
    {
        if (_isProcessing) return;
        _isProcessing = true;

        //魔法陣を表示
        _magicCircle.Show();

        //魔法陣の球を表示
        _magicSphere.ShowDefaultAllMagicSpheres();

        //少し待つ
        await UniTask.Delay(TimeSpan.FromSeconds(_fadeDuration), cancellationToken: ct);

        _isProcessing = false;
    }

    //魔法陣の非表示、トークンがキャンセルされれば表示にする
    public async UniTask DeActivateMagicCircleAsync(CancellationToken ct)
    {
        if (_isProcessing) return;
        _isProcessing = true;

        //魔法陣の非表示アニメーションを開始する
        _magicCircle.StartHide();

        //魔法球の非表示処理を開始
        _magicSphere.HideAllMagicSpheres(ct).Forget();

        //魔法陣の線を非表示にし始める
        _magicTrail.HideMagicTrail(_fadeDuration);

        //少し待つ
        await UniTask.Delay(TimeSpan.FromSeconds(_fadeDuration), cancellationToken: ct);

        //魔法陣を完全に非表示にする
        _magicCircle.CompleteHide();

        _isProcessing = false;
    }
}
