using UnityEngine;
using UnityEngine.InputSystem;

//作成者：杉山
//魔法陣をなぞるカーソルの動き
//レイを飛ばして、魔法陣上の球との当たり判定をとる

public class MagicCircleTracerCursor : MonoBehaviour
{
    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit)) return;

        if (!hit.collider.CompareTag(TagNameList.MagicSphere)) return;

        var magicSphere = hit.collider.GetComponent<MagicSphereVer3>();
        magicSphere.ToDeactive();
    }
}
