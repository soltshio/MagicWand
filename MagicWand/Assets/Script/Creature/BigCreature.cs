using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

//作成者:杉山
//道をふさぐでかい生物

public class BigCreature : MonoBehaviour
{
    [Tooltip("最大体力(起きるまでに対象の魔法を撃たないといけない回数)")] [SerializeField]
    int _maxHp;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    AudioClip _damageSE;

    [SerializeField]
    AudioClip _zzzSE;

    [SerializeField]
    AudioClip _wakeUpSE;

    int _hp;

    public bool _isWakeUp { get { return _hp <= 0; } }//起きたか

    void Awake()
    {
        _hp = _maxHp;
    }

    public async UniTask TakeMagicAsync(EMagic magic)
    {
        var token = this.GetCancellationTokenOnDestroy();

        //ダメージ処理
        if(magic == EMagic.Water || magic == EMagic.Thunder)
        {
            _hp--;
            //ダメージ音
        }
        else
        {
            //zzz音
        }

        //体力が0になったら起きて道を譲る演出を入れる
        if (!_isWakeUp) return;

        //起きた効果音
        //移動
    }
}
