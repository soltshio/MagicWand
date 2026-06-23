using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//星魔法の内容

public class MagicContentTypeStar : MagicContentTypeBase
{
    [SerializeField]
    BigCreature _bigCreature;

    [SerializeField]
    AudioSource _starAudioSource;

    [Tooltip("魔法の影響を与えるまでに遅らせる時間")] [SerializeField]
    float _delayDurationAffection = 0.8f;

    [Tooltip("流れ星のエフェクト")] [SerializeField]
    ParticleSystem _shootingStarParticle;

    [Tooltip("通常時のCInemachineカメラ")] [SerializeField]
    CinemachineCamera _defaultCamera;

    [Tooltip("見上げる視点のCinemachineカメラ")] [SerializeField]
    CinemachineCamera _lookUpCamera;

    public override async UniTask ActivateAsync(CancellationToken token)
    {
        _shootingStarParticle.Play();

        _starAudioSource.Play();

        SwitchActiveCamera(true);

        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationAffection), cancellationToken: token);

        SwitchActiveCamera(false);

        _starAudioSource.Stop();

        List<UniTask> runningTasks = new();

        //でか生物に魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Star));

        await UniTask.WhenAll(runningTasks);

        _shootingStarParticle.Stop();
    }

    //カメラを切り替える
    void SwitchActiveCamera(bool isToLookUp)
    {
        _defaultCamera.enabled = !isToLookUp;
        _lookUpCamera.enabled = isToLookUp;
    }
}
