using Cysharp.Threading.Tasks;
using System;
using Unity.Cinemachine;
using UnityEngine;

//作成者:杉山
//雷雲が発生して雷が落ちるまでの演出

public class ThunderEventPlayer : MonoBehaviour
{
    [Header("エフェクト関係")]

    [SerializeField]
    ParticleSystem _thunderEffect;

    [SerializeField]
    CloudFadeController _cloudFadeController;


    [Header("効果音関係")]

    [SerializeField]
    AudioSource _audioSource;

    [Tooltip("遠雷の効果音")] [SerializeField]
    AudioClip _distantThunderSE;

    [Tooltip("目の前に落ちる雷の効果音")] [SerializeField]
    AudioClip _directThunderSE;


    [Header("カメラ関係")]

    [Tooltip("通常時のカメラ")] [SerializeField]
    CinemachineCamera _defaultCamera;

    [Tooltip("見上げるカメラ")] [SerializeField]
    CinemachineCamera _lookUpCamera;
    
    [Tooltip("カメラの揺れ")] [SerializeField]
    CinemachineImpulseSource _impulseSource;


    [Header("遅延時間関係")]

    [Tooltip("見上げる視点のカメラに切り替え始めてから、何秒で遠雷と雷雲を発生させ始めるか")] [SerializeField]
    float _delayDurationFromLookUpCameraToCauseThunderCloud=1.5f;

    [Tooltip("雷エフェクトが発生してから、何秒で地面に落ちた時の演出(効果音・画面の揺れ)を入れるか")] [SerializeField]
    float _delayDurationFromEffectPlayToLightningStrike=0.5f;

    [Tooltip("地面に落ちた時の演出(効果音・画面の揺れ)が入ってから、何秒で元の状態に戻そうとするか")] [SerializeField]
    float _delayDurationFromLightningStrikeToDefault = 1f;

    public async UniTask CauseThunderEventAsync()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        //見上げる視点のカメラに切り替える
        SwitchActiveCamera(true);

        //カメラが完全に切り替わるのを待ってから、遠雷と雷雲を発生させる
        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationFromLookUpCameraToCauseThunderCloud), cancellationToken: ct);

        _audioSource.PlayOneShot(_distantThunderSE);

        await _cloudFadeController.ActivateAsync();

        //雷雲が発生しきってから雷エフェクトを落とす
        _thunderEffect.Play();

        //少し遅れて、雷が地面に落ちた時の演出を入れる
        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationFromEffectPlayToLightningStrike), cancellationToken: ct);

        _audioSource.PlayOneShot(_directThunderSE);
        _impulseSource.GenerateImpulse();

        //雷が落ちた音と画面の揺れが収まってから、カメラを元に戻し始め、雷雲を元に戻す
        await UniTask.Delay(TimeSpan.FromSeconds(_delayDurationFromLightningStrikeToDefault), cancellationToken: ct);

        SwitchActiveCamera(false);
        _cloudFadeController.DeactivateAsync().Forget();
    }

    //カメラを切り替える
    void SwitchActiveCamera(bool isToLookUp)
    {
        _defaultCamera.enabled = !isToLookUp;
        _lookUpCamera.enabled = isToLookUp;
    }
}
