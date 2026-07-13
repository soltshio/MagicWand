using extOSC;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

//作成者:杉山
//OSC通信で送られてきた北陽レーザーが察知した物体(塊)の座標を受け取る
//物体が何も存在しない場合はBlobPositionにはx:0,y:0のベクトルが入っている

public class HokuyoBlobPosReceiver : MonoBehaviour
{
    [SerializeField]
    OSCReceiver _oscReceiver;

    [SerializeField]
    OSCRunningChecker _oscRunnincChecker;

    [SerializeField]
    Vector2 _isNotExistObjectThreashold;

    private Vector2 _blobPosition = new();
    const string _posAddressName="/position";

    bool _isExistObject = false;//北陽レーザー検知範囲内にオブジェクトがあるか

    public event Action<Vector2> OnCatchPos;//OSC通信で位置を受け取ったことを通知(その時のBlobPositionが送られてくる)
    public event Action<bool> OnSwitchExistObject;

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
            OnSwitchExistObject?.Invoke(_isExistObject);
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

            //検知範囲内にオブジェクトがないかの判定の更新
            UpdateIsExistObject(_blobPosition);

            OnCatchPos?.Invoke(_blobPosition);
        }
    }

    void Start()
    {
        _oscReceiver.Bind(_posAddressName, ReceivePos);
    }

    void ReceivePos(OSCMessage message)
    {
        _oscRunnincChecker.UpdateRunning(this.GetCancellationTokenOnDestroy());

        Vector2 blobPos;
        blobPos.x = message.Values[0].FloatValue;
        blobPos.y = message.Values[1].FloatValue;

        BlobPosition = blobPos;
    }

    void UpdateIsExistObject(Vector2 blobPosition)
    {
        //blobPoositionがだいたいゼロベクトルであれば、オブジェクトが検知範囲内に無いことにする
        //値をfloatで管理しているので、閾値内であればゼロベクトルという判定にする

        //xの比較
        float x = blobPosition.x;
        float threasholdX = _isNotExistObjectThreashold.x;

        if (!MathfExtension.IsInRange(x, -threasholdX, threasholdX))
        {
            IsExistObject = true;
            return;
        }


        //yの比較
        float y = blobPosition.y;
        float threasholdY = _isNotExistObjectThreashold.y;

        if (!MathfExtension.IsInRange(y, -threasholdY, threasholdY))
        {
            IsExistObject = true;
            return;
        }

        IsExistObject = false;
    }
}
