using Cysharp.Threading.Tasks;
using extOSC;
using System;
using UnityEngine;

public class TestOSCTransmit : MonoBehaviour
{
    [SerializeField]
    private OSCTransmitter transmitter;

    async void Start()
    {
        var hokuyoDataTransmitter = await HokuyoDataTransmitter.GetInstanceAsync(this.GetCancellationTokenOnDestroy());

        hokuyoDataTransmitter.SendCenterM(new Vector2(1.0f, 2.0f));
        hokuyoDataTransmitter.SendSizeM(new Vector2(3.0f, 4.0f));
    }
}
