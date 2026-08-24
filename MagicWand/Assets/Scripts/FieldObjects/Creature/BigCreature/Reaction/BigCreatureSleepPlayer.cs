using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

//作成者:杉山
//巨大生物の睡眠

public class BigCreatureSleepPlayer : MonoBehaviour
{
    [Tooltip("zzzの文字。要素番号が0のものから順に表示していく")] [SerializeField]
    TextMeshProUGUI[] _zzzTexts;

    [SerializeField]
    float _showInterval = 0.8f;

    [Tooltip("全てのzの文字を表示してから、何秒で全てのzの文字を非表示にするか")] [SerializeField]
    float _waitDurationFromAllShowToAllHide = 1.3f;

    [Tooltip("全てのzの文字を非表示にしてから、何秒でzの文字を表示し始めるか")] [SerializeField]
    float _waitDurationFromAllHideToStartShowZ = 1.5f;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    BigCreatureStatus _bigCreatureStatus;

    int _showZCount;//Zを表示する数、現状は起きるまでの残り放たなければならない正解の魔法の回数分表示する
    SingleTaskCancellation _singleTaskCancellation = new();

    public void Play()
    {
        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        ShowSleepZAsync(ct).Forget();

        _audioSource.Play();
    }

    public void Stop()
    {
        _singleTaskCancellation.Cancel();

        HideAllZText();

        _audioSource.Stop();
    }

    async UniTask ShowSleepZAsync(CancellationToken ct)
    {
        while (true)
        {
            int showZCount = Mathf.Min(_zzzTexts.Length, _showZCount);

            //zを順に表示していく
            for (int i = 0; i < showZCount; i++)
            {
                _zzzTexts[i].enabled = true;

                await UniTask.Delay(TimeSpan.FromSeconds(_showInterval), cancellationToken: ct);
            }

            //少し待ってから全てを非表示にする
            await UniTask.Delay(TimeSpan.FromSeconds(_waitDurationFromAllShowToAllHide), cancellationToken: ct);

            HideAllZText();

            //少し待ってからまたzを表示し始める
            await UniTask.Delay(TimeSpan.FromSeconds(_waitDurationFromAllHideToStartShowZ), cancellationToken: ct);
        }
    }

    void HideAllZText()
    {
        for (int i = 0; i < _zzzTexts.Length; i++)
        {
            _zzzTexts[i].enabled = false;
        }
    }

    void OnEnable()
    {
        _bigCreatureStatus.OnUpdateHP += UpdateShowZCount;
    }

    private void OnDisable()
    {
        _bigCreatureStatus.OnUpdateHP -= UpdateShowZCount;
    }

    void Start()
    {
        _showZCount = _bigCreatureStatus.HP;

        //全てのzを非表示にしておく
        HideAllZText();
    }

    void UpdateShowZCount(int hp)
    {
        _showZCount = hp;
    }
}
