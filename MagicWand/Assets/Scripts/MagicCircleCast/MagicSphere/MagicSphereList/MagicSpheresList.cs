using UnityEngine;

//作成者:杉山
//魔法陣上の魔法球を一括管理するクラス

public class MagicSpheresList : MonoBehaviour
{
    [Tooltip("12時の方向から時計回りに入れるようにしてください")] [SerializeField]
    GameObject[] _magicSphereObjs;//魔法陣上の球の配列

    ComponentCache[] _magicSphereComponentCaches;

    ComponentsArrayCache _componentsArrayCache;

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
        return _componentsArrayCache.GetOrCreateCache<T>();
    }

    void Awake()
    {
        InitComponentCache();

        _componentsArrayCache = new ComponentsArrayCache(_magicSphereComponentCaches);
    }

    void InitComponentCache()
    {
        _magicSphereComponentCaches = new ComponentCache[_magicSphereObjs.Length];

        for (int i = 0; i < _magicSphereComponentCaches.Length; i++)
        {
            if (_magicSphereObjs[i] == null) continue;

            _magicSphereComponentCaches[i] = new(_magicSphereObjs[i]);
        }
    }
}