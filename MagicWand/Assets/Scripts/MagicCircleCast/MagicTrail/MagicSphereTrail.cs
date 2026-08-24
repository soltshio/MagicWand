using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//魔法陣の描いた線を描画する

public class MagicSphereTrail : MonoBehaviour
{
    [SerializeField]
    LineRenderer _lineRenderer;

    [ColorUsage(true, true)]
    public Color _activeEmissionColor;

    [ColorUsage(true, true)]
    public Color _deactiveEmissionColor;

    static readonly int _baseEmissionColorID = Shader.PropertyToID("_BaseEmissionColor");
    static readonly int _alphaClipThresholdID = Shader.PropertyToID("_AlphaClipThreshold");

    const float _visibleAlphaClipThreshold = 0f;
    const float _invisibleAlphaClipThreshold = 1f;

    private List<Vector3> points = new List<Vector3>();

    Material _lineMat;

    void Awake()
    {
        _lineMat = _lineRenderer.material;
    }

    void Start()
    {
        ResetTrail();
    }

    public void Add(Vector3 pointLocalPos)
    {
        points.Add(pointLocalPos);

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }

    public void ResetTrail()
    {
        points.Clear();

        _lineRenderer.positionCount = 0;

        //色を目立たない色にする
        _lineMat.SetColor(_baseEmissionColorID,_deactiveEmissionColor);
        //透明度を見える状態にする
        _lineMat.SetFloat(_alphaClipThresholdID, 0f);
    }

    public void Activate()
    {
        //色を光らせる
        _lineMat.SetColor(_baseEmissionColorID, _activeEmissionColor);
    }

    public async UniTask HideAsync(float fadeOutDuration)
    {
        var ct = this.GetCancellationTokenOnDestroy();

        ProgressTimer progressTimer = new(fadeOutDuration);

        while (!progressTimer.IsFinished)
        {
            progressTimer.Tick();

            float progress = progressTimer.CalcProgress();

            float newAlphaClipThreshold = Mathf.Lerp(_visibleAlphaClipThreshold, _invisibleAlphaClipThreshold, progress);
            _lineMat.SetFloat(_alphaClipThresholdID, newAlphaClipThreshold);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }

        _lineMat.SetFloat(_alphaClipThresholdID, _invisibleAlphaClipThreshold);
    }
}
