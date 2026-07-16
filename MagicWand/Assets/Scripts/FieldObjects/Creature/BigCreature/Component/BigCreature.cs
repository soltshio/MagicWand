using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

//作成者:杉山
//道をふさぐでかい生物

public class BigCreature : MonoBehaviour
{
    [Tooltip("最大体力(起きるまでに対象の魔法を撃たないといけない回数)")] [SerializeField]
    int _maxHp;

    [SerializeField]
    SerializableDictionary<EMagic, BigCreatureReactionTypeBase> _bigCreatureReactions;

    [Tooltip("でかい生き物の歩行演出")] [SerializeField]
    BigCreatureWalking _bigCreatureWalking;

    [Tooltip("でかい生き物の睡眠演出")] [SerializeField]
    SleepReaction _sleepZZZReaction;

    int _hp;

    public bool _isWakeUp { get { return _hp <= 0; } }//起きたか

    void Awake()
    {
        _hp = _maxHp;
    }

    public async UniTask TakeMagicAsync(EMagic magic)
    {
        var token = this.GetCancellationTokenOnDestroy();

        if(IsCorrectMagic(magic))//正解の魔法が来た場合にhpを減らす
        {
            _hp--;
        }


        if(!_bigCreatureReactions.TryGetValue(magic,out var reaction))
        {
            Debug.Log("巨大生物のリアクションの取得に失敗");
            return;
        }

        await reaction.TakeReactionAsync();

        
        if (!_isWakeUp)
        {
            //体力が0じゃない間は眠る演出
            await _sleepZZZReaction.TakeSleepReactionAsunc(_hp,token);
        }
        else
        {
            //体力が0になったら起きて道を譲る演出を入れる
            await _bigCreatureWalking.WalkAsync(token);
        }
    }

    bool IsCorrectMagic(EMagic magic)
    {
        return magic == EMagic.Rain || magic == EMagic.Thunder;
    }
}
