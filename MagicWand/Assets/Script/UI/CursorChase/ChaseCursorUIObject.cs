using UnityEngine;
using UnityEngine.InputSystem;

//作成者:杉山
//カーソルを追いかけるUIオブジェクト

public class ChaseCursorUIObject : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        mouseScreenPos.x -= screenWidth / 2f;
        mouseScreenPos.x = Mathf.Clamp(mouseScreenPos.x, -screenWidth / 2f, screenWidth / 2f);

        mouseScreenPos.y -= screenHeight / 2f;
        mouseScreenPos.y = Mathf.Clamp(mouseScreenPos.y, -screenHeight / 2f, screenHeight / 2f);

        transform.localPosition = mouseScreenPos;
    }
}
