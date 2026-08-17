using System;
using UnityEngine;

//作成者:杉山
//OSCから送られてくるデータのアドレス名をまとめたリスト

[CreateAssetMenu(fileName = "OSCAddressNameList", menuName = "ScriptableObjects/Create OSCAddressNameList")]
public class OSCAddressNameList : ScriptableObject
{
    [SerializeField]
    string _positionAddressName;

    [SerializeField]
    string _isExistBlobsAddressName;

    [SerializeField]
    string _sizeScaleAddressName;

    [SerializeField]
    string _centerAddressName;

    [SerializeField]
    string _sizeAddressName;

    public string PositionAddressName { get { return _positionAddressName; } }
    public string IsExistBlobsAddressName { get { return _isExistBlobsAddressName; } }
    public string SizeScaleAddressName { get { return _sizeScaleAddressName; } }
    public string CenterAddressName { get { return _centerAddressName; } }
    public string SizeAddressName { get { return _sizeAddressName; } }
}
