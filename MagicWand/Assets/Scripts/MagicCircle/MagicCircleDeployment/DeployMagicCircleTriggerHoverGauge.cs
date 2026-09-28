using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//魔法陣展開のホバーゲージ

[System.Serializable]
public class DeployMagicCircleTriggerHoverGauge
{
    [SerializeField]
    Transform _hoverGauge;

    [SerializeField]
    float _maxLocalScale = 0.1f;

    public void SetGauge(float progress)
    {
        float scale = Mathf.Lerp(0, _maxLocalScale, progress);

        Vector3 newLocalScale = new Vector3(scale, scale, scale);

        _hoverGauge.localScale = newLocalScale;
    }
}
