using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

//作成者:杉山
//でか生き物の歩行

public class BigCreatureWalking : MonoBehaviour
{
    [SerializeField]
    float _moveDuration=3f;

    [SerializeField]
    Transform _bigCreatureTrs;

    [SerializeField]
    Transform _destination;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    AudioClip _walkSE;

    public async UniTask WalkAsync(CancellationToken ct)
    {
        //歩く効果音
        PlayAudio(_walkSE);

        //移動
        Vector3 beforeMovePos = _bigCreatureTrs.position;

        float elapsed = 0f;

        //進行方向に向かせとく
        Vector3 moveDirection = _destination.position - beforeMovePos;
        Quaternion lookRot = Quaternion.LookRotation(moveDirection, Vector3.up);
        _bigCreatureTrs.rotation = lookRot;

        while (true)
        {
            elapsed += Time.deltaTime;

            float rate = elapsed / _moveDuration;

            _bigCreatureTrs.position = Vector3.Lerp(beforeMovePos, _destination.position, rate);

            if (elapsed >= _moveDuration) break;

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }

        //移動を終了
        _bigCreatureTrs.position = _destination.position;

        _audioSource.Stop();
    }

    void PlayAudio(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
