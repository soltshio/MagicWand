using UnityEngine;
using UnityEngine.Playables;

public class MaterialPropertyMixerBehaviour : PlayableBehaviour
{
    private MaterialPropertyBlock block;

    public override void ProcessFrame(
        Playable playable,
        FrameData info,
        object playerData)
    {
        if (playerData is not Renderer renderer)
            return;

        block ??= new MaterialPropertyBlock();

        renderer.GetPropertyBlock(block);

        int inputCount = playable.GetInputCount();

        for (int i = 0; i < inputCount; i++)
        {
            float weight = playable.GetInputWeight(i);

            if (weight <= 0f)
                continue;

            var input =
                (ScriptPlayable<MaterialPropertyBehaviour>)
                playable.GetInput(i);

            var behaviour = input.GetBehaviour();

            float t = 1f;

            if (input.GetDuration() > 0)
            {
                t = Mathf.Clamp01(
                    (float)(input.GetTime() / input.GetDuration()));
            }

            float value = Mathf.Lerp(
                behaviour.StartValue,
                behaviour.EndValue,
                t);

            block.SetFloat(
                behaviour.PropertyID,
                value);
        }

        renderer.SetPropertyBlock(block);
    }
}