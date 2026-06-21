using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//作成者:杉山
//杖を制御するクラス
//常にジャイロオン
//ひっくり返っているかを考慮する
//回転速度は調整不可

//TODO
//初期化は諦め(後にゲーム開始してから数秒後に自動的にリセットさせるようにする)



public class WandControllerVer1 : MonoBehaviour
{
    [SerializeField]
    JoyconInputManager _joyconInputManager;

    [SerializeField]
    KeyWandController _keyWandController;

    [SerializeField]
    JoyconWandController _joyconWandController;

    [Tooltip("杖(動かす対象)")] [SerializeField]
    Transform _wand;

    [Tooltip("リセットにかかる時間")] [SerializeField]
    float _resetDuration = 1f;

    Quaternion _originRot= Quaternion.identity;
    bool _isResetting = false;

    public Quaternion OriginRot => _originRot;

    //現在の絶対的な回転の値
    public Quaternion CurrentAbsoluteRot()
    {
        return _wand.localRotation * Quaternion.Inverse(_originRot);
    }

    public void ResetAiming(InputAction.CallbackContext context)//照準をリセット
    {
        if (!context.performed) return;

        if (_isResetting) return;//リセット中は無視

        Quaternion currentRot = CurrentAbsoluteRot();

        //今の絶対的な回転の逆回転を基準の回転として保存することで、今の向きを基準の向きとして扱う
        StartCoroutine(ResetAimingCoroutine(Quaternion.Inverse(currentRot)));
    }

    IEnumerator ResetAimingCoroutine(Quaternion newOriginRot)
    {
        _isResetting = true;
        Quaternion preOriginRot = _originRot;

        float elapsed_s = 0f;

        while (elapsed_s < _resetDuration)
        {
            elapsed_s += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed_s / _resetDuration);
            _originRot = Quaternion.Slerp(preOriginRot, newOriginRot, t);
            yield return null;
        }

        _originRot = newOriginRot;
        _isResetting = false;
    }

    private void Update()
    {
        UpdateOrientation();
    }

    void UpdateOrientation()
    {
        bool isConnectedJoycon = _joyconInputManager != null && _joyconInputManager.IsConnected;

        if (isConnectedJoycon)
        {
            _joyconWandController.NewWandRot(this,_wand);
        }
        else
        {
            _keyWandController.NewWandRot(this, _wand);
        }
    }
}
