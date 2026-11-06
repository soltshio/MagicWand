using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

//作成者:杉山
//起動時の音量の初期化マネージャー

public class SoundVolumeInitializeManager : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;

    [SerializeField] SoundConfigs _soundConfigs;

    OptionDataSaveLoadManager _optionDataSaveLoadManagerInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        // OptionDataSaveLoadManagerのインスタンスを取得
        await UniTask.WaitUntil(() => OptionDataSaveLoadManager.Instance != null, cancellationToken: this.GetCancellationTokenOnDestroy());
        _optionDataSaveLoadManagerInstance = OptionDataSaveLoadManager.Instance;

        //音量を設定
        SetAudioVolume();
    }

    void SetAudioVolume()
    {
        for (int i = 0; i < (int)ESoundType.Length; i++)
        {
            ESoundType soundType = (ESoundType)i;
            string soundName = AudioMixerName.SoundName(soundType);

            float volume;

            //音量の設定データがなければ、デフォルトの音量を使用してAudioMixerに反映する(同時にセーブをしておく)
            if (!_optionDataSaveLoadManagerInstance.LoadVolume(soundType, out volume))
            {
                volume = _soundConfigs.DefaultVolume(soundType);
            }

            //音量を範囲内に収める
            volume = Mathf.Clamp(volume, _soundConfigs.MinVolume(soundType), _soundConfigs.MaxVolume(soundType));

            //音量をセーブしておく
            _optionDataSaveLoadManagerInstance.SaveVolume(soundType, volume);

            //AudioMixerに反映する
            _audioMixer.SetFloat(soundName, volume);
        }

    
    }
}
