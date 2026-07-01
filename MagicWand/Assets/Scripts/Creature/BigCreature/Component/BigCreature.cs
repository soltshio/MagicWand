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
    AudioSource _audioSource;

    [SerializeField]
    AudioClip _damageSE;

    [Tooltip("ダメージのイベントの時間")] [SerializeField]
    float _damageEventDuration=1f;

    [Tooltip("ノーダメージ(睡眠)のイベントの時間")] [SerializeField]
    float _sleepEventDuration = 1f;

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
        _sleepZZZReaction.Start();
        _shifterBigCreatureSoil.Start();
    }

    public async UniTask TakeMagicAsync(EMagic magic)
    {
        var token = this.GetCancellationTokenOnDestroy();

        if(magic == EMagic.Rain || magic == EMagic.Thunder)//正解の魔法が来た場合
        {
            _hp--;

            //ダメージ音
            PlayAudio(_damageSE);

            _shifterBigCreatureSoil.RemoveSoil(this.GetCancellationTokenOnDestroy());

            await UniTask.Delay(TimeSpan.FromSeconds(_damageEventDuration), cancellationToken: token);
        }
        else//不正解の魔法が来た場合
        {
            _shifterBigCreatureSoil.AddSoil(this.GetCancellationTokenOnDestroy());

            await UniTask.Delay(TimeSpan.FromSeconds(_sleepEventDuration), cancellationToken: token);
        }

        _audioSource.Stop();

        
        if (!_isWakeUp)
        {
            //体力が0じゃない間は眠る演出
            await _sleepZZZReaction.SleepReactionAsunc(_hp,token);
        }
        else
        {
            //体力が0になったら起きて道を譲る演出を入れる
            await _bigCreatureWalking.WalkAsync(token);
        }
    }

    void PlayAudio(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
