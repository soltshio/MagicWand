using Cysharp.Threading.Tasks;
using extOSC;
using UnityEngine;

public class TestOSCTransmit : MonoBehaviour
{
    [SerializeField]
    private OSCTransmitter transmitter;

    [SerializeField]
    float value;

    async void Start()
    {
        await UniTask.Delay(5000,cancellationToken:this.GetCancellationTokenOnDestroy());

        OSCMessage message = new OSCMessage("/test");

        message.AddValue(OSCValue.Float(value));

        transmitter.Send(message);
    }
}
