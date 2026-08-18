using Cysharp.Threading.Tasks;
using extOSC;
using System;
using UnityEngine;

public class TestOSCTransmit : MonoBehaviour
{
    [SerializeField]
    private OSCTransmitter transmitter;

    [SerializeField]
    float value;

    [SerializeField]
    float value2;

    async void Start()
    {
        await UniTask.Delay(5000,cancellationToken:this.GetCancellationTokenOnDestroy());

        OSCMessage message = new OSCMessage("/test");

        message.AddValue(OSCValue.Float(value));

        transmitter.Send(message);

        await UniTask.Delay(5000, cancellationToken: this.GetCancellationTokenOnDestroy());

        OSCMessage message2 = new OSCMessage("/ppp");

        message2.AddValue(OSCValue.Float(value2));

        transmitter.Send(message2);
    }
}
