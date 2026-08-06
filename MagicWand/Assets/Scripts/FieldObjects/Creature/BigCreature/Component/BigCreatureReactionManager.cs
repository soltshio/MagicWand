using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

//作成者:杉山
//道をふさぐでかい生物

public class BigCreatureReactionManager : MonoBehaviour
{
    [SerializeField]
    SerializableDictionary<EMagic, BigCreatureReactionTypeBase> _bigCreatureReactions;

    [Tooltip("でかい生き物の歩行演出")] [SerializeField]
    BigCreatureWalking _bigCreatureWalking;

    [Tooltip("でかい生き物の睡眠演出")] [SerializeField]
    SleepReaction _sleepZZZReaction;

    [Tooltip("巨大生物のステータス")] [SerializeField]
    BigCreatureStatus _bigCreatureStatus;

    public async UniTask TakeMagicAsync(EMagic magic)
    {
        var token = this.GetCancellationTokenOnDestroy();

        if(IsCorrectMagic(magic))//正解の魔法が来た場合にhpを減らす
        {
            _bigCreatureStatus.HP--;
        }


        if(!_bigCreatureReactions.TryGetValue(magic,out var reaction))
        {
            Debug.Log("巨大生物のリアクションの取得に失敗");
            return;
        }

        await reaction.TakeReactionAsync();

        
        if (!_bigCreatureStatus.IsWakeUp)
        {
            //体力が0じゃない間は眠る演出
            await _sleepZZZReaction.TakeSleepReactionAsunc(_bigCreatureStatus.HP,token);
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
