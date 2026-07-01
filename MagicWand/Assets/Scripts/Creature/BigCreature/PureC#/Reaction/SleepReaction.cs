using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

//作成者:杉山
//ZZZのリアクション

[System.Serializable]
public class SleepReaction
{
    [Tooltip("zzzの文字。要素番号が0のものから順に表示していく")] [SerializeField]
    TextMeshProUGUI[] _zzzTexts;

    [SerializeField]
    float _showInterval = 0.3f;

    [Tooltip("全てのzの文字を表示してから、何秒で全てのzの文字を非表示にするか")] [SerializeField]
    float _waitDurationFromAllShowToAllHide;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    AudioClip _sleepSE;

    //睡眠のリアクションを行う、showZCountは表示するZの数
    public async UniTask SleepReactionAsunc(int showZCount,CancellationToken ct)
    {
        if(!MathfExtension.IsInRange(showZCount,0,_zzzTexts.Length))
        {
            Debug.Log("範囲外の表示するZの数が指定されています！");
            return;
        }

        //睡眠の効果音を流し始める
        PlayAudio(_sleepSE);

        await ShowZTextAsync(showZCount,ct);
    }

    public void Start()
    {
        //全てのzを非表示にしておく
        HideAllZText();
    }


    async UniTask ShowZTextAsync(int showZCount,CancellationToken ct)
    {
        //全てのzを順に表示していく
        for (int i = 0; i < showZCount; i++)
        {
            _zzzTexts[i].enabled = true;

            await UniTask.Delay(TimeSpan.FromSeconds(_showInterval), cancellationToken: ct);
        }

        //少し待ってから全てを非表示にする
        await UniTask.Delay(TimeSpan.FromSeconds(_waitDurationFromAllShowToAllHide), cancellationToken: ct);
        
        HideAllZText();
        _audioSource.Stop();
    }

    void HideAllZText()
    {
        for (int i = 0; i < _zzzTexts.Length; i++)
        {
            _zzzTexts[i].enabled = false;
        }
    }

    void PlayAudio(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
