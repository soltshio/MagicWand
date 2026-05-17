using UnityEngine;
using UnityEngine.InputSystem;

//キー操作でマウスカーソルを動かすテスト
//現在の座標はビューポート座標で管理し、位置を書き換える時にスクリーン座標に変換

public class KeyControlMouseCursorTest : MonoBehaviour
{
    [SerializeField]
    Vector2 _speed;

    Vector2 _currentPos;

    bool _controlEnabled = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentPos = new Vector2(0.5f, 0.5f);

        var mainCamera = Camera.main;
        Vector2 currentSpreenPos = mainCamera.ViewportToScreenPoint(_currentPos);

        Mouse.current.WarpCursorPosition(currentSpreenPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_controlEnabled) return;

        if(Keyboard.current.escapeKey.isPressed)
        {
            _controlEnabled = false;
        }


        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            input.y += 1;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1;

        _currentPos += input * _speed * Time.deltaTime;

        _currentPos.x = Mathf.Clamp01(_currentPos.x);
        _currentPos.y = Mathf.Clamp01(_currentPos.y);

        var mainCamera = Camera.main;
        Vector2 currentScreenPos = mainCamera.ViewportToScreenPoint(_currentPos);

        Mouse.current.WarpCursorPosition(currentScreenPos);
    }
}
