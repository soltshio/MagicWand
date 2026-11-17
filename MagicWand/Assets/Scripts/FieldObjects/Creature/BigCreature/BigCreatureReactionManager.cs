using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

//作成者:杉山
//巨大生物の魔法ごとのリアクション演出を行う

public class BigCreatureReactionManager : MonoBehaviour
{
    [SerializeField]
    SerializableDictionary<EMagic, BigCreatureReactionTypeBase> _bigCreatureReactions;

    public async UniTask TakeMagicAsync(EMagic magic)
    {
        if(!_bigCreatureReactions.TryGetValue(magic,out var reaction))
        {
            Debug.Log("巨大生物のリアクションの取得に失敗");
            return;
        }

        await reaction.TakeReactionAsync();
    }
}
