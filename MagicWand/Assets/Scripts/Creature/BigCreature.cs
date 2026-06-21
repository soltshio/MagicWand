using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//道をふさぐでかい生物

public class BigCreature : MonoBehaviour
{
    [Tooltip("最大体力(起きるまでに対象の魔法を撃たないといけない回数)")] [SerializeField]
    int _maxHp;

    [SerializeField]
    float _moveDuration;

    [SerializeField]
    Transform _destination;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    AudioClip _damageSE;

    [Tooltip("ダメージのイベントの時間")] [SerializeField]
    float _damageEventDuration=1f;

    [SerializeField]
    AudioClip _zzzSE;

    [Tooltip("ノーダメージ(睡眠)のイベントの時間")] [SerializeField]
    float _sleepEventDuration = 1f;

    [SerializeField]
    AudioClip _walkSE;

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
            //zzz音
            PlayAudio(_zzzSE);

            _shifterBigCreatureSoil.AddSoil(this.GetCancellationTokenOnDestroy());

            await UniTask.Delay(TimeSpan.FromSeconds(_sleepEventDuration), cancellationToken: token);
        }

        _audioSource.Stop();

        //体力が0になったら起きて道を譲る演出を入れる
        if (!_isWakeUp) return;

        //歩く効果音
        PlayAudio(_walkSE);

        //移動
        Vector3 beforeMovePos = transform.position;

        float elapsed = 0f;

        //進行方向に向かせとく
        Vector3 moveDirection = _destination.position - beforeMovePos;
        Quaternion lookRot = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = lookRot;

        while(true)
        {
            elapsed += Time.deltaTime;

            float rate = elapsed / _moveDuration;

            transform.position = Vector3.Lerp(beforeMovePos, _destination.position, rate);

            if (elapsed >= _moveDuration) break;

            await UniTask.Yield(PlayerLoopTiming.Update,cancellationToken:token);
        }

        //移動を終了
        transform.position = _destination.position;
        _audioSource.Stop();
    }

    void PlayAudio(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
