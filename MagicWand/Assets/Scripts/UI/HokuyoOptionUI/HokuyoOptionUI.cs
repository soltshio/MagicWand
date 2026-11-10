using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//測距(北陽)レーザーのオプションUI

public class HokuyoOptionUI : MonoBehaviour
{
    [SerializeField]
    HokuyoDataValueInputField _centerXInputField;

    [SerializeField]
    HokuyoDataValueInputField _centerYInputField;

    [SerializeField]
    HokuyoDataValueInputField _sizeXInputField;

    [SerializeField]
    HokuyoDataValueInputField _sizeYInputField;

    [SerializeField]
    HokuyoDataValueInputField _sizeScaleInputField;


    [Tooltip("センサーからの値が受け取れない時に表示するUI")] [SerializeField]
    GameObject _noSignalUI;

    [Tooltip("調整UI(センサーからの値が受け取れている時に表示するUI)")] [SerializeField]
    GameObject _sensorOptionUI;


    [Header("不正な入力がされた時の設定")]

    [Tooltip("不正な入力がされた時に表示するテキスト内容")] [Multiline(2)] [SerializeField]
    string _textContent_InvalidInput;

    HokuyoDataReceiver _hokuyoDataReceiverInstance;
    HokuyoDataTransmitter _hokuyoDataTransmitterInstance;

    const int _frameCountToInitSwitchShowNoSignalUI = 3;


    //☆入力受け取り
    //中心Xへの入力
    public void OnInputtedCenterX(float newCenterX)
    {
        if (_hokuyoDataReceiverInstance == null) return;

        Vector2 newCenter = _hokuyoDataReceiverInstance.CenterM;
        newCenter.x = newCenterX;

        SendCenterValue(newCenter);
    }

    //中心Yへの入力
    public void OnInputtedCenterY(float newCenterY)
    {
        if (_hokuyoDataReceiverInstance == null) return;

        Vector2 newCenter = _hokuyoDataReceiverInstance.CenterM;
        newCenter.y = newCenterY;

        SendCenterValue(newCenter);
    }

    //サイズXへの入力
    public void OnInputtedSizeX(float newSizeX)
    {
        if (_hokuyoDataReceiverInstance == null) return;

        Vector2 newSize = _hokuyoDataReceiverInstance.SizeM;
        newSize.x = newSizeX;

        SendSizeValue(newSize);
    }

    //サイズYへの入力
    public void OnInputtedSizeY(float newSizeY)
    {
        if (_hokuyoDataReceiverInstance == null) return;

        Vector2 newSize = _hokuyoDataReceiverInstance.SizeM;
        newSize.y = newSizeY;

        SendSizeValue(newSize);
    }

    //SizeScaleへの入力
    public void OnInputtedSizeScale(float sizeScale)
    {
        if (_hokuyoDataReceiverInstance == null) return;

        SendSizeScale(sizeScale);
    }




    async void OnEnable()
    {
        //インスタンスが取得できていなければ取得する
        if(_hokuyoDataReceiverInstance == null) _hokuyoDataReceiverInstance = await HokuyoDataReceiver.GetInstanceAsync(this.GetCancellationTokenOnDestroy());
        if(_hokuyoDataTransmitterInstance == null) _hokuyoDataTransmitterInstance = await HokuyoDataTransmitter.GetInstanceAsync(this.GetCancellationTokenOnDestroy());

        InitSwitchShowNoSignalUIAsync(this.GetCancellationTokenOnDestroy()).Forget();

        _hokuyoDataReceiverInstance.OnSwitchIsRunning += SwitchNoSignalUI;

        _centerXInputField.OnInputtedValue += OnInputtedCenterX;
        _centerYInputField.OnInputtedValue += OnInputtedCenterY;
        _sizeXInputField.OnInputtedValue += OnInputtedSizeX;
        _sizeYInputField.OnInputtedValue += OnInputtedSizeY;
        _sizeScaleInputField.OnInputtedValue += OnInputtedSizeScale;
    }

    void OnDisable()
    {
        _hokuyoDataReceiverInstance.OnSwitchIsRunning -= SwitchNoSignalUI;

        _centerXInputField.OnInputtedValue -= OnInputtedCenterX;
        _centerYInputField.OnInputtedValue -= OnInputtedCenterY;
        _sizeXInputField.OnInputtedValue -= OnInputtedSizeX;
        _sizeYInputField.OnInputtedValue -= OnInputtedSizeY;
        _sizeScaleInputField.OnInputtedValue -= OnInputtedSizeScale;
    }

    //起動時、少し遅延させてからじゃないとIsRunningの値が正しく取得できない場合があるので、少し遅延させてからUIの表示切り替えを行う
    async UniTask InitSwitchShowNoSignalUIAsync(CancellationToken ct)
    {
        if (_hokuyoDataReceiverInstance == null) return;

        for(int i=0; i< _frameCountToInitSwitchShowNoSignalUI ;i++)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }
            
        SwitchNoSignalUI(_hokuyoDataReceiverInstance.IsRunning);
    }

    //測距センサーからデータを受け取れていない場合は設定UIを表示させないようにする
    void SwitchNoSignalUI(bool isRunning)
    {
        _sensorOptionUI.SetActive(isRunning);
        _noSignalUI.SetActive(!isRunning);
    }

    void Update()
    {
        if (!_hokuyoDataReceiverInstance.IsRunning) return;

        //現在の中心位置を更新
        _centerXInputField.SetInputFieldText(_hokuyoDataReceiverInstance.CenterM.x);
        _centerYInputField.SetInputFieldText(_hokuyoDataReceiverInstance.CenterM.y);
        //現在のサイズを更新
        _sizeXInputField.SetInputFieldText(_hokuyoDataReceiverInstance.SizeM.x);
        _sizeYInputField.SetInputFieldText(_hokuyoDataReceiverInstance.SizeM.y);
        //現在のサイズ拡大率を更新
        _sizeScaleInputField.SetInputFieldText(_hokuyoDataReceiverInstance.SizeScale);
    }



    //☆HokuyoDataTransmitterの送信処理を呼ぶ処理
    void SendCenterValue(Vector2 center)
    {
        if (_hokuyoDataTransmitterInstance == null) return;

        _hokuyoDataTransmitterInstance.SendCenterM(center);//送信
    }

    void SendSizeValue(Vector2 size)
    {
        if (_hokuyoDataTransmitterInstance == null) return;

        _hokuyoDataTransmitterInstance.SendSizeM(size);//送信
    }

    void SendSizeScale(float sizeScale)
    {
        if (_hokuyoDataTransmitterInstance == null) return;

        _hokuyoDataTransmitterInstance.SendSizeScale(sizeScale);
    }
}
