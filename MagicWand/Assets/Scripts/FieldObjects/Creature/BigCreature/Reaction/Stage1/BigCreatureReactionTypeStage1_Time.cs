using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

//作成者:杉山
//巨大生物のステージ1の時間魔法に対してのリアクション

public class BigCreatureReactionTypeStage1_Time : BigCreatureReactionTypeBase
{
    [SerializeField]
    PlayableDirector _timeReactionDirecter;

    [Tooltip("でかい生き物の土の量を変更する機能")] [SerializeField]
    ShifterBigCreatureSoilMaterial _shifterBigCreatureSoil;

    public void AddBigCreatureSoil()
    {
        _shifterBigCreatureSoil.AddSoil();
    }

    public override async UniTask TakeReactionAsync()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _timeReactionDirecter.Play();

        await _timeReactionDirecter.WaitForStoppedAsync(ct);
    }
}
