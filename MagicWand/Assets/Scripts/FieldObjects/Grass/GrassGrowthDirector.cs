using UnityEngine;

//作成者：杉山
//草の成長の演出を管理する

public class GrassGrowthDirector : MonoBehaviour
{
    [SerializeField]
    GrassGrowthController _grassGrowthController;

    [SerializeField]
    ParticleSystem _growthParticle;

    float _startGrowthRate=0;
    float _endGrowthRate=0;

    void Start()
    {
        _growthParticle.Stop();
    }

    //どのくらいの範囲で成長させるかを設定する
    public void SetGrowthRateRange(float growthDelta)
    {
        growthDelta = Mathf.Clamp01(growthDelta);

        _startGrowthRate = _grassGrowthController.CurrentGrowthRate;
        _endGrowthRate = Mathf.Clamp01(_startGrowthRate + growthDelta);
    }

    public void EnterGrowGrass()
    {
        _growthParticle.Play();
    }

    public void UpdateGrowGrass(float time)
    {
        time = Mathf.Clamp01(time);

        float newGrowthRate = Mathf.Lerp(_startGrowthRate, _endGrowthRate, time);
        _grassGrowthController.SetGrowth(newGrowthRate);
    }

    public void ExitGrowGrass()
    {
        _growthParticle.Stop();
    }
}
