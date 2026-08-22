using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.Playables;

//PlayableDirectorのstoppedイベントをUniTaskで待機するための拡張メソッド

public static class PlayableDirectorExtensions
{
    public static UniTask WaitForStoppedAsync(this PlayableDirector director, CancellationToken token)
    {
        var tcs = new UniTaskCompletionSource();

        void OnStopped(PlayableDirector _)
        {
            director.stopped -= OnStopped;
            tcs.TrySetResult();
        }

        director.stopped += OnStopped;

        token.Register(() =>
        {
            director.stopped -= OnStopped;
            tcs.TrySetCanceled();
        });

        return tcs.Task;
    }
}