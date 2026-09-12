using UnityEngine;

public class TestGrass : MonoBehaviour
{
    [SerializeField]
    AnimationClip _aniClip1;

    [SerializeField]
    AnimationClip _aniClip2;

    [SerializeField] [Range(0,1)]
    float _normalizedTime1;

    [SerializeField] [Range(0, 1)]
    float _normalizedTime2;


    private void OnValidate()
    {
        if (_aniClip1 == null) return;
        if (_aniClip2 == null) return;

        float time1 = _aniClip1.length * _normalizedTime1;

        _aniClip1.SampleAnimation(gameObject, time1);

        float time2 = _aniClip1.length * _normalizedTime2;

        _aniClip2.SampleAnimation(gameObject, time2);
    }
}
