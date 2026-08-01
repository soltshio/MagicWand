using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//いずれかの魔法球に振れた際に、どの魔法球に触れたかを判定するクラス

public class MagicSphereTouchChecker
{
    MagicSpheresList _magicSpheresList;

    public MagicSphereTouchChecker(MagicSpheresList magicSpheresList)
    {
        _magicSpheresList = magicSpheresList;
    }

    //いずれかの球に杖がタッチしたか。
    //touchedMagicSphereindexは触れた魔法球のインデックスを返す。いずれの球にも触れていない場合は-1を返す。
    public bool IsTouchedAnyMagicSphere(List<int> activeMagicSphereIndexList, out int touchedMagicSphereindex)
    {
        touchedMagicSphereindex = -1;

        foreach (var i in activeMagicSphereIndexList)
        {
            if (!MathfExtension.IsInRange(i, 0, _magicSpheresList.MagicSphereObjects.Length - 1)) continue;

            var magicSphere = _magicSpheresList.GetComponentFromMagicSphere<MagicSphereVer3>(i);

            if (magicSphere == null) continue;

            if (!magicSphere.IsActive)
            {
                touchedMagicSphereindex = i;
                return true;
            }
        }

        return false;
    }
}
