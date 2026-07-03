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

    [Tooltip("でかい生き物の無視(沈黙)演出")] [SerializeField]
    IgnoreReaction _ignoreReaction;

    [Tooltip("でかい生き物の驚き演出")] [SerializeField]
    SurpriseReaction _surpriseReaction;

    [Tooltip("でかい生き物の歩行演出")] [SerializeField]
    BigCreatureWalking _bigCreatureWalking;

    [Tooltip("でかい生き物の睡眠演出")] [SerializeField]
    SleepReaction _sleepZZZReaction;

    [Tooltip("でかい生き物の土の量を変更する機能")] [SerializeField]
    ShifterBigCreatureSoilMaterial _shifterBigCreatureSoil;

    int _hp;

    public bool _isWakeUp { get { return _hp <= 0; } }//起きたか

    void Awake()
    {
        _hp = _maxHp;
    }

    void Start()
    {
        _ignoreReaction.Start();
        _surpriseReaction.Start();
        _sleepZZZReaction.Start();
    }

    public async UniTask TakeMagicAsync(EMagic magic)
    {
        var token = this.GetCancellationTokenOnDestroy();

        if(IsCorrectMagic(magic))//正解の魔法が来た場合
        {
            _hp--;

            _shifterBigCreatureSoil.RemoveSoil();

            //驚き演出
            await _surpriseReaction.TakeSurpriseReactionAsync(token);
        }
        else//不正解の魔法が来た場合
        {
            _shifterBigCreatureSoil.AddSoil();

            //無視(沈黙)演出
            await _ignoreReaction.TakeIgnoreReactionAsync(token);
        }

        
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
