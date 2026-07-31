using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//草たちの成長演出を行う機能

public class GrassesGrowth : MonoBehaviour
{
    class GrowthAsyncProcess
    {
        float _startGrowthRate;
        float _endGrowthRate;
        GrassGrowthController _grassGrowthController;

        public GrowthAsyncProcess(float startGrowthRate,float endGrowthRate,GrassGrowthController grassGrowthController)
        {
            _startGrowthRate = startGrowthRate;
            _endGrowthRate = endGrowthRate;
            _grassGrowthController = grassGrowthController;
        }

        public void SetGrowth(float rate)
        {
            float newGrowthRate = Mathf.Lerp(_startGrowthRate, _endGrowthRate, rate);
            _grassGrowthController.SetGrowth(newGrowthRate);
        }
    }

    [SerializeField]
    GrassGrowthController[] _grassGrowthControllers;

    [Tooltip("1回魔法を撃つごとにどれくらい成長するようにするか")] [SerializeField] [Range(0,1)]
    float _growthDelta;

    [Tooltip("どのくらいの時間をかけて成長演出を行うか")] [SerializeField]
    float _growthDuration;

    public async UniTask TakeMagicAsync(EMagic magic)
    {
        //時間魔法と水魔法の時は草が成長するようにする。
        if (!IsCorrectMagic(magic)) return;

        var ct = this.GetCancellationTokenOnDestroy();
        ProgressTimer progressTimer = new ProgressTimer(_growthDuration);

        //GrowthAsyncProcessの初期化
        GrowthAsyncProcess[] growthAsyncProcesses = new GrowthAsyncProcess[_grassGrowthControllers.Length];

        for(int i=0; i< growthAsyncProcesses.Length; i++)
        {
            float start = _grassGrowthControllers[i].CurrentGrowthRate;
            float end = Mathf.Clamp01(start + _growthDelta);

            growthAsyncProcesses[i] = new(start, end, _grassGrowthControllers[i]);
        }

        //だんだん成長させていく
        while(!progressTimer.IsFinished)
        {
            progressTimer.Tick();

            float progress = progressTimer.CalcProgress();

            for (int i = 0; i < growthAsyncProcesses.Length; i++)
            {
                growthAsyncProcesses[i].SetGrowth(progress);
            }

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }
    }

    bool IsCorrectMagic(EMagic magic)
    {
        return magic == EMagic.Rain || magic == EMagic.Time;
    }
}
