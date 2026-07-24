using UnityEngine;
using UnityEngine.InputSystem;

//作成者：杉山
//魔法陣をなぞるカーソルの動き
//レイを飛ばして、魔法陣上の球との当たり判定をとる

public class MagicCircleTracerCursor : MonoBehaviour
{
    HokuyoBlobPosReceiver _hokuyoBlobPosReceiver;

    void Awake()
    {
        var oscReceiver = GameObject.FindWithTag(TagNameList.OSCReceiver);
        _hokuyoBlobPosReceiver = oscReceiver.GetComponent<HokuyoBlobPosReceiver>();

        if(_hokuyoBlobPosReceiver==null)
        {
            Debug.Log("北陽レーザーの位置情報レシーバーの取得に失敗");
        }
    }

    void Update()
    {
        if (_hokuyoBlobPosReceiver == null) return;

        //OSC通信が動いているかつ、北陽レーザーの検知範囲内にオブジェクトが無い
        if ( _hokuyoBlobPosReceiver.IsRunning && !_hokuyoBlobPosReceiver.IsExistObject) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit)) return;

        if (!hit.collider.CompareTag(TagNameList.MagicSphere)) return;

        var magicSphere = hit.collider.GetComponent<MagicSphereVer3>();
        magicSphere.ToDeactive();
    }
}
