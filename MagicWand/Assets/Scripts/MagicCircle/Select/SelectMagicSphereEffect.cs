using UnityEngine;

//作成者:杉山
//魔法選択時の魔法球のエフェクト

public class SelectMagicSphereEffect : MonoBehaviour
{
    [SerializeField]
    MagicSphereMaterialController _magicSphereMaterialController;

    [SerializeField]
    HoverTouchedReceiver _hoverTouchReceiver;

    [SerializeField]
    AudioSource _hoverSE_AudioSource;

    private void OnEnable()
    {
        //メソッドを登録
        _hoverTouchReceiver.OnTouchedEnter += OnTouchedEnter;
        _hoverTouchReceiver.OnTouchedExit += OnTouchedExit;

        //マークのゲージの表示度をリセットしておく
        _magicSphereMaterialController.SetInMarkGaugeFillAmount(0f);
    }

    private void OnDisable()
    {
        //メソッドの登録を解除
        _hoverTouchReceiver.OnTouchedEnter -= OnTouchedEnter;
        _hoverTouchReceiver.OnTouchedExit -= OnTouchedExit;

        _hoverSE_AudioSource.Stop();
    }

    void Update()
    {
        UpdateMarkGaugeFillAmount();
    }

    //カーソルが_hoverTouchReceiverに触れている時のみ、選択の進捗度(どれだけ魔法球に触れ続けたか)をマークの表示度に反映する
    void UpdateMarkGaugeFillAmount()
    {
        if (!_hoverTouchReceiver.IsTouched) return;

        float progress = _hoverTouchReceiver.HoverProgress;

        _magicSphereMaterialController.SetInMarkGaugeFillAmount(progress);
    }

    //_hoverTouchReceiverにカーソルが触れ始めた時に呼ぶ
    void OnTouchedEnter()
    {
        _hoverSE_AudioSource.Play();
    }

    //_hoverTouchReceiverにカーソルが触れ始めた時に呼ぶ
    void OnTouchedExit()
    {
        _hoverSE_AudioSource.Stop();

        //マークのゲージの表示度をリセットしておく
        _magicSphereMaterialController.SetInMarkGaugeFillAmount(0f);
    }
}
