//作成者:杉山
//マイコンのコマンド

public enum EMiconCommand
{
    //---太陽---
    StartSun,                  //太陽魔法ステートに遷移
    CastSun,                   //詠唱
    InvokeSun,                 //発動
    InvokeSun_PlaySound,       //太陽の効果音を鳴らす
    FinishSun,                 //太陽魔法ステートを終わる

    //---星---
    StartStar,                 //星魔法ステートに遷移
    CastStar,                  //詠唱
    InvokeStar,                //発動
    InvokeStar_PlaySound,      //星の効果音を鳴らす
    FinishStar,                //星魔法ステートを終わる

    //---雷---
    StartThunder,              //雷魔法ステートに遷移
    CastThunder,               //詠唱
    InvokeThunder,             //発動
    InvokeThunder_PlaySound,   //雷の効果音を鳴らす
    FinishThunder,             //雷魔法ステートを終わる

    //---水---
    StartWater,                //水魔法ステートに遷移
    CastWater,                 //詠唱
    InvokeWater,               //発動
    InvokeWater_PlaySound,     //水の効果音を鳴らす
    FinishWater,               //水魔法ステートを終わる

    //---時---
    StartTime,                 //時魔法ステートに遷移
    CastTime,                  //詠唱
    InvokeTime,                //発動
    InvokeTime_PlaySound,      //時の効果音を鳴らす
    FinishTime,                //時魔法ステートを終わる
}
