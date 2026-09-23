// Unityでシリアル通信を制御するクラス
// 例えば空のGameObjectを作り、そこにアタッチする
// 2025_10月Ver.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO.Ports; // これを通すために、Api Compatibility Levelの設定を変更
using System.Threading;
using System;

//通信を接続や切断を行う基本のクラス

public class SerialHandler : MonoBehaviour
{
    public delegate void SerialDataReceivedEventHandler(string message);
    public event SerialDataReceivedEventHandler OnDataReceived;

    [SerializeField]
    int _bitRate = 115200;

    // COM10以上は\\\\.\\を付加しないと開けない。
    // portNameに直接代入すると失敗するので、ここでいったん別の変数に代入し、AwakeでportNameに代入
	// myPortNameが空文字列であればOpenを呼ばない＝デバイスがなくてもアプリケーションを実行することができる
    const string _myPortName = "\\\\.\\COM";
    
    SerialPort _serialPort;
    Thread _thread;
    bool _isRunning = false;

    string _message;
    bool _isNewMessageReceived = false;

    public static SerialHandler Instance { get; private set; }

    public bool IsRunning => _isRunning;// シリアル通信が実行中かどうかを返すプロパティ

    //通信を始める
    public void Open(int portNum)
    {
        if(_isRunning)
        {
            Debug.LogWarning("既に通信中です。");
            return;
        }

        string portName = _myPortName + portNum.ToString();

        _serialPort = new SerialPort(portName, _bitRate, Parity.None, 8, StopBits.One);

        _serialPort.RtsEnable = true;
        _serialPort.DtrEnable = true;

        _serialPort.Open();

        _isRunning = true;

        _thread = new Thread(Read);
        _thread.Start();
    }

    //通常を終了する
    public void Close()
    {
        if (!_isRunning)
        {
            Debug.LogWarning("通信が開始されていません。");
            return;
        }

        _isNewMessageReceived = false;
        _isRunning = false;

        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.Close();
        }

        if (_thread != null && _thread.IsAlive)
        {
            if (!_thread.Join(500)) // 500msは適当な時間
            {
                // 少し待って応答がなければ強制終了
                Debug.LogWarning("from SerialHandler.cs Close(): Abort");
                _thread.Abort();
            }
        }

        if (_serialPort != null)
        {
            _serialPort.Dispose();
        }
    }

    void Awake()
    {
        //インスタンスを生成しておく(シングルトン)
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (!_isNewMessageReceived) return;

        OnDataReceived(_message);
        _isNewMessageReceived = false;
    }

    void OnDestroy()
    {
        if (_isRunning)
        {
            Close();
        }
    }

    private void Read()
    {
        while (_isRunning && _serialPort != null && _serialPort.IsOpen)
        {
            try
            {
                _message = _serialPort.ReadLine(); // 改行付きデータが送られる前提
                _isNewMessageReceived = true;
            }
            catch (TimeoutException)
            {
                // このタイムアウトは通常の状態なのでスルーする
            }
            catch (System.Exception e)
            {
                if (!_isRunning)
                    break;

                Debug.LogWarning("from SerialHandler.cs Read(): " + e.Message);
            }
        }
    }

    public void Write(string message)
    {
        try
        {
            _serialPort.Write(message);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("from SerialHandler.cs Write(): " + e.Message);
        }
    }
}
