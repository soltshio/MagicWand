using Cysharp.Threading.Tasks;
using extOSC;
using System;
using System.Threading;
using UnityEngine;

//作成者:杉山
//OSC通信で送られてきた北陽レーザーが察知した物体(塊)の座標を受け取る
//物体が何も存在しない場合はBlobPositionにはx:0,y:0のベクトルが入っている

public class HokuyoDataReceiver : MonoBehaviour
{
    [SerializeField]
    OSCReceiver _oscReceiver;

    [SerializeField]
    OSCRunningChecker _oscRunnincChecker;

    [SerializeField]
    OSCAddressNameList _oscAddressNameList;

    private Vector2 _blobPosition = new();
    bool _isExistObject = false;//北陽レーザー検知範囲内にオブジェクトがあるか
    float _sizeScale;//画面の大きさに対する北陽レーザー検知範囲の拡大率
    Vector2 _center;//北陽レーザーの中心の設定
    Vector2 _size;//北陽レーザーのサイズの設定

    public event Action<Vector2> OnCatchPos;//OSC通信で位置を受け取ったことを通知(その時のBlobPositionが送られてくる)
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

    public bool IsRunning { get { return _oscRunnincChecker.IsRunning; } }

    //検知範囲内にオブジェクトが存在するか
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

    public Vector2 BlobPosition
    {
        get
        {
            return _blobPosition; 
        } 
        private set
        {
            _blobPosition = value;

            OnCatchPos?.Invoke(_blobPosition);
        }
    }

    public float SizeScale { get { return _sizeScale; } }
    public Vector2 Center { get { return _center; } }
    public Vector2 Size { get { return _size; } }

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
        _oscReceiver.Bind(_oscAddressNameList.PositionAddressName, ReceivePos);
        _oscReceiver.Bind(_oscAddressNameList.IsExistObjectAddressName, ReceiveIsExistObject);
        _oscReceiver.Bind(_oscAddressNameList.SizeScaleAddressName, ReceiveSizeScale);
        _oscReceiver.Bind(_oscAddressNameList.CenterAddressName, ReceiveCenter);
        _oscReceiver.Bind(_oscAddressNameList.SizeAddressName, ReceiveSize);
    }

    void ReceivePos(OSCMessage message)
    {
        _oscRunnincChecker.UpdateRunning(this.GetCancellationTokenOnDestroy());

        Vector2 blobPos;
        blobPos.x = message.Values[0].FloatValue;
        blobPos.y = message.Values[1].FloatValue;

        BlobPosition = blobPos;
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

    void ReceiveCenter(OSCMessage message)
    {
        _center.x = message.Values[0].FloatValue;
        _center.y = message.Values[0].FloatValue;
    }

    void ReceiveSize(OSCMessage message)
    {
        _size.x = message.Values[0].FloatValue;
        _size.y = message.Values[1].FloatValue;
    }
}
