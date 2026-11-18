using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//詠唱の履歴

//1回分のなぞりの情報
public class TraceInfo
{
    List<(EMagic magic, int sphereIndex)> _selectableMagicSphereIndexList;//選択できた魔法とそれに対応する魔法球の番号
    (EMagic magic, int sphereIndex) _selectedMagicSphereIndex;//実際に選択した魔法とそれに対応する魔法球の番号

    //選択できた魔法とそれに対応する魔法球の番号
    public List<(EMagic magic, int sphereIndex)> SelectableMagicSphereIndexList { get { return _selectableMagicSphereIndexList; } }

    //実際に選択した魔法とそれに対応する魔法球の番号
    public (EMagic magic, int sphereIndex) SelectedMagicSphereIndex { get { return _selectedMagicSphereIndex; } }

    public TraceInfo(List<(EMagic magic, int sphereIndex)> selectableMagicSphereIndexList, (EMagic magic, int sphereIndex) selectedMagicSphereIndex)
    {
        _selectableMagicSphereIndexList = selectableMagicSphereIndexList;
        _selectedMagicSphereIndex = selectedMagicSphereIndex;
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

    //新しい詠唱に切り替える
    public void CreateNewCastInfo()
    {
        CastInfo newCastInfo = new();
        _castInfos.Add(newCastInfo);
    }

    //なぞった時の情報を最新の詠唱の情報に追加
    public void AddNewTraceInfo(List<(EMagic magic, int sphereIndex)> selectableMagicSphereIndexList, (EMagic magic, int sphereIndex) selectedMagicSphereIndex)
    {
        if(_castInfos.Count - 1 < 0)
        {
            Debug.Log("詠唱の情報が１つもありません！");
            return;
        }

        TraceInfo newTraceInfo = new(selectableMagicSphereIndexList, selectedMagicSphereIndex);
        _castInfos[_castInfos.Count - 1].AddTraceInfo(newTraceInfo);
    }
}
