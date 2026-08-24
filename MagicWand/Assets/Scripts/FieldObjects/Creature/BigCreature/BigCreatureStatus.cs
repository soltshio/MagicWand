using System;
using UnityEngine;

//作成者:杉山
//巨大生物の状態

public class BigCreatureStatus : MonoBehaviour
{
    [Tooltip("最大体力(起きるまでに対象の魔法を撃たないといけない回数)")] [SerializeField]
    int _maxHp=3;

    public event Action<int> OnUpdateHP;//残り体力が更新された際のコールバック、引数には更新後の残り体力が入る

    int _hp;

    public int HP
    {
        get
        {
            return _hp;
        }
        set
        {
            if (value == _hp) return;

            _hp = value;
            OnUpdateHP?.Invoke(_hp);
        }
    }

    public bool IsWakeUp { get { return _hp <= 0; } }//起きたか

    void Awake()
    {
        _hp = _maxHp;
    }
}
