using UnityEngine;

//作成者:杉山
//誘導エフェクト

public class LeadEffect : MonoBehaviour
{
    Vector3 _start;
    Vector3 _end;

    public void Initialize(Vector3 start,Vector3 end)
    {
        _start = start;
        _end = end;

        transform.position = start;
    }

    //progressが0の時はstart、1の時はendの位置になるように移動させる
    public void SetPos(float progress)
    {
        transform.position = Vector3.Lerp(_start, _end, progress);
    }
}
