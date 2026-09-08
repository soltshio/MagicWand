using System;
using System.Collections.Generic;
using UnityEngine;

//コンポーネントを配列としてキャッシュする機能

public class ComponentsArrayCache
{
    readonly ComponentCache[] _componentCaches;
    readonly Dictionary<Type, Array> _cache = new();

    public ComponentsArrayCache(ComponentCache[] componentCaches)
    {
        _componentCaches = componentCaches;
    }

    public T[] GetOrCreateCache<T>() where T : Component
    {
        var type = typeof(T);

        //既にキャッシュがある場合はそれを返す
        if (_cache.TryGetValue(type, out var cached))
        {
            return (T[])cached;
        }

        //ない場合は新しく作成してキャッシュに追加する
        var components = new T[_componentCaches.Length];

        for (int i = 0; i < _componentCaches.Length; i++)
        {
            components[i] = _componentCaches[i]?.GetComponent<T>();
        }

        _cache.Add(type, components);

        return components;
    }
}