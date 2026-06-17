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

        SwitchToLookUpCamera();

        List<UniTask> runningTasks = new();

        //でか生物に魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Star));

        await UniTask.WhenAll(runningTasks);

        SwitchToDefaultCamera();

        _starAudioSource.Stop();

        _shootingStarParticle.Stop();
    }

    //カメラを見上げる視点に
    void SwitchToLookUpCamera()
    {
        _defaultCamera.enabled = false;
        _lookUpCamera.enabled = true;
    }

    //カメラを元に戻す
    void SwitchToDefaultCamera()
    {
        _defaultCamera.enabled = true;
        _lookUpCamera.enabled = false;
    }
}
