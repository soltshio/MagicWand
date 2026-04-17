using UnityEngine;

//作成者:杉山
//雷魔法の発動内容

public class MagicContentTypeThunder : MagicContentTypeBase
{
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

    public override void Activate()
    {
        //雷エフェクトの発生地点に雷エフェクトを発生させる
        var thunderEffectInstance = Instantiate(_thunderEffectPrefab, _spawnThunderPoint.position, _spawnThunderPoint.rotation);
        Destroy(thunderEffectInstance, _thunderEffectLifeDuration);

        //雷エフェクトの効果音を鳴らす
        _audioSource.PlayOneShot(_thunderSE);
    }
}
