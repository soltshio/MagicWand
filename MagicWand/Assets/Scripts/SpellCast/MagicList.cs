using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//魔法の種類ごとに魔法関係のコンポーネントが付いたオブジェクトをまとめるリスト

public class MagicList : MonoBehaviour
{
    [SerializeField]
    SerializableDictionary<EMagic, GameObject> _magicObjsDic;

    Dictionary<EMagic, ComponentCache> _magicComponentCacheDic = new();

    ComponentsDictionaryCache<EMagic> _componentsEMagicDictionaryCache;

    //指定の番号の魔法球からコンポーネントを取得
    public T GetComponentFromMagic<T>(EMagic magic) where T : Component
    {
        if (!_magicComponentCacheDic.TryGetValue(magic, out var cache)) return null;

        return cache?.GetComponent<T>();
    }

    //全ての魔法からコンポーネントの配列を取得
    public Dictionary<EMagic,T> GetComponentsDictionaryFromMagics<T>() where T : Component
    {
        return _componentsEMagicDictionaryCache.GetOrCreateCache<T>();
    }

    void Awake()
    {
        InitMagicComponentCache();

        _componentsEMagicDictionaryCache = new ComponentsDictionaryCache<EMagic>(_magicComponentCacheDic);
    }

    void InitMagicComponentCache()
    {
        foreach(var magicObj in _magicObjsDic)
        {
            ComponentCache componentCache = new(magicObj.Value);

            _magicComponentCacheDic.Add(magicObj.Key, componentCache);
        }
    }
}
