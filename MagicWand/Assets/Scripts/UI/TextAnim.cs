using UnityEngine;
using TMPro;
using System.Collections;

public class TextAnim : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText;

    void Start()
    {
        StartCoroutine(Simple());
    }

    private IEnumerator Simple()
    {
        // 文字の表示数を0に(テキストが表示されなくなる)
        tmpText.maxVisibleCharacters = 0;

        // テキストの文字数分ループ
        for (var i = 0; i < tmpText.text.Length; i++)
        {
            // 一文字ごとに0.2秒待機
            yield return new WaitForSeconds(0.1f);

            // 文字の表示数を増やしていく
            tmpText.maxVisibleCharacters = i + 1;
        }
    }
}

        