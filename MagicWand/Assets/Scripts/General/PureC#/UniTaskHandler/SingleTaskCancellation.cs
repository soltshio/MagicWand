using System.Threading;

//作成者:杉山
//よく使う、タスクを必ず一つだけ動かすようにする処理のクラス

public class SingleTaskCancellation
{
    CancellationTokenSource _cts;

    public CancellationToken CancelAndReCreateToken(CancellationToken ct)
    {
        CancelRunningUniTask();

        return CreateLinkedToken(ct);
    }

    void CancelRunningUniTask()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    CancellationToken CreateLinkedToken(CancellationToken ct)
    {
        _cts = new CancellationTokenSource();

        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, ct);

        return linkedCts.Token;
    }
}
