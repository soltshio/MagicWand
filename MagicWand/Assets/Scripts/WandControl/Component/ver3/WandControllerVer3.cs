using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//作成者:杉山
//杖を動かすクラス(バージョン3)
//常にジャイロオン
//ひっくり返っているかは考慮しない
//回転速度を調整可能

public class WandControllerVer3 : MonoBehaviour
{
    [SerializeField]
    JoyconInputManager _joyconInputManager;

    [SerializeField]
    Vector2 _rotationSpeed;

    [Tooltip("杖(動かす対象)")] [SerializeField]
    Transform _wand;

    [Tooltip("リセットにかかる時間")] [SerializeField]
    float _resetDuration = 1f;

    bool _isResetting = false;

    public void ResetAiming(InputAction.CallbackContext context)//照準をリセット
    {
        if (!context.performed) return;

        if (_isResetting) return;//リセット中は無視

        StartCoroutine(ResetAimingCoroutine());
    }

    IEnumerator ResetAimingCoroutine()
    {
        _isResetting = true;
        Quaternion preOriginRot = _wand.localRotation;

        float elapsed_s = 0f;

        while (elapsed_s < _resetDuration)
        {
            elapsed_s += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed_s / _resetDuration);
            _wand.localRotation = Quaternion.Slerp(preOriginRot, Quaternion.identity, t);
            yield return null;
        }

        _wand.localRotation = Quaternion.identity;
        _isResetting = false;
    }

    private void Update()
    {
        UpdateOrientation();
    }

    void UpdateOrientation()
    {
        bool isConnectedJoycon = _joyconInputManager != null && _joyconInputManager.IsConnected;

        if(!isConnectedJoycon) return;

        //x軸の回転(上下方向)
        float angleX = -_joyconInputManager.Gyro.y * _rotationSpeed.y * Time.deltaTime;
        Quaternion xRot = Quaternion.AngleAxis(angleX, _wand.parent.right);

        //y軸の回転(左右方向)
        float angleY = _joyconInputManager.Gyro.z * _rotationSpeed.x * Time.deltaTime;
        Quaternion yRot = Quaternion.AngleAxis(angleY, _wand.parent.up);

        //回転を加える
        _wand.localRotation *= yRot * xRot;
    }
}
