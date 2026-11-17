//作成者:杉山
//魔法球のアニメーションのプロパティ

using System.Collections.Generic;

public class MagicSphereAnimatorProperty
{
    //魔法ごとのトリガー
    static readonly Dictionary<EMagic, string> _magicTriggerNameDic = new Dictionary<EMagic, string>
    {
        { EMagic.Sun,       "Sun"    },//太陽
        { EMagic.Star,      "Star"   },//星
        { EMagic.Thunder,   "Thunder"},//雷
        { EMagic.Rain,      "Water"  },//水
        { EMagic.Time,      "Time"   },//時間
    };

    public static bool TryGetMagicTriggerName(EMagic magic,out string triggerName)
    {
        if (!_magicTriggerNameDic.TryGetValue(magic,out triggerName)) return false;

        return true;
    }

    //隠すトリガー
    public static string HideTriggerName = "Hide";

    //通常状態トリガー
    public static string DefaultTriggerName = "Default";
}
