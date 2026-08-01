using System.Collections.Generic;

//作成者:杉山
//通った魔法球の番号を記録するクラス

public class PassedSphereIndexHistory
{
    private readonly List<List<int>> _passedIndexHistory = new();
    private List<int> _currentHistory;

    public void AddIndex(int index)
    {
        _currentHistory.Add(index);
    }

    public void CreateNewHistory()
    {
        _currentHistory = new List<int>();
        _passedIndexHistory.Add(_currentHistory);
    }

    public IReadOnlyList<List<int>> GetAllHistory()
    {
        return _passedIndexHistory;
    }

    //一番最新の番号を取得する
    //履歴が空の場合はfalse
    public bool TryGetLatestIndex(out int index)
    {
        if (_currentHistory.Count == 0)
        {
            index = -1;
            return false;
        }

        index = _currentHistory[_currentHistory.Count - 1];
        return true;
    }
}
