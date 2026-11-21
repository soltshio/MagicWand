using UnityEngine;

//作成者：杉山
//地面の乾き度を操作する機能

public class GroundDryController : MonoBehaviour
{
    [Tooltip("乾き度の初期値")] [SerializeField] [Range(0,1)]
    float _defaultDryRate;

    [Tooltip("湿る際にどれだけ乾き度を変化させるか(減少させるか)")] [SerializeField] [Range(0, 1)]
    float _getWetDelta;

    [Tooltip("乾く際にどれだけ渇き度を変化させるか(増加させるか)")] [SerializeField] [Range(0, 1)]
    float _getDryDelta;

    [SerializeField]
    MeshRenderer _groundRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
