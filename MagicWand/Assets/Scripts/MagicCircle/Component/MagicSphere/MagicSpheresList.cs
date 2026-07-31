using System;
using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//魔法陣上の魔法球を一括管理するクラス

public class MagicSpheresList : MonoBehaviour
{
    [Tooltip("12時の方向から時計回りに入れるようにしてください")] [SerializeField]
    GameObject[] _magicSphereObjs;//魔法陣上の球の配列

    ComponentCache[] _magicSphereComponentCaches;

    Dictionary<Type, Array> _componentsArrayCache = new();//配列のコンポーネントのキャッシュ

    public GameObject[] MagicSphereObjects { get { return _magicSphereObjs; } }

    //指定の番号の魔法球からコンポーネントを取得
    public T GetComponentFromMagicSphere<T>(int num) where T : Component
    {
        if(!MathfExtension.IsInRange(num,0,_magicSphereComponentCaches.Length-1)) return null;

        var magicSphereComponentCache = _magicSphereComponentCaches[num];

        if (magicSphereComponentCache == null) return null;

        return magicSphereComponentCache.GetComponent<T>();
    }

    //全ての魔法球からコンポーネントの配列を取得
    public T[] GetComponentsArrayFromMagicSpheres<T>() where T : Component
    {
        var type = typeof(T);

        if (!_componentsArrayCache.TryGetValue(type, out var retComponentsArray))
        {
            var ret = new T[_magicSphereComponentCaches.Length];

            for (int i = 0; i < _magicSphereComponentCaches.Length; i++)
            {
                var cache = _magicSphereComponentCaches[i];
                ret[i] = cache != null ? cache.GetComponent<T>() : null;
            }

            _componentsArrayCache.Add(type, ret);
            return ret;
        }

        return (T[])retComponentsArray;
    }

    void Awake()
    {
        _magicSphereComponentCaches = new ComponentCache[_magicSphereObjs.Length];

        for(int i=0; i<_magicSphereComponentCaches.Length ;i++)
        {
            if (_magicSphereObjs[i] == null) continue;

            _magicSphereComponentCaches[i] = new(_magicSphereObjs[i]);
        }
    }
}