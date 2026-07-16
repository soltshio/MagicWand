using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

//作成者:杉山
//巨大生物のステージ1の雷魔法に対してのリアクション

public class BigCreatureReactionTypeStage1_Thunder : BigCreatureReactionTypeBase
{
    [Tooltip("でかい生き物の驚き演出")] [SerializeField]
    SurpriseReaction _surpriseReaction;

    [Tooltip("でかい生き物の土の量を変更する機能")] [SerializeField]
    ShifterBigCreatureSoilMaterial _shifterBigCreatureSoil;

    [SerializeField] [Tooltip("感電リアクション時間")]
    float _waitElectricShockDuration = 0.5f;

    [SerializeField]
    SkinnedMeshRenderer _bigCreatureBodyMeshRenderer;

    [SerializeField]
    Material _electricShockMat;

    public override async UniTask TakeReactionAsync()
    {
        var token = this.GetCancellationTokenOnDestroy();

        //巨大生物の感電リアクション
        //await TakeElectricShockAsync(token);

        _shifterBigCreatureSoil.RemoveSoil();

        //驚き演出
        await _surpriseReaction.TakeSurpriseReactionAsync(token);
    }

    async UniTask TakeElectricShockAsync(CancellationToken ct)
    {
        var defaultMat = _bigCreatureBodyMeshRenderer.material;
        _bigCreatureBodyMeshRenderer.material = _electricShockMat;

        //少し待つ
        await UniTask.Delay(TimeSpan.FromSeconds(_waitElectricShockDuration), cancellationToken: ct);

        _bigCreatureBodyMeshRenderer.material = defaultMat;
    }
}
