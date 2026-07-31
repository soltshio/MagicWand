using System;
using UnityEngine;

//作成者:杉山
//草の成長コントローラー

public class GrassGrowthController : MonoBehaviour
{
    [System.Serializable]
    struct LeaveGrowthSegment//葉っぱの成長区間
    {
        public Renderer leaveRenderer;

        [Range(0, 1)] public float startRate;

        [Range(0, 1)] public float rangeRate;
    }

    [SerializeField]
    LeaveGrowthSegment[] _leaveGrowthSegments;

    [SerializeField]
    Renderer _stemRenderer;

    [SerializeField]
    float _height;

    [SerializeField] [Range(0, 1)]
    float _defaultGrowthRate;

    static readonly int _displayRateID = Shader.PropertyToID("_DisplayRate");

    void Start()
    {
        SetGrowth(_defaultGrowthRate);
    }

    private void OnValidate()
    {
        SetGrowth(_defaultGrowthRate);
    }

    //1を設定すると最大まで成長させる
    public void SetGrowth(float rate)
    {
        rate = Mathf.Clamp01(rate);

        //葉っぱの成長
        SetLeaveGrowth(rate);

        //茎の成長
        SetStemGrowth(rate);
    }

    void SetLeaveGrowth(float rate)
    {
        for (int i = 0; i < _leaveGrowthSegments.Length; i++)
        {
            var growthSegment = _leaveGrowthSegments[i];

            if (growthSegment.leaveRenderer == null) continue;

            //その成長セグメントに達していない場合は非表示にする
            if (rate < growthSegment.startRate)
            {
                growthSegment.leaveRenderer.enabled = false;
                continue;
            }

            growthSegment.leaveRenderer.enabled = true;
            //位置を設定
            float heightRate = Mathf.Clamp(rate, growthSegment.startRate, growthSegment.startRate + growthSegment.rangeRate);

            float leavePosY = heightRate * _height;
            var leaveLocalPos = growthSegment.leaveRenderer.transform.localPosition;
            leaveLocalPos.y = leavePosY;
            growthSegment.leaveRenderer.transform.localPosition = leaveLocalPos;

            //大きさを設定
            float scaleRate = Mathf.InverseLerp(growthSegment.startRate, growthSegment.startRate + growthSegment.rangeRate, rate);
            scaleRate = Mathf.Clamp01(scaleRate);

            Vector3 leaveScale = new Vector3(scaleRate, scaleRate, scaleRate);
            growthSegment.leaveRenderer.transform.localScale = leaveScale;
        }
    }

    void SetStemGrowth(float rate)
    {
        if (_stemRenderer == null) return;

        MaterialPropertyBlock block = new MaterialPropertyBlock();

        _stemRenderer.GetPropertyBlock(block);

        block.SetFloat(_displayRateID, rate);

        _stemRenderer.SetPropertyBlock(block);
    }
}


