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

    [Tooltip("流れ星のエフェクト")] [SerializeField]
    ParticleSystem _shootingStarParticle;

    [Tooltip("通常時のCInemachineカメラ")] [SerializeField]
    CinemachineCamera _defaultCamera;

    [Tooltip("見上げる視点のCinemachineカメラ")] [SerializeField]
    CinemachineCamera _lookUpCamera;

    public override async UniTask ActivateAsync(CancellationToken token)
    {
        //流れ星エフェクトを表示
        _shootingStarParticle.Play();

        //カメラを見上げる視点に
        SwitchToLookUpCamera();

        List<UniTask> runningTasks = new();

        //でか生物に魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Star));

        await UniTask.WhenAll(runningTasks);

        //カメラを元に戻す
        SwitchToDefaultCamera();

        //流れ星エフェクトを停止
        _shootingStarParticle.Stop();
    }

    void SwitchToLookUpCamera()
    {
        _defaultCamera.enabled = false;
        _lookUpCamera.enabled = true;
    }

    void SwitchToDefaultCamera()
    {
        _defaultCamera.enabled = true;
        _lookUpCamera.enabled = false;
    }
}
