using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//カーソルの位置の調整シーンの開始処理

[System.Serializable]
public class Start_CursorAdjust
{
    [SerializeField]
    TextMeshProUGUI _startText;

    [SerializeField]
    float _showDuration = 2f;

    public async UniTask InformStartAsync(CancellationToken ct)
    {
        _startText.gameObject.SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(_showDuration), cancellationToken: ct);

        _startText.gameObject.SetActive(false);
    }
}
