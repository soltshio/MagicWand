using UnityEngine;
using Cysharp.Threading.Tasks;

//作成者:杉山
//測距(北陽)レーザーのオプションUI

public class HokuyoOptionUI : MonoBehaviour
{
    [Tooltip("センサーからの値が受け取れない時に表示するUI")] [SerializeField]
    GameObject _noSignalUI;

    [Tooltip("調整UI(センサーからの値が受け取れている時に表示するUI)")] [SerializeField]
    GameObject _sensorOptionUI;

    HokuyoDataReceiver _hokuyoDataReceiverInstance;
    HokuyoDataTransmitter _hokuyoDataTransmitterInstance;

    async void Start()
    {
        _hokuyoDataReceiverInstance = await HokuyoDataReceiver.GetInstanceAsync(this.GetCancellationTokenOnDestroy());
        _hokuyoDataTransmitterInstance = await HokuyoDataTransmitter.GetInstanceAsync(this.GetCancellationTokenOnDestroy());
    }

    private void OnEnable()
    {
        _hokuyoDataReceiverInstance.OnSwitchIsRunning += SwitchNoSignalUI;
    }

    private void OnDisable()
    {
        _hokuyoDataReceiverInstance.OnSwitchIsRunning -= SwitchNoSignalUI;
    }

    void SwitchNoSignalUI(bool isRunning)
    {
        _sensorOptionUI.SetActive(isRunning);
        _noSignalUI.SetActive(!isRunning);
    }

    void Update()
    {

    }
}
