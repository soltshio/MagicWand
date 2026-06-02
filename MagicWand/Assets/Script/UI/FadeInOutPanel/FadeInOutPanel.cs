using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//作成者:杉山
//フェードイン・アウトするパネル

[RequireComponent(typeof(Image))]
public class FadeInOutPanel : MonoBehaviour
{
    [Tooltip("シーン開始時点でパネルで隠しておくか")] [SerializeField] 
    bool _isInitHide = false;

    [Tooltip("フェードイン・アウトにかける時間")] [SerializeField] 
    float _fadeInOutDuration = 1f;

    [Tooltip("隠す時に使うパネル")] [SerializeField]
    Image _myPanelImage;

    CancellationTokenSource _cts;

    FadeInOutEState _fadeState;

    public float FadeInOutDuration
    {
        get { return _fadeInOutDuration; }
        set { _fadeInOutDuration = value; }
    }

    //フェードイン・アウトを開始する
    //引数isFadeInがtrueのときはフェードイン、falseのときはフェードアウト
    public void FadeTrigger(bool isFadeIn)
    {
        //既に完了していた場合は弾く
        if(_fadeState == FadeInOutEState.CompleteFadeIn && isFadeIn)//フェードインが完了しているのにフェードインをしようとしたとき
            return;
        if(_fadeState == FadeInOutEState.CompleteFadeOut && !isFadeIn)//フェードアウトが完了しているのにフェードアウトをしようとしたとき
            return;

        //フェードイン・アウトの最中であれば今行っているフェード処理を中断して新しくフェード処理を開始する
        if(_fadeState == FadeInOutEState.FadingIn || _fadeState == FadeInOutEState.FadingOut)
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        //トークンを生成
        _cts = new CancellationTokenSource();
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token,this.GetCancellationTokenOnDestroy());

        //フェード処理開始(後で呼び出す処理を作成予定)
    }

    void Start()
    {
        //開始時にパネルの色の透明度をあらかじめ変えておく
        Color currentMyPanelColor = _myPanelImage.color;

        currentMyPanelColor.a = _isInitHide ? 1f : 0f;
        _fadeState = _isInitHide ? FadeInOutEState.CompleteFadeOut : FadeInOutEState.CompleteFadeIn;

        _myPanelImage.color = currentMyPanelColor;
    }

    //フェード処理
    async UniTask FadeAsync(bool isFadeIn, CancellationToken ct)
    {
        //後でフェードインアウトの実装予定
    }
}
