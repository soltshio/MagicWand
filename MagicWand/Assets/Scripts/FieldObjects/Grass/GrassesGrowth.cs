using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//草たちの成長演出を行う機能

public class GrassesGrowth : MonoBehaviour
{
    [SerializeField]
    GrassGrowthDirector[] _grassGrowthDirectors;

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

        //GrowthAsyncProcessの成長範囲の設定
        SetGrowthRateRange();

        //成長演出の開始
        EnterGrowth();

        //だんだん成長させていく
        while (!progressTimer.IsFinished)
        {
            progressTimer.Tick();

            float progress = progressTimer.CalcProgress();

            UpdateGrowth(progress);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }

        //成長演出の終了
        ExitGrowth();
    }

    void SetGrowthRateRange()
    {
        for (int i = 0; i < _grassGrowthDirectors.Length; i++)
        {
            _grassGrowthDirectors[i].SetGrowthRateRange(_growthDelta);
        }
    }

    void EnterGrowth()
    {
        for (int i = 0; i < _grassGrowthDirectors.Length; i++)
        {
            _grassGrowthDirectors[i].EnterGrowGrass();
        }
    }

    void UpdateGrowth(float progress)
    {
        for (int i = 0; i < _grassGrowthDirectors.Length; i++)
        {
            _grassGrowthDirectors[i].UpdateGrowGrass(progress);
        }
    }

    void ExitGrowth()
    {
        for (int i = 0; i < _grassGrowthDirectors.Length; i++)
        {
            _grassGrowthDirectors[i].ExitGrowGrass();
        }
    }

    bool IsCorrectMagic(EMagic magic)
    {
        return magic == EMagic.Rain || magic == EMagic.Time;
    }
}
