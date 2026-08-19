using Cysharp.Threading.Tasks;
using extOSC;
using System;
using System.Threading;
using UnityEngine;

//作成者:杉山
//OSC通信で送られてきたデータを受け取る

public class HokuyoDataReceiver : MonoBehaviour
{
    [SerializeField]
    OSCReceiver _oscReceiver;

    [SerializeField]
    OSCRunningChecker _oscRunningChecker;

    [SerializeField]
    OSCAddressNameList _oscAddressNameList;

    private Vector2 _detectionPortPosition = new();
    bool _isExistObject = false;
    float _sizeScale;
    Vector2 _centerM;
    Vector2 _sizeM;

    public event Action<Vector2> OnCatchDetectionPortPos;//OSC通信で位置を受け取ったことを通知(その時のDetectionPortPositionが送られてくる)
    public event Action<bool> OnSwitchIsExistObject;

    //インスタンスを取得する(まだ生成されていなかった場合待ってから取得する)
    public static async UniTask<HokuyoDataReceiver> GetInstanceAsync(CancellationToken ct)
    {
        if(Instance != null) return Instance;

        await UniTask.WaitUntil(() => Instance != null,cancellationToken: ct);

        return Instance;
    }

    public static HokuyoDataReceiver Instance
    {
        get;
        private set;
    }

    public bool IsRunning { get { return _oscRunningChecker.IsRunning; } }

    //レーザー検知範囲内にオブジェクトがあるか
    public bool IsExistObject
    {
        get 
        {
            return _isExistObject;
        }
        private set
        {
            if (_isExistObject == value) return;

            _isExistObject = value;

            OnSwitchIsExistObject?.Invoke(_isExistObject);
        }
    }

    //レーザー検知範囲の中でどの位置に物体はあるか(xy共に0～1に正規化された割合で表す)
    public Vector2 DetectionPortPosition
    {
        get
        {
            return _detectionPortPosition; 
        } 
        private set
        {
            _detectionPortPosition = value;

            OnCatchDetectionPortPos?.Invoke(_detectionPortPosition);
        }
    }

    public float SizeScale { get { return _sizeScale; } }//実際の画面の大きさに対するレーザー検知範囲の拡大率
    public Vector2 CenterM { get { return _centerM; } }//レーザー検知範囲の中心位置の設定(単位はm)
    public Vector2 SizeM { get { return _sizeM; } }//実際の画面のサイズの設定(単位はm)

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

    //データの受け取り関連

    void Start()
    {
        _oscReceiver.Bind(_oscAddressNameList.PositionAddressName, ReceiveDetectionPortPos);
        _oscReceiver.Bind(_oscAddressNameList.IsExistObjectAddressName, ReceiveIsExistObject);
        _oscReceiver.Bind(_oscAddressNameList.SizeScaleAddressName, ReceiveSizeScale);
        _oscReceiver.Bind(_oscAddressNameList.CenterAddressName, ReceiveCenterM);
        _oscReceiver.Bind(_oscAddressNameList.SizeAddressName, ReceiveSizeM);
    }

    void ReceiveDetectionPortPos(OSCMessage message)
    {
        _oscRunningChecker.UpdateRunning(this.GetCancellationTokenOnDestroy());

        Vector2 blobPos;
        blobPos.x = message.Values[0].FloatValue;
        blobPos.y = message.Values[1].FloatValue;

        DetectionPortPosition = blobPos;
    }

    void ReceiveIsExistObject(OSCMessage message)
    {
        if (!message.ToBool(out bool value)) return;
        
        IsExistObject = value;
    }

    void ReceiveSizeScale(OSCMessage message)
    {
        _sizeScale = message.Values[0].FloatValue;
    }

    void ReceiveCenterM(OSCMessage message)
    {
        _centerM.x = message.Values[0].FloatValue;
        _centerM.y = message.Values[0].FloatValue;
    }

    void ReceiveSizeM(OSCMessage message)
    {
        _sizeM.x = message.Values[0].FloatValue;
        _sizeM.y = message.Values[1].FloatValue;
    }
}
