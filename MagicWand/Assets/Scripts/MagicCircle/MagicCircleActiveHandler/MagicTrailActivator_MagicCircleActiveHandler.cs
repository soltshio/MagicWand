using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//MagicCircleActiveHandlerの通った球をつなぐ線の表示・非表示の処理をする

[System.Serializable]
public class MagicTrailRendererActivator_MagicCircleActiveHandler
{
    [Tooltip("魔法陣の線")] [SerializeField]
    MagicSphereTrail _magicSphereTrail;

    public void HideMagicTrail(float fadeDuration)
    {
        _magicSphereTrail.HideAsync(fadeDuration).Forget();
    }
}
