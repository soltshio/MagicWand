//作成者:杉山
//マイコンのコマンド

public enum EMiconCommand
{
    //---太陽---
    ChangeToSun,//太陽魔法ステートに遷移
    PlaySunSound,//太陽の効果音を鳴らす
    FinishSun,//太陽魔法ステートを終わる

    //---星---
    ChangeToStar,//星魔法ステートに遷移
    PlayStarSound,//星の効果音を鳴らす
    FinishStar,//星魔法ステートを終わる

    //---雷---
    ChangeToThunder,//雷魔法ステートに遷移
    PlayThunderSound,//雷の効果音を鳴らす
    FinishThunder,//雷魔法ステートを終わる

    //---水---
    ChangeToWater,//水魔法ステートに遷移
    PlayWaterSound,//水の効果音を鳴らす
    FinishWater,//水魔法ステートを終わる

    //---時---
    ChangeToTime,//時魔法ステートに遷移
    PlayTimeSound,//時の効果音を鳴らす
    FinishTime,//時魔法ステートを終わる
}
