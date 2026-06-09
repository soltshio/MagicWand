using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//雷魔法の発動内容

public class MagicContentTypeThunder : MagicContentTypeBase
{
    [SerializeField]
    BigCreature _bigCreature;

    [Tooltip("雷エフェクトプレハブ")] [SerializeField]
    GameObject _thunderEffectPrefab;

    [Tooltip("雷エフェクトを何秒で消すか")] [SerializeField]
    float _thunderEffectLifeDuration;

    [Tooltip("雷エフェクトの発生地点")] [SerializeField]
    Transform _spawnThunderPoint;

    [Tooltip("雷エフェクトの効果音")][SerializeField]
    AudioClip _thunderSE;

    [SerializeField]
    AudioSource _audioSource;

    public override async UniTask ActivateAsync(CancellationToken token)
    {
        //雷エフェクトの発生地点に雷エフェクトを発生させる
        var thunderEffectInstance = Instantiate(_thunderEffectPrefab, _spawnThunderPoint.position, _spawnThunderPoint.rotation);
        Destroy(thunderEffectInstance, _thunderEffectLifeDuration);

        //雷エフェクトの効果音を鳴らす
        _audioSource.PlayOneShot(_thunderSE);

        List<UniTask> runningTasks = new();

        //でか生物に雷魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Thunder));

        await UniTask.WhenAll(runningTasks);
    }
}
