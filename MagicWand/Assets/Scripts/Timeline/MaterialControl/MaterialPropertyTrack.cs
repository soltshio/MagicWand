using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(Renderer))]
[TrackClipType(typeof(MaterialPropertyClip))]
public class MaterialPropertyTrack : TrackAsset
{
    public override Playable CreateTrackMixer(
        PlayableGraph graph,
        GameObject go,
        int inputCount)
    {
        return ScriptPlayable<MaterialPropertyMixerBehaviour>
            .Create(graph, inputCount);
    }
}