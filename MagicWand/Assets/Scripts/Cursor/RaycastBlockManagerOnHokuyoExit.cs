using UnityEngine;

//作成者:杉山
//北陽レーザー検知範囲内に物が無くなった際にレイキャストをブロックする機能

public class RaycastBlockManagerOnHokuyoExit : MonoBehaviour
{
    [Tooltip("北陽レーザー検知範囲内にオブジェクトが無くなってから、UIを反応させなくするまでの時間")] [SerializeField]
    float _timeOutToBlockRaycast = 0.5f;

    HokuyoDataReceiver _hokuyoDataReceiver;

    SingleTaskCancellation _singleTaskCancellation = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
