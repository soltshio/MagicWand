using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//詠唱の履歴

//1回分のなぞりの情報
public class TraceInfo
{
    EMagic[] _selectableMagicsBySphere;//要素番号を球の番号として、その番号の球で選ぶことのできた魔法
    int _selectedSphereIndex;//選んだ球の番号

    public EMagic[] SelectableMagicsBySphere { get { return _selectableMagicsBySphere; } }
    public int SelectedSphereIndex { get { return _selectedSphereIndex; } }

    public TraceInfo(EMagic[] selectableMagicsBySphere,int selectedSphereIndex)
    {
        _selectableMagicsBySphere = selectableMagicsBySphere;
        _selectedSphereIndex = selectedSphereIndex;
    }
}

//1回分の詠唱の情報
public class CastInfo
{
    List<TraceInfo> _traceInfos;

    public List<TraceInfo> TraceInfos
    {
        get
        {
            //そのままリストを渡してしまうと、渡した先でAddなどをされた時に自分の持っているリストまで影響が及んでしまうため、リストのコピーを渡す
            return new List<TraceInfo>(_traceInfos);
        }
    }

    public CastInfo()
    {
        _traceInfos = new();
    }

    public void AddTraceInfo(TraceInfo newTraceInfo)
    {
        _traceInfos.Add(newTraceInfo);
    }
}

public class CastHistory : MonoBehaviour
{
    List<CastInfo> _castInfos;

    public List<CastInfo> CastInfos
    {
        get
        {
            //そのままリストを渡してしまうと、渡した先でAddなどをされた時に自分の持っているリストまで影響が及んでしまうため、リストのコピーを渡す
            return new List<CastInfo>(_castInfos);
        }
    }

    void Awake()
    {
        _castInfos = new();
    }

    //最新の詠唱の情報の取得を試みる
    public bool TryGetLatestCastInfo(out CastInfo latestCastInfo)
    {
        latestCastInfo = null;

        //詠唱の履歴情報が何も追加されていなければ弾く
        if (CastInfos.Count <= 0) return false;

        //現在の詠唱の状況
        latestCastInfo = CastInfos[CastInfos.Count - 1];
        return true;
    }

    //最新のなぞりの情報の取得を試みる
    public bool TryGetLatestTraceInfo(out TraceInfo latestTraceInfo)
    {
        latestTraceInfo = null;

        if (!TryGetLatestCastInfo(out var latestCastInfo)) return false;

        //なぞりの履歴情報が何も追加されていなければ弾く
        int traceInfosCount = latestCastInfo.TraceInfos.Count;

        if (traceInfosCount <= 0) return false;

        latestTraceInfo = latestCastInfo.TraceInfos[traceInfosCount - 1];
        return true;
    }

    //新しい詠唱に切り替える
    public void CreateNewCastInfo()
    {
        CastInfo newCastInfo = new();
        _castInfos.Add(newCastInfo);
    }

    //なぞった時の情報を最新の詠唱の情報に追加
    public void AddNewTraceInfo(EMagic[] selectableMagicsBySphere, int selectedSphereIndex)
    {
        if(_castInfos.Count - 1 < 0)
        {
            Debug.Log("詠唱の情報が１つもありません！");
            return;
        }

        TraceInfo newTraceInfo = new(selectableMagicsBySphere, selectedSphereIndex);
        _castInfos[_castInfos.Count - 1].AddTraceInfo(newTraceInfo);
    }
}
