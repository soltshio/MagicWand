using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

//作成者:杉山
//雲をだんだんと表示・非表示する機能

public class CloudFadeController : MonoBehaviour
{
    [Tooltip("何秒かけて雲の表示・非表示を行うか")] [SerializeField]
    float _shiftDuration = 1.5f;

    [Tooltip("非表示時のAlphaOffsetの値")] [SerializeField]
    float _offAlphaOffset = -1f;

    [Tooltip("表示時のAlphaOffsetの値")] [SerializeField]
    float _onAlphaOffset = 0.56f;

    [SerializeField]
    MeshRenderer _cloudMeshRenderer;

    Material _cloudMat;
    CancellationTokenSource _cts;

    static readonly int _alphaOffsetID = Shader.PropertyToID("_AlphaOffset");

    public async UniTask ActivateAsync()
    {
        await ShiftCloudAlphaOffsetFromCurrentAsync(_onAlphaOffset);
    }

    public async UniTask DeactivateAsync()
    {
        await ShiftCloudAlphaOffsetFromCurrentAsync(_offAlphaOffset);
    }

    //現在の値から目標値(toAlpha)までAlphaOffsetをだんだんと変化させていく
    async UniTask ShiftCloudAlphaOffsetFromCurrentAsync(float toAlpha)
    {
        CancelRunningUniTask();

        var token = CreateLinkedToken(this.GetCancellationTokenOnDestroy());

        var duration = CalcDurationFromCurrentAlphaToDestinationAlpha(toAlpha);

        await _cloudMat.DOFloat(toAlpha, _alphaOffsetID, duration).ToUniTask(cancellationToken: token);
    }

    void Start()
    {
        _cloudMat = _cloudMeshRenderer.material;

        //シーン開始時は雲は非表示にしておく
        _cloudMat.SetFloat(_alphaOffsetID, _offAlphaOffset);
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

    //現在の透明度から目標の透明度までかかる時間を算出する
    float CalcDurationFromCurrentAlphaToDestinationAlpha(float destinationAlpha)
    {
        //最大時の差を求める
        float maxDelta = Mathf.Abs(_onAlphaOffset - _offAlphaOffset);

        if(maxDelta==0)//0除算になるのを避ける(この場合は一瞬で透明度が変わるようにする)
        {
            Debug.LogWarning("表示時と非表示時のalphaの差がありません！");
            return 0;
        }

        //現在の値と目標値との差を求める
        float currentAlpha = _cloudMat.GetFloat(_alphaOffsetID);
        float currentDelta = Mathf.Abs(destinationAlpha - currentAlpha);

        return _shiftDuration * currentDelta / maxDelta;
    }
}
