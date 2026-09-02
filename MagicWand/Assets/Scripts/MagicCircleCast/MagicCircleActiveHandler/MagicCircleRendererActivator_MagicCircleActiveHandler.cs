using UnityEngine;

//作成者:杉山
//MagicCircleActiveHandlerの魔法陣の表示・非表示の処理をする

[System.Serializable]
public class MagicCircleRendererActivator_MagicCircleActiveHandler
{
    [SerializeField]
    Animator _magicCircleAnimator;

    //初期状態では非表示にしておく
    public void Start()
    {
        _magicCircleAnimator.gameObject.SetActive(false);
    }

    public void Show()
    {
        _magicCircleAnimator.gameObject.SetActive(true);
        _magicCircleAnimator.SetBool(MagicCircleAnimatorProperty.IsVisibleBoolName, true);
    }

    public void Hide()
    {
        _magicCircleAnimator.SetBool(MagicCircleAnimatorProperty.IsVisibleBoolName, false);
    }
}
