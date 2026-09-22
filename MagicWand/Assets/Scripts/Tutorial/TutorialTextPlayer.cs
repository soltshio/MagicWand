using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using System.Xml;
using TMPro;
using UnityEngine;

//作成者:杉山
//チュートリアルのセリフを流すクラス

[System.Serializable]
public class TutorialTextPlayer
{
    [SerializeField]
    TextMeshProUGUI _tutorialText;

    [SerializeField]
    float _intervalPerCharacter = 0.1f;

    [SerializeField]
    float _intervalPerString = 2f;

    //セリフを１文字ずつ表示しながら見せていく
    public async UniTask PlayTextAsync(CancellationToken ct, string[] textContents)
    {
        for(int i=0; i<textContents.Length ;i++)
        {
            //テキストの文字を一旦全部消す
            _tutorialText.text = string.Empty;

            //1文字ずつ表示していく
            await DisplayTextLetterByLetter(ct, textContents[i]);

            //少し待ってから次の文の表示へ
            await UniTask.Delay(TimeSpan.FromSeconds(_intervalPerString), cancellationToken: ct);
        }
    }

    async UniTask DisplayTextLetterByLetter(CancellationToken ct,string text)
    {
        //一旦、文字の表示数を0に
        _tutorialText.maxVisibleCharacters = 0;

        for (int i = 0; i < text.Length; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_intervalPerCharacter), cancellationToken: ct);

            //順に文字の表示数を増やしていく
            _tutorialText.maxVisibleCharacters++;

            _tutorialText.text += text[i];
        }
    }
}
