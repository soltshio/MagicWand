using Cysharp.Threading.Tasks;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//作成者:杉山
//音量スライダー

public class SoundVolumeSlider : MonoBehaviour
{
    [Tooltip("音のボリュームを何段階で変えられるようにするか")] [SerializeField]
    int _valueStep = 10;

    [SerializeField]
    ESoundType _soundType;

    [SerializeField]
    Slider _slider;

    [SerializeField]
    AudioMixer _audioMixer;

    [SerializeField]
    SoundConfigs _soundConfigs;

    [SerializeField]
    AudioSource _audioSource;

    [Tooltip("音量を変えた時に流す効果音")] [SerializeField]
    AudioClip _soundEffectOnValueChange;

    OptionDataSaveLoadManager _optionDataSaveLoadManagerInstance;

    private void OnValidate()
    {
        if (_slider == null) return;

        _slider.wholeNumbers = true;
        _slider.maxValue = _valueStep;
        _slider.minValue = 0;
    }

    async void Start()
    {
        // OptionDataSaveLoadManagerのインスタンスを取得
        await UniTask.WaitUntil(() => OptionDataSaveLoadManager.Instance != null, cancellationToken: this.GetCancellationTokenOnDestroy());
        _optionDataSaveLoadManagerInstance = OptionDataSaveLoadManager.Instance;

        InitializeSlider();
    }

    public void OnValueChanged(float newVolumeLevel)
    {
        SetVolume(newVolumeLevel);
    }

    void InitializeSlider()
    {
        //音量の設定データがなければ、デフォルトの音量を使用する
        if (!_optionDataSaveLoadManagerInstance.LoadVolume(_soundType, out float volume))
        {
            volume = _soundConfigs.DefaultVolume(_soundType);
        }

        float maxVolume = _soundConfigs.MaxVolume(_soundType);

        float minVolume = _soundConfigs.MinVolume(_soundType);

        float volumeRate = Mathf.InverseLerp(minVolume, maxVolume, volume);

        //スライダーの設定の初期化
        _slider.value = volumeRate * _valueStep;
    }

    void SetVolume(float newVolumeLevel)
    {
        if(_optionDataSaveLoadManagerInstance == null) return;

        //スライダーから受け取った値をボリュームの値に変換
        float maxVolume = _soundConfigs.MaxVolume(_soundType);
        float minVolume = _soundConfigs.MinVolume(_soundType);
        float volume = MathfExtension.Remap(newVolumeLevel, 0, _valueStep, minVolume, maxVolume);

        //AudioMixerに反映する
        string soundName = AudioMixerName.SoundName(_soundType);
        _audioMixer.SetFloat(soundName, volume);

        //セーブデータに現在の音量をセーブしておく
        _optionDataSaveLoadManagerInstance.SaveVolume(_soundType, volume);

        //確認用に効果音を流しておく
        _audioSource.PlayOneShot(_soundEffectOnValueChange);
    }
}
