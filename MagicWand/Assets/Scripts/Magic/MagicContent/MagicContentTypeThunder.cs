using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Cinemachine;
using UnityEngine;

//作成者:杉山
//雷魔法の発動内容

public class MagicContentTypeThunder : MagicContentTypeBase
{
    [SerializeField]
    BigCreature _bigCreature;

    [Tooltip("雷雲の演出")] [SerializeField]
    ThunderCloudEvent _thunderCloudEvent;

    [Tooltip("目の前に落ちる雷の演出")] [SerializeField]
    DirectThunderEvent _directThunderEvent;

    [Tooltip("通常時のカメラ")] [SerializeField]
    CinemachineCamera _defaultCamera;

    [Tooltip("見上げるカメラ")] [SerializeField]
    CinemachineCamera _lookUpCamera;

    [Tooltip("カメラを動かしてから、雷雲を発生させるまでに遅らせる時間")] [SerializeField]
    float _delayDurationFromMoveCameraToThunderCloud = 0.5f;

    [Tooltip("雷雲を発生させてから雷を落とすまでに遅らせる時間")] [SerializeField]
    float _delayDurationFromThunderCloudToDirectThunder=1f;

    [Tooltip("雷を落としてから魔法の影響を与えるまでに遅らせる時間")][SerializeField]
    float _delayDurationAffection = 0.8f;

    [SerializeField]
    AudioSource _audioSource;

    public override async UniTask ActivateAsync(CancellationToken token)
    {
        //見上げる視点のカメラにする
        SwitchActiveCamera(true);

        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationFromMoveCameraToThunderCloud), cancellationToken: token);

        //雷雲発生
        _thunderCloudEvent.CauseThunderCloud();

        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationFromThunderCloudToDirectThunder), cancellationToken: token);

        //雷雲が出て少し待ってから目の前に雷を落とす
        _directThunderEvent.CauseDirectThunder();

        List<UniTask> runningTasks = new();

        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationAffection), cancellationToken: token);

        //カメラを元に戻す
        SwitchActiveCamera(false);

        //ここから魔法の影響を近くの物に与える

        //でか生物に魔法を当てる
        runningTasks.Add(_bigCreature.TakeMagicAsync(EMagic.Thunder));

        await UniTask.WhenAll(runningTasks);
    }

    //カメラを切り替える
    void SwitchActiveCamera(bool isToLookUp)
    {
        _defaultCamera.enabled = !isToLookUp;
        _lookUpCamera.enabled = isToLookUp;
    }
}
