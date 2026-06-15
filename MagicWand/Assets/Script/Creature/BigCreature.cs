using Cysharp.Threading.Tasks;
using System;
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
            PlayAudio(_damageSE);

            await UniTask.Delay(TimeSpan.FromSeconds(_damageEventDuration), cancellationToken: token);
        }
        else
        {
            //zzz音
            PlayAudio(_zzzSE);

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
