using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

//作成者:杉山
//チュートリアルのセリフを流すクラス

[System.Serializable]
public class TutorialTextPlayer
{
    [SerializeField]
    TextMeshProUGUI _tutorialText;

    [Tooltip("セリフを話す時に流すAudioSource")] [SerializeField]
    AudioSource _dialogueAudioSource;

    [Tooltip("セリフを話す時に流す効果音(複数の中からランダムで選択)")] [SerializeField]
    AudioClip[] _dialogueAudioClips;

    [SerializeField]
    float _intervalPerCharacter = 0.1f;

    [SerializeField]
    float _intervalPerString = 2f;

    //セリフを１文字ずつ表示しながら見せていく
    public async UniTask PlayTextAsync(CancellationToken ct, string[] textContents)
    {
        try
        {
            for (int i = 0; i < textContents.Length; i++)
            {
                //テキストの文字を一旦全部消す
                _tutorialText.text = string.Empty;

                //効果音を決めてから流し始める
                _dialogueAudioSource.clip = DecideRandomDialogueSE();
                _dialogueAudioSource.Play();

                //1文字ずつ表示していく
                await DisplayTextLetterByLetter(ct, textContents[i]);

                //効果音を止める
                _dialogueAudioSource.Stop();

                //少し待ってから次の文の表示へ
                await UniTask.Delay(TimeSpan.FromSeconds(_intervalPerString), cancellationToken: ct);
            }
        }
        catch
        {
            if (_dialogueAudioSource != null) _dialogueAudioSource.Stop();//効果音を止める
        }
        
    }

    AudioClip DecideRandomDialogueSE()
    {
        int randomNum = UnityEngine.Random.Range(0, _dialogueAudioClips.Length);

        return _dialogueAudioClips[randomNum];
    }

    async UniTask DisplayTextLetterByLetter(CancellationToken ct,string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_intervalPerCharacter), cancellationToken: ct);

            _tutorialText.text += text[i];
        }
    }
}
