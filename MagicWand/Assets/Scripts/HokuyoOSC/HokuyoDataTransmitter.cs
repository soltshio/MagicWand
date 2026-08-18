using Cysharp.Threading.Tasks;
using extOSC;
using System.Threading;
using UnityEngine;

public class HokuyoDataTransmitter : MonoBehaviour
{
    [SerializeField]
    private OSCTransmitter transmitter;

    [SerializeField]
    OSCAddressNameList _oscAddressNameList;

    //インスタンスを取得する(まだ生成されていなかった場合待ってから取得する)
    public static async UniTask<HokuyoDataTransmitter> GetInstanceAsync(CancellationToken ct)
    {
        if (Instance != null) return Instance;

        await UniTask.WaitUntil(() => Instance != null, cancellationToken: ct);

        return Instance;
    }

    public static HokuyoDataTransmitter Instance
    {
        get;
        private set;
    }

    //OSC通信で画面の中心位置を送信する(向こう側で自動的に値が更新されるようになっている)
    public void SendCenter(Vector2 center)
    {
        OSCMessage message = new OSCMessage(_oscAddressNameList.CenterAddressName);

        message.AddValue(OSCValue.Float(center.x));
        message.AddValue(OSCValue.Float(center.y));

        transmitter.Send(message);
    }

    //OSC通信でモニターに映す画面サイズを送信する(向こう側で自動的に値が更新されるようになっている)
    public void SendSize(Vector2 size)
    {
        OSCMessage message = new OSCMessage(_oscAddressNameList.SizeAddressName);

        message.AddValue(OSCValue.Float(size.x));
        message.AddValue(OSCValue.Float(size.y));

        transmitter.Send(message);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
