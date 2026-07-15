using UnityEngine;
using UnityEngine.Playables;

public class MaterialPropertyMixerBehaviour : PlayableBehaviour
{
    private MaterialPropertyBlock _block;
    Renderer _renderer;

    public override void ProcessFrame(Playable playable,FrameData info,object playerData)
    {
        _renderer = playerData as Renderer;

        if (_renderer == null) return;

        if (_block == null)
        {
            _block = new MaterialPropertyBlock();
        }

        _renderer.GetPropertyBlock(_block);

        int inputCount = playable.GetInputCount();

        for (int i = 0; i < inputCount; i++)
        {
            float weight = playable.GetInputWeight(i);

            if (weight <= 0f) continue;
            
            var input =(ScriptPlayable<MaterialPropertyBehaviour>)playable.GetInput(i);

            var behaviour = input.GetBehaviour();

            float t = 1f;

            if (input.GetDuration() > 0)
            {
                t = Mathf.Clamp01((float)(input.GetTime() / input.GetDuration()));
            }

            float value = Mathf.Lerp(behaviour.StartValue,behaviour.EndValue,t);

            _block.SetFloat(behaviour.PropertyID,value);
        }

        _renderer.SetPropertyBlock(_block);
    }

    public override void OnGraphStop(Playable playable)
    {
        if(_renderer!=null) _renderer.SetPropertyBlock(null);
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        if (_renderer != null) _renderer.SetPropertyBlock(null);
    }
}