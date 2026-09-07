using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//魔法を発動させる機能

public class MagicInvoker : MonoBehaviour
{
    [SerializeField]
    SerializableDictionary<EMagic, MagicContentTypeBase> _magicContents;

    public event Action OnMagicInvoked;

    bool _isPlayingEvent = false;//魔法の演出中か

    public bool IsPlayingEvent { get => _isPlayingEvent; }

    public async UniTask InvokeMagicAsync(EMagic invokableMagic)
    {
        if (_isPlayingEvent) return;

        _isPlayingEvent = true;
        OnMagicInvoked?.Invoke();

        var token = this.GetCancellationTokenOnDestroy();

        List<UniTask> runningTasks =new();

        if (!_magicContents.TryGetValue(invokableMagic, out var value)) return;

        runningTasks.Add(value.ActivateAsync(token));

        await UniTask.WhenAll(runningTasks);

        _isPlayingEvent = false;
    }
}
