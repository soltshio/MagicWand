using System.Collections.Generic;
using System.Linq;

//作成者:杉山
//発動可能な魔法を管理するクラス

public class CastableMagics
{
    Dictionary<EMagic, SpellCast> _castableMagicDic;

    public CastableMagics(Dictionary<EMagic, SpellCast> spellCastsDictionary)
    {
        _castableMagicDic = new Dictionary<EMagic, SpellCast>(spellCastsDictionary);
    }

    //発動可能性のある魔法から、次になぞるべき球をアクティブにする
    //アクティブにした球のインデックスリストを返す
    public List<int> ActivateNextTraceMagicSphere(MagicSpheresList magicSpheresList)
    {
        List<int> activeMagicSphereIndexList = new();

        foreach (var spellCastPair in _castableMagicDic)
        {
            int nextIndex = spellCastPair.Value.NextMagicSphereIndex;

            if (nextIndex == -1) continue;

            magicSpheresList[nextIndex].ToActive(spellCastPair.Value.MagicSphereMaterial);
            activeMagicSphereIndexList.Add(nextIndex);
        }

        return activeMagicSphereIndexList;
    }

    //杖が触れた球のインデックスを魔法に伝える(それにより次になぞる球の番号の更新、魔法の発動処理を行う)
    //発動可能な魔法を伝える
    public EMagic[] CastTouchedIndexToMagics(int touchedMagicSphereindex)
    {
        List<EMagic> invokableMagicsList = new();

        foreach (var spellCastPair in _castableMagicDic)
        {
            spellCastPair.Value.Cast(touchedMagicSphereindex);//触れた球のインデックスを魔法に伝える

            if (spellCastPair.Value.IsReadyToInvoke)
            {
                invokableMagicsList.Add(spellCastPair.Key);
            }
        }

        return invokableMagicsList.ToArray();
    }

    //発動可能性の無い魔法を発動可能性のある魔法リストから消す
    public void RemoveIncastableMagic()
    {
        //発動可能性のない魔法のキーを取得
        var incastableMagicKeyList = _castableMagicDic.Where(spellCast => !spellCast.Value.SpellIsValid).Select(spellCast => spellCast.Key).ToList();

        foreach (var incastableKey in incastableMagicKeyList)
        {
            _castableMagicDic.Remove(incastableKey);
        }
    }
}
