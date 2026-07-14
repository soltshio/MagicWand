using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

//作成者:杉山
//土の量を変化させる

[System.Serializable]
public class ShifterBigCreatureSoilMaterial
{
    [SerializeField]
    BigCreatureSoilController _bigCreatureSoilController;

    [SerializeField] [Range(0,1)]
    float _deltaRate = 0.17f;

    [SerializeField]
    float _shiftDuration;

    public void AddSoil()
    {
        float newSoilValueRate = _bigCreatureSoilController.CurrentSoilValueRate + _deltaRate;

        newSoilValueRate = Mathf.Clamp01(newSoilValueRate);

        _bigCreatureSoilController.SetSoilValueAsync(newSoilValueRate, _shiftDuration).Forget();
    }

    public void RemoveSoil()
    {
        float newSoilValueRate = _bigCreatureSoilController.CurrentSoilValueRate - _deltaRate;

        newSoilValueRate = Mathf.Clamp01(newSoilValueRate);

        _bigCreatureSoilController.SetSoilValueAsync(newSoilValueRate, _shiftDuration).Forget();
    }

    

    
}
