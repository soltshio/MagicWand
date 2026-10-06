using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Rendering;

//作成者:杉山
//レンズフレアの強さをフェードさせる

public class LensFlareIntensityFader : MonoBehaviour
{
    [SerializeField]
    LensFlareComponentSRP _lensFlare;

    SingleTaskCancellation _singleTaskCancellation = new();

    //現在の光の強さ
    public float CurrentIntensity { get { return (_lensFlare != null) ? _lensFlare.intensity : 0f; } }

    public async UniTask FadeIntensityAsync(float toIntensity,float duration)
    {
        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        float fromIntensity = _lensFlare.intensity;

        try//duration秒かけて、だんだんと今のintensityからtoIntensityに変えていく
        {
            ProgressTimer progressTimer = new(duration);

            while (!progressTimer.IsFinished)
            {
                progressTimer.Tick();
                float progress = progressTimer.CalcProgress();

                _lensFlare.intensity = Mathf.Lerp(fromIntensity, toIntensity, progress);

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
            }

            _lensFlare.intensity = toIntensity;
        }
        catch(OperationCanceledException)//途中で処理が中断されたら変更前のintensityに戻しておく
        {
            if(_lensFlare!=null) _lensFlare.intensity = fromIntensity;
        }
        
    }
}
