using System;
using UnityEngine;

//作成者:杉山
//草の成長コントローラー

public class GrassGrowthController : MonoBehaviour
{
    [System.Serializable]
    struct GrowthSegment//成長区間
    {
        public GameObject leaveObject;

        [Range(0, 1)] public float startRate;

        [Range(0, 1)] public float rangeRate;
    }

    [SerializeField]
    GrowthSegment[] _growthSegments;

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
        for(int i=0; i<_growthSegments.Length ;i++)
        {
            var growthSegment = _growthSegments[i];

            if (growthSegment.leaveObject == null) continue;

            //その成長セグメントに達していない場合は非表示にする
            if(rate<growthSegment.startRate)
            {
                growthSegment.leaveObject.SetActive(false);
                continue;
            }

            growthSegment.leaveObject.SetActive(true);

            //位置を設定
            float heightRate = Mathf.Clamp(rate, growthSegment.startRate, growthSegment.startRate + growthSegment.rangeRate);
            
            float leavePosY = heightRate * _height;
            var leaveLocalPos = growthSegment.leaveObject.transform.localPosition;
            leaveLocalPos.y = leavePosY;
            growthSegment.leaveObject.transform.localPosition = leaveLocalPos;

            //大きさを設定
            float scaleRate = Mathf.InverseLerp(growthSegment.startRate, growthSegment.startRate + growthSegment.rangeRate, rate);
            scaleRate = Mathf.Clamp01(scaleRate);

            Vector3 leaveScale = new Vector3(scaleRate, scaleRate, scaleRate);
            growthSegment.leaveObject.transform.localScale=leaveScale;
        }

        //茎の成長
        if (_stemRenderer == null) return;

        MaterialPropertyBlock block = new MaterialPropertyBlock();

        _stemRenderer.GetPropertyBlock(block);

        block.SetFloat(_displayRateID, rate);

        _stemRenderer.SetPropertyBlock(block);
    }
}


