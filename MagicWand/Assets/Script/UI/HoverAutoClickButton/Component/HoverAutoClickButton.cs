using UnityEngine;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタン

public class HoverAutoClickButton : MonoBehaviour
{
    [Tooltip("何秒間カーソルを合わせていると自動クリックされるか")] [SerializeField]
    private float _hoverDurationToClick = 2f;

    const float _pressDuration = 0.1f;

    public float HoverDurationToClick { get => _hoverDurationToClick; }//何秒間カーソルを合わせていると自動クリックされるか

   

}
