using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//巨大生物のステージ1の星魔法に対してのリアクション

public class BigCreatureReactionTypeStage1_Star : BigCreatureReactionTypeBase
{
    [Tooltip("でかい生き物の無視(沈黙)演出")] [SerializeField]
    IgnoreReaction _ignoreReaction;

    [Tooltip("でかい生き物の土の量を変更する機能")] [SerializeField]
    ShifterBigCreatureSoilMaterial _shifterBigCreatureSoil;

    public override async UniTask TakeReactionAsync()
    {
        var token = this.GetCancellationTokenOnDestroy();

        _shifterBigCreatureSoil.AddSoil();

        //無視(沈黙)演出
        await _ignoreReaction.TakeIgnoreReactionAsync(token);
    }
}
