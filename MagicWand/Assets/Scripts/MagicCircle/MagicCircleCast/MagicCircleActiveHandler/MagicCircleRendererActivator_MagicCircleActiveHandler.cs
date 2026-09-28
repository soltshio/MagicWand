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

    //魔法陣を隠し始める
    public void StartHide()
    {
        _magicCircleAnimator.SetBool(MagicCircleAnimatorProperty.IsVisibleBoolName, false);
    }

    //魔法陣を完全に隠す(収納アニメーションをさせてから隠す)
    public void CompleteHide()
    {
        _magicCircleAnimator.gameObject.SetActive(false);
    }
}
