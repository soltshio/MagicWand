using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

//作成者:杉山
//全てのタスクが終了するまで、ポーズして待つイベントディレクター

[System.Serializable]
public class WaitUntilAllFinishTasksEventDirecter
{
    [SerializeField]
    PlayableDirector _eventDirecter;

    List<UniTask> _runningTasks = new();

    public void AddTasks(UniTask task)
    {
        _runningTasks.Add(task);
    }

    public void ClearTasks()
    {
        _runningTasks.Clear();
    }

    public async UniTask StartPlayingAndWaitUntilFinishPlayingAsync(CancellationToken ct)
    {
        _eventDirecter.Play();

        //タイムラインの再生が終わるまで待つ
        await _eventDirecter.WaitForStoppedAsync(ct);
    }

    public async UniTask PauseUntilAllFinishTasksAsync()
    {
        _eventDirecter.Pause();

        //全てのタスクが終わるまでポーズしている
        await UniTask.WhenAll(_runningTasks);

        _eventDirecter.Play();
    }
}
