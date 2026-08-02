using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//魔法の種類に対する魔法の詠唱をまとめたリスト

public class SpellCastList : MonoBehaviour
{
    [SerializeField]
    SerializableDictionary<EMagic, SpellCast> _spellCastsDictionary;

    public Dictionary<EMagic, SpellCast> SpellCasts { get { return _spellCastsDictionary; } }

    public bool TryGetSpellCast(EMagic keyMagic,out SpellCast spellCast)
    {
        return _spellCastsDictionary.TryGetValue(keyMagic, out spellCast);
    }
}
