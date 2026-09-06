using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//魔法の種類ごとに魔法関係のコンポーネントが付いたオブジェクトをまとめるリスト

public class MagicList : MonoBehaviour
{
    [SerializeField]
    SerializableDictionary<EMagic, GameObject> _magicObjsDic;

    [SerializeField]
    SerializableDictionary<EMagic, SpellCast> _spellCastsDictionary;

    public Dictionary<EMagic, SpellCast> SpellCasts { get { return _spellCastsDictionary; } }

    Dictionary<EMagic, ComponentCache> _magicComponentCacheDic;

    public bool TryGetSpellCast(EMagic keyMagic,out SpellCast spellCast)
    {
        return _spellCastsDictionary.TryGetValue(keyMagic, out spellCast);
    }

    void Awake()
    {

    }

    void InitMagicComponentCache()
    {

    }
}
