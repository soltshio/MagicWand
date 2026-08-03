using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//誘導エフェクト

public class LeadEffect : MonoBehaviour
{
    float _moveDuration;
    Vector3 _start;
    Vector3 _end;

    public void Initialize(float moveDuration, Vector3 start,Vector3 end)
    {
        _start = start;
        _end = end;
        _moveDuration = moveDuration;

        transform.position = start;
    }

    async void Start()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        ProgressTimer _progressTimer = new(_moveDuration);

        while(!_progressTimer.IsFinished)
        {
            _progressTimer.Tick();

            float progress = _progressTimer.CalcProgress();

            SetPos(progress);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }
    }

    //progressが0の時はstart、1の時はendの位置になるように移動させる
    void SetPos(float progress)
    {
        transform.position = Vector3.Lerp(_start, _end, progress);
    }
}
