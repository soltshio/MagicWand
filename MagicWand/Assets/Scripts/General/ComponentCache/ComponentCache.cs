using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//オブジェクトから取得したコンポーネントをキャッシュするクラス(オブジェクト1つにつき1種類のComponentしかアタッチされない前提)

public class ComponentCache
{
    GameObject _gameObj;
    Dictionary<System.Type, Component> _cache = new();//キャッシュ

    public ComponentCache(GameObject gameObj)
    {
        _gameObj = gameObj;
    }

    //オブジェクトからコンポーネントを取得(既に取得したことがあるならキャッシュから返す)
    public T GetComponent<T>() where T : Component
    {
        System.Type type = typeof(T);//型を取り出す

        //キャッシュから同じ型を探し、なかったら普通にGetComponent
        if (!_cache.TryGetValue(type, out Component ret))
        {
            ret = _gameObj.GetComponent<T>();

            //nullじゃなければキャッシュに登録
            if (ret != null)
            {
                _cache[type] = ret;
            }
        }

        return ret as T;
    }
}
