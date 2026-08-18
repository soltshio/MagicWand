using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

//作成者:杉山
//カーソルの位置の調整シーンの開始処理

[System.Serializable]
public class Finish_CursorAdjust
{
    [SerializeField]
    TextMeshProUGUI _finishText;

    [SerializeField]
    float _showDuration = 2f;

    public async UniTask InformFinishAsync(CancellationToken ct)
    {
        _finishText.gameObject.SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(_showDuration), cancellationToken: ct);

        _finishText.gameObject.SetActive(false);
    }
}
