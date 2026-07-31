using UnityEngine;

//作成者:杉山
//MagicCircleActiveHandlerの通った球をつなぐ線の表示・非表示の処理をする

[System.Serializable]
public class MagicTrailRendererActivator_MagicCircleActiveHandler
{
    [Tooltip("魔法陣の線の描画機能")] [SerializeField]
    LineRenderer _magicCircleTrailRenderer;

    public void Start()
    {
        _magicCircleTrailRenderer.enabled = false;
    }

    public void MagicTrailSwitchEnable(bool isEnable)
    {
        _magicCircleTrailRenderer.enabled = isEnable;
    }
}
