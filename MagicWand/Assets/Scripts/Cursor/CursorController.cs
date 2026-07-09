using extOSC;
using UnityEngine;

//作成者:杉山
//OSC通信で送られてきた北陽レーザーの位置情報を取得し、カーソルを動かす

public class CursorController : MonoBehaviour
{
    [SerializeField]
    OSCReceiver _oscReceiver;

    float tx;
    float ty;

    void Start()
    {
        _oscReceiver.Bind("/tx", ReceiveX);
        _oscReceiver.Bind("/ty", ReceiveY);
    }

    void ReceiveX(OSCMessage message)
    {
        tx = message.Values[0].FloatValue;
    }

    void ReceiveY(OSCMessage message)
    {
        ty = message.Values[0].FloatValue;
    }

    void Update()
    {
        Debug.Log(tx + ":" + ty);
    }
}
