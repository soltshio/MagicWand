using extOSC;
using System;
using UnityEngine;
using UnityEngine.UIElements;

//作成者:杉山
//OSC通信で送られてきた北陽レーザーが察知した物体(塊)の座標を受け取る

public class HokuyoBlobPosReceiver : MonoBehaviour
{
    [SerializeField]
    OSCReceiver _oscReceiver;

    private Vector2 _blobPosition = new();
    const string _posAddressName="/position";

    public event Action<Vector2> OnMovePos;

    public Vector2 BlobPosition
    {
        get
        {
            return _blobPosition; 
        } 
        private set
        {
            _blobPosition = value;
            OnMovePos?.Invoke(_blobPosition);
        }
    }

    void Start()
    {
        _oscReceiver.Bind(_posAddressName, ReceivePos);
    }

    void ReceivePos(OSCMessage message)
    {
        Vector2 blobPos;
        blobPos.x = message.Values[0].FloatValue;
        blobPos.y = message.Values[1].FloatValue;

        BlobPosition = blobPos;
        
    }

    void Update()
    {
        Debug.Log(BlobPosition);
    }

}
