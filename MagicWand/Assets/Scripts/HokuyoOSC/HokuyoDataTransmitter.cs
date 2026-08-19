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

    //OSC通信で画面のレーザー検知範囲の中心位置(単位はm)を送信する(向こう側で自動的に値が更新されるようになっている)
    public void SendCenterM(Vector2 newCenterM)
    {
        OSCMessage message = new OSCMessage(_oscAddressNameList.CenterAddressName);

        message.AddValue(OSCValue.Float(newCenterM.x));
        message.AddValue(OSCValue.Float(newCenterM.y));

        transmitter.Send(message);
    }

    //OSC通信で実際の画面のサイズ(単位はm)を送信する(向こう側で自動的に値が更新されるようになっている)
    public void SendSizeM(Vector2 newSizeM)
    {
        OSCMessage message = new OSCMessage(_oscAddressNameList.SizeAddressName);

        message.AddValue(OSCValue.Float(newSizeM.x));
        message.AddValue(OSCValue.Float(newSizeM.y));

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
