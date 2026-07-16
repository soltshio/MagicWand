using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

//作成者:杉山
//無視(沈黙)した反応の演出

public class IgnoreReaction : MonoBehaviour
{
    [Tooltip("。をテキストに追加していくインターバル")] [SerializeField]
    float _intervalAddPoint = 0.9f;

    [Tooltip("全ての。を表示してからテキストそのものを非表示にするまで待つ時間")] [SerializeField]
    float _waitDurationFromAllShowToAllHide = 0.2f;

    [SerializeField]
    TextMeshProUGUI _ignoreText;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    AudioClip _ignoreSE;

    const int _addPointCount = 3;//テキストに表示する。の個数

    public async UniTask TakeIgnoreReactionAsync(CancellationToken ct)
    {
        PlayAudio(_ignoreSE);

        _ignoreText.enabled = true;
        _ignoreText.text = "";

        //全ての。を順に表示していく
        for (int i = 0; i < _addPointCount; i++)
        {
            _ignoreText.text += "。　";
            await UniTask.Delay(TimeSpan.FromSeconds(_intervalAddPoint), cancellationToken: ct);
        }

        //少し待ってからテキストそのものを非表示にする
        await UniTask.Delay(TimeSpan.FromSeconds(_waitDurationFromAllShowToAllHide), cancellationToken: ct);

        _ignoreText.enabled = false;
        _audioSource.Stop();
    }

    void Start()
    {
        _ignoreText.enabled = false;
    }

    void PlayAudio(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
