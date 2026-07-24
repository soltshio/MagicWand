using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MaterialPropertyClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField]
    private string propertyName = "_Dissolve";

    [SerializeField]
    private float startValue = 0f;

    [SerializeField]
    private float endValue = 1f;

    public ClipCaps clipCaps =>
        ClipCaps.Blending |
        ClipCaps.ClipIn |
        ClipCaps.SpeedMultiplier;

    public override Playable CreatePlayable(
        PlayableGraph graph,
        GameObject owner)
    {
        var playable =
            ScriptPlayable<MaterialPropertyBehaviour>.Create(graph);

        var behaviour = playable.GetBehaviour();

        behaviour.PropertyID = Shader.PropertyToID(propertyName);
        behaviour.StartValue = startValue;
        behaviour.EndValue = endValue;

        return playable;
    }
}