using UnityEngine;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンの共有情報

[System.Serializable]
public class HoverAutoClickButtonParameter
{
    [Tooltip("何秒間カーソルを合わせていると自動クリックされるか")] [SerializeField]
    float _hoverDurationToClick = 2f;

    [Tooltip("クリックの継続時間")] [SerializeField]
    private float _pressDuration = 0.1f;

    [Tooltip("ホバー時のオーディオソース")] [SerializeField]
    AudioSource _hoveringAudioSource;

    [SerializeField]
    GameObject _buttonUIObject;

    public float HoverDurationToClick { get => _hoverDurationToClick; }//何秒間カーソルを合わせていると自動クリックされるか
    public float PressDuration { get => _pressDuration; }//クリックの継続時間
    public AudioSource HoveringAudioSource { get => _hoveringAudioSource; }//ホバー時のオーディオソース
    public GameObject ButtonUIObject { get => _buttonUIObject; }//ボタンのUIオブジェクト
}
