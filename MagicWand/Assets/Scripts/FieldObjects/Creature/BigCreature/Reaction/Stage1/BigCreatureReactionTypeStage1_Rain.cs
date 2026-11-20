using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

//作成者:杉山
//巨大生物のステージ1の雨魔法に対してのリアクション

public class BigCreatureReactionTypeStage1_Rain : BigCreatureReactionTypeBase
{
    [SerializeField]
    PlayableDirector _rainReactionDirecter;

    [Tooltip("でかい生き物の土の量を変更する機能")] [SerializeField]
    ShifterBigCreatureSoilMaterial _shifterBigCreatureSoil;

    [SerializeField]
    AnimatorReference _bigCreatureAnimator;

    //巨大生物に二度寝のモーションをさせる
    public void PlayBigCreatureGoBackToSleepMotion()
    {
        _bigCreatureAnimator.Animator.SetBool(BigCreatureAnimatorProperty.GoBackToSleepBoolName, true);
    }

    //巨大生物の二度寝のモーションを止める
    public void StopBigCreatureGoBackToSleepMotion()
    {
        _bigCreatureAnimator.Animator.SetBool(BigCreatureAnimatorProperty.GoBackToSleepBoolName, true);
    }

    public void RemoveBigCreatureSoil()
    {
        _shifterBigCreatureSoil.RemoveSoil();
    }

    public override async UniTask TakeReactionAsync()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _rainReactionDirecter.Play();

        await _rainReactionDirecter.WaitForStoppedAsync(ct);
    }
}
