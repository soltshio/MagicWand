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

        public float startRate;

        public float rangeRate;
    }

    [SerializeField]
    GrowthSegment[] _growthSegments;

    [SerializeField]
    Renderer _stemRenderer;

    [SerializeField]
    float _height;

    [SerializeField] [Range(0, 1)]
    float _defaultGrowthRate;

    void Start()
    {
        SetGrowth(_defaultGrowthRate);
    }

    private void OnValidate()
    {
        SetGrowth(_defaultGrowthRate);
    }

    public void SetGrowth(float rate)
    {
        rate = Mathf.Clamp01(rate);

        //葉っぱの成長
        for(int i=0; i<_growthSegments.Length ;i++)
        {
            var growthSegment = _growthSegments[i];

            if (growthSegment.leaveObject == null) continue;
        }

        //茎の成長
        MaterialPropertyBlock block = new MaterialPropertyBlock();
    }
}


