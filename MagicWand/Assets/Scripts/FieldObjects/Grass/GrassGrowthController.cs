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

        public AnimationClip leaveAnimationClip;

        [Range(0, 1)] public float startRate;

        [Range(0, 1)] public float finishRate;
    }

    [SerializeField]
    LeaveGrowthSegment[] _leaveGrowthSegments;

    [SerializeField]
    Renderer _stemRenderer;

    [SerializeField] [Range(0, 1)]
    float _defaultGrowthRate;

    [SerializeField]
    float _stemHeight;

    float _currentGrowthRate;

    static readonly int _displayRateID = Shader.PropertyToID("_DisplayRate");
    static readonly int _heightID = Shader.PropertyToID("_Height");

    public float CurrentGrowthRate { get { return _currentGrowthRate; } }

    void Start()
    {
        SetGrowth(_defaultGrowthRate);
    }

    private void OnValidate()
    {
        SetGrowth(_defaultGrowthRate);
    }

    //1を設定すると最大まで成長させる
    public void SetGrowth(float newGrowthRate)
    {
        newGrowthRate = Mathf.Clamp01(newGrowthRate);
        _currentGrowthRate = newGrowthRate;

        //葉っぱの成長
        SetLeaveGrowth(newGrowthRate);

        //茎の成長
        SetStemGrowth(newGrowthRate);

        //茎の高さを設定
        SetStemHeight();
    }

    //現在の茎のMaterialの設定をリセットする
    [ContextMenu("Clear Material Property Block")]
    void ClearMaterialPropertyBlock()
    {
        if (_stemRenderer == null) return;

        _stemRenderer.SetPropertyBlock(null);
    }



    void SetLeaveGrowth(float growthRate)
    {
        for (int i = 0; i < _leaveGrowthSegments.Length; i++)
        {
            var growthSegment = _leaveGrowthSegments[i];

            if (growthSegment.leaveRenderer == null) continue;
            if (growthSegment.leaveAnimationClip == null) continue;
            if (growthSegment.startRate == growthSegment.finishRate) continue;//ゼロ除算を防ぐためにスキップする

            //その成長セグメントに達していない場合は非表示にする
            if (growthRate < growthSegment.startRate)
            {
                growthSegment.leaveRenderer.enabled = false;
                continue;
            }

            growthSegment.leaveRenderer.enabled = true;

            //どのくらい成長させるかを決める
            float range = growthSegment.finishRate - growthSegment.startRate;
            float time = Mathf.Clamp01((growthRate - growthSegment.startRate) / range);
            time *= growthSegment.leaveAnimationClip.length;

            //葉っぱの大きさや位置をアニメーションクリップから設定
            growthSegment.leaveAnimationClip.SampleAnimation(gameObject, time);
        }
    }

    void SetStemGrowth(float rate)
    {
        SetStemMaterialPropertyBlock(_displayRateID, rate);
    }

    void SetStemHeight()
    {
        SetStemMaterialPropertyBlock(_heightID, _stemHeight);
    }

    void SetStemMaterialPropertyBlock(int propertyID,float value)
    {
        if (_stemRenderer == null) return;

        MaterialPropertyBlock block = new MaterialPropertyBlock();

        _stemRenderer.GetPropertyBlock(block);

        block.SetFloat(propertyID, value);

        _stemRenderer.SetPropertyBlock(block);
    }
}


