using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//巨大生物のステージ1の雨魔法に対してのリアクション

public class BigCreatureReactionTypeStage1_Rain : BigCreatureReactionTypeBase
{
    [Tooltip("でかい生き物の驚き演出")] [SerializeField]
    SurpriseReaction _surpriseReaction;

    [Tooltip("でかい生き物の土の量を変更する機能")] [SerializeField]
    ShifterBigCreatureSoilMaterial _shifterBigCreatureSoil;

    public override async UniTask TakeReactionAsync()
    {
        var token = this.GetCancellationTokenOnDestroy();

        _shifterBigCreatureSoil.RemoveSoil();

        //驚き演出
        await _surpriseReaction.TakeSurpriseReactionAsync(token);
    }
}
