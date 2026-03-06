using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//作成者:杉山
//杖を制御するクラス

public class WandController : MonoBehaviour
{
    [SerializeField]
    MovingAveragedJoyconOrientation _movingAveragedJoyconOrientation;

    [Tooltip("杖(動かす対象)")] [SerializeField]
    Transform _wand;

    [Tooltip("リセットにかかる時間")] [SerializeField]
    float _resetDuration = 1f;

    Quaternion _originJoyconOrientation = Quaternion.identity;

    Quaternion _currentRot=Quaternion.identity;

    bool _isResetting = false;

    public void ResetAiming(InputAction.CallbackContext context)//照準をリセット
    {
        if (!context.performed) return;

        if (_isResetting) return;

        var newOriginJoyconOrientation = _movingAveragedJoyconOrientation.SmoothedOrientation * Quaternion.AngleAxis(90f,Vector3.right);

        StartCoroutine(ResetAimingCoroutine(newOriginJoyconOrientation));
    }

    IEnumerator ResetAimingCoroutine(Quaternion newOriginJoyconOrientation)
    {
        _isResetting = true;
        Quaternion preOriginJoyconOrientation = _originJoyconOrientation;

        float elapsed_s= 0f;

        while (elapsed_s < _resetDuration)
        {
            elapsed_s += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed_s / _resetDuration);
            _originJoyconOrientation = Quaternion.Slerp(preOriginJoyconOrientation, newOriginJoyconOrientation, t);
            yield return null;
        }

        _originJoyconOrientation = newOriginJoyconOrientation;
        _isResetting = false;
    }

    private void Awake()
    {
        _currentRot = Quaternion.identity;
    }

    private void Update()
    {
        Quaternion newRot;

        var joyconOrientation = _movingAveragedJoyconOrientation.SmoothedOrientation;

        //基準の回転との計算
        joyconOrientation = Quaternion.Inverse(_originJoyconOrientation) * joyconOrientation;

        //y軸回転とz軸回転を入れ替える
        Quaternion c = Quaternion.AngleAxis(90f, Vector3.right);

        newRot = c * joyconOrientation * Quaternion.Inverse(c);

        //杖を回転させる
        _currentRot = newRot;

        _wand.localRotation = _currentRot;
    }
}
