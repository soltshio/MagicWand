using Cysharp.Threading.Tasks;
using System;
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

    public event Action <FadeInOutEState> OnChangeState;

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
        if(_fadeState == FadeInOutEState.CompleteFadeIn && isFadeIn) return;//フェードインが完了しているのにフェードインをしようとしたとき

        if (_fadeState == FadeInOutEState.CompleteFadeOut && !isFadeIn) return;//フェードアウトが完了しているのにフェードアウトをしようとしたとき

        //フェードイン・アウトの最中であれば今行っているフェード処理を中断して新しくフェード処理を開始する
        if (_fadeState == FadeInOutEState.FadingIn || _fadeState == FadeInOutEState.FadingOut)
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        //トークンを生成
        _cts = new CancellationTokenSource();
        
        //フェード処理開始(後で呼び出す処理を作成予定)
        FadeAsync(isFadeIn, _cts.Token).Forget();
    }

    void Start()
    {
        //開始時にパネルの色の透明度をあらかじめ変えておく
        Color currentMyPanelColor = _myPanelImage.color;

        currentMyPanelColor.a = _isInitHide ? 1f : 0f;
        _fadeState = _isInitHide ? FadeInOutEState.CompleteFadeOut : FadeInOutEState.CompleteFadeIn;

        _myPanelImage.color = currentMyPanelColor;
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    //フェード処理
    async UniTask FadeAsync(bool isFadeIn, CancellationToken ct)
    {
        //トークンの生成
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, this.GetCancellationTokenOnDestroy());

        try
        {
            //フェード状態の更新
            _fadeState = isFadeIn ? FadeInOutEState.FadingIn : FadeInOutEState.FadingOut;

            //透明度の更新に必要な変数の準備
            float targetAlpha = isFadeIn ? 0f : 1f;
            float beforeAlpha = _myPanelImage.color.a;
            float alphaDeltaFromCurrentToTarget = Mathf.Abs(targetAlpha - beforeAlpha);

            float elapsedTime = 0f;
            float fadeDuration = _fadeInOutDuration * alphaDeltaFromCurrentToTarget;//現在の透明度から目標の透明度までの差に応じてフェードにかける時間を変える

            while (elapsedTime < fadeDuration)
            {
                //透明度変更
                float newAlpha = Mathf.Lerp(beforeAlpha, targetAlpha, elapsedTime / fadeDuration);
                SetPanelAlpha(newAlpha);

                await UniTask.Yield(PlayerLoopTiming.Update, linkedCts.Token);

                elapsedTime += Time.deltaTime;
            }

            //目標の透明度にする
            SetPanelAlpha(targetAlpha);

            //フェード状態の更新
            _fadeState = isFadeIn ? FadeInOutEState.CompleteFadeIn : FadeInOutEState.CompleteFadeOut;
        }
        catch(OperationCanceledException)
        {

        }
    }

    void SetPanelAlpha(float alpha)
    {
        Color currentMyPanelColor = _myPanelImage.color;
        currentMyPanelColor.a = alpha;
        _myPanelImage.color = currentMyPanelColor;
    }
}
