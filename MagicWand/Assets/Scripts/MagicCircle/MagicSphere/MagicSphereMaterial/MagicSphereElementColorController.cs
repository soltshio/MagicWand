using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//魔法球に宿る属性に応じて色を変える

public class MagicSphereElementColorController : MonoBehaviour
{
    [SerializeField]
    float _fadeDuration=0.5f;

    [SerializeField]
    DefaultMagicSphereMaterialProperty _defaultMaterialProperty;

    [SerializeField]
    MagicSphereMaterialController _magicSphereMaterialController;

    //elementProgress関係
    float _activeProgress = 0;//これが0になるほど無属性に、1になるほど属性の色に近づいているということにする
    const float _activeProgress_Deactivated = 0f;
    const float _activeProgress_Activated = 1f;

    //markAlphaClipThreshold関係
    const float _activeMarkAlphaClipThreshold = 0f;
    const float _deactiveMarkAlphaClipThreshold = 1f;

    SingleTaskCancellation _singleTaskCancellation = new();

    public async UniTask ToActiveAsync(MagicSphereMaterialProperty activeMaterialProperty)
    {
        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        //この処理開始時点での_elementProgressの値を記憶
        float preElementProgress = _activeProgress;

        //最初に何秒かけてだんだん色を変えていくかを決める
        float fadeDuration = (_activeProgress_Activated - _activeProgress) * _fadeDuration;

        //変化前のマテリアルのプロパティを取得しておく
        GetCurrentMaterialProperty(out var preMagicSphereMaterialProperty, out float preMarkAlphaClipThreshold);

        //最初にテクスチャを変えておく
        _magicSphereMaterialController.SetTexture(activeMaterialProperty);

        ProgressTimer progressTimer = new(fadeDuration);

        while (!progressTimer.IsFinished)
        {
            progressTimer.Tick();

            float progress = progressTimer.CalcProgress();

            _activeProgress = Mathf.Lerp(preElementProgress,_activeProgress_Activated,progress);

            //色を変更
            MagicSphereMaterialProperty newProperty = MagicSphereMaterialProperty.ColorLerp(preMagicSphereMaterialProperty, activeMaterialProperty, progress);
            _magicSphereMaterialController.SetColor(newProperty);

            //マークの透明度を変更
            float newMarkAlphaClipThreshold = Mathf.Lerp(preMarkAlphaClipThreshold,_activeMarkAlphaClipThreshold,progress);
            _magicSphereMaterialController.SetMarkAlphaClipThreshold(newMarkAlphaClipThreshold);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }
    }

    //球を何の属性も無い色にする
    public async UniTask ToDeactiveAsync()
    {
        var ct = _singleTaskCancellation.CancelAndReCreateToken(this.GetCancellationTokenOnDestroy());

        //この処理開始時点での_elementProgressの値を記憶
        float preElementProgress = _activeProgress;

        //最初に何秒かけてだんだん色を変えていくかを決める
        float fadeDuration = _activeProgress * _fadeDuration;

        //変化前のマテリアルのプロパティを取得しておく
        GetCurrentMaterialProperty(out var preMagicSphereMaterialProperty, out float preMarkAlphaClipThreshold);

        ProgressTimer progressTimer = new(fadeDuration);

        while(!progressTimer.IsFinished)
        {
            progressTimer.Tick();

            float progress = progressTimer.CalcProgress();

            _activeProgress = Mathf.Lerp(preElementProgress, _activeProgress_Deactivated, progress);

            //色を変更
            MagicSphereMaterialProperty newProperty = MagicSphereMaterialProperty.ColorLerp(preMagicSphereMaterialProperty, _defaultMaterialProperty.Property, progress);
            _magicSphereMaterialController.SetColor(newProperty);

            //マークの透明度を変更
            float newMarkAlphaClipThreshold = Mathf.Lerp(preMarkAlphaClipThreshold, _deactiveMarkAlphaClipThreshold, progress);
            _magicSphereMaterialController.SetMarkAlphaClipThreshold(newMarkAlphaClipThreshold);

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }

        //フェードが終わった際にテクスチャを変えておく
        _magicSphereMaterialController.SetTexture(_defaultMaterialProperty.Property);
    }

    //現在のマテリアルのプロパティを取得
    void GetCurrentMaterialProperty(out MagicSphereMaterialProperty magicSphereMaterialProperty,out float markAlphaClipThreshold)
    {
        Texture currentMarkTexture = _magicSphereMaterialController.CurrentMarkTexture;
        Color currentBaseInEmissionColor = _magicSphereMaterialController.CurrentBaseInEmissionColor;
        Color currentBaseOutEmissionColor = _magicSphereMaterialController.CurrentBaseOutEmissionColor;
        Color currentMarkEmissionColor = _magicSphereMaterialController.CurrentMarkEmissionColor;

        magicSphereMaterialProperty = new(currentMarkTexture, currentBaseInEmissionColor, currentBaseOutEmissionColor, currentMarkEmissionColor);

        markAlphaClipThreshold = _magicSphereMaterialController.CurrentMarkAlphaClipThreshold;
    }
}
