using System;
using System.Collections.Generic;
using UnityEngine;

//コンポーネントを辞書としてキャッシュする機能

public class ComponentsDictionaryCache<TKey> where TKey : Enum
{
    readonly Dictionary<TKey, ComponentCache> _componentCacheDic;
    readonly Dictionary<Type, object> _cache = new();

    public ComponentsDictionaryCache(Dictionary<TKey, ComponentCache> componentCacheDic)
    {
        _componentCacheDic = componentCacheDic;
    }

    public Dictionary<TKey, TValue> GetOrCreateCache<TValue>() where TValue : Component
    {
        var valueType = typeof(TValue);

        // 既にキャッシュがある場合はそれを返す
        if (_cache.TryGetValue(valueType, out var cached))
        {
            return (Dictionary<TKey, TValue>)cached;
        }

        // 新しくDictionaryを作成
        var componentDictionary = new Dictionary<TKey, TValue>();

        foreach (var pair in _componentCacheDic)
        {
            var component = pair.Value?.GetComponent<TValue>();

            if (component != null)
            {
                componentDictionary.Add(pair.Key, component);
            }
        }

        // キャッシュに追加
        _cache.Add(valueType, componentDictionary);

        return componentDictionary;
    }
}