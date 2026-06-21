using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//魔法を発動させる機能

public class MagicInvoker : MonoBehaviour
{
    [SerializeField]
    SerializableDictionary<EMagic, MagicContentTypeBase> _magicContents;

    bool _isPlayingEvent = false;//魔法の演出中か

    public bool IsPlayingEvent { get => _isPlayingEvent; }

    public async UniTask InvokeMagicAsync(EMagic[] invokableMagics)
    {
        if (_isPlayingEvent) return;

        _isPlayingEvent = true;

        var token = this.GetCancellationTokenOnDestroy();

        List<UniTask> runningTasks =new();

        for(int i=0; i<invokableMagics.Length ;i++)
        {
            if (!_magicContents.TryGetValue(invokableMagics[i], out var value)) continue;

            runningTasks.Add(value.ActivateAsync(token));
        }

        await UniTask.WhenAll(runningTasks);

        _isPlayingEvent = false;
    }
}
