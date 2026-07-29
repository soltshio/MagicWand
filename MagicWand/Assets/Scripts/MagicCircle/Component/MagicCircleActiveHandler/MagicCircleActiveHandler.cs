using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

//作成者:杉山
//魔法陣の表示・非表示をする

public class MagicCircleActiveHandler : MonoBehaviour
{
    [Tooltip("初期状態では表示しておくか")] [SerializeField]
    bool _isInitShow = false;

    [Tooltip("表示・非表示にかける時間")] [SerializeField]
    float _fadeDuration = 1f;


    [Header("魔法陣関係")]
    [Tooltip("魔法陣の描画機能")] [SerializeField]
    SpriteRenderer _magicCircleRenderer;

    [Tooltip("表示時の透明度")] [Range(0f, 1f)] [SerializeField]
    float _magicCircleAlpha_Active;


    [Header("魔法陣の球関係")]
    [Tooltip("魔法陣の球の当たり判定")] [SerializeField]
    Collider[] _magicSphereColliders;

    [SerializeField]
    MagicSpheresList _magicSphereList;


    [Header("魔法陣の線関係")]
    [Tooltip("魔法陣の線の描画機能")] [SerializeField]
    LineRenderer _magicCircleTrailRenderer;

    [Tooltip("表示時の透明度")] [Range(0f, 1f)] [SerializeField]
    float _magicCircleTrailAlpha_Active;

    bool _isProcessing = false;

    void Start()
    {
        //魔法陣
        _magicCircleRenderer.enabled = _isInitShow;

        //魔法陣の球
        for(int i=0; i< _magicSphereColliders.Length ;i++)
        {
            _magicSphereColliders[i].enabled=_isInitShow;
        }

        //魔法陣の線
        _magicCircleTrailRenderer.enabled = _isInitShow;
    }

    //魔法陣の表示
    public async UniTask ActivateMagicCircleAsync(CancellationToken ct)
    {
        if (_isProcessing) return;
        _isProcessing = true;

        //魔法陣を表示
        _magicCircleRenderer.enabled = true;

        //魔法陣の線を表示
        _magicCircleTrailRenderer.enabled = true;

        float elapsed = 0f;

        while(elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float rate = elapsed / _fadeDuration;

            //魔法陣
            float magicCircleAlpha = Mathf.Lerp(0f, _magicCircleAlpha_Active, rate);
            SetMagicCircleAlpha(magicCircleAlpha);

            //球の表示
            _magicSphereList.SetAllMagicSpheresAlpha(rate);

            //魔法陣の線

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }

        //球の当たり判定をオンにする
        for (int i = 0; i < _magicSphereColliders.Length; i++)
        {
            _magicSphereColliders[i].enabled = true;
        }

        //魔法陣の透明度を最大化
        SetMagicCircleAlpha(_magicCircleAlpha_Active);
        
        //魔法陣の線の透明度を最大化
        

        _isProcessing = false;
    }

    //魔法陣の非表示
    public async UniTask DeActivateMagicCircleAsync(CancellationToken ct)
    {
        if (_isProcessing) return;
        _isProcessing = true;

        //球の当たり判定をオフにする
        for (int i = 0; i < _magicSphereColliders.Length; i++)
        {
            _magicSphereColliders[i].enabled = false;
        }

        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float rate = elapsed / _fadeDuration;

            //魔法陣
            float magicCircleAlpha = Mathf.Lerp(_magicCircleAlpha_Active, 0f, rate);
            SetMagicCircleAlpha(magicCircleAlpha);

            //魔法陣の球
            float magicSphereAlpha = Mathf.Lerp(1, 0, rate);
            _magicSphereList.SetAllMagicSpheresAlpha(magicSphereAlpha);

            //魔法陣の線

            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
        }

        //魔法陣を完全に非表示
        SetMagicCircleAlpha(0f);
        _magicCircleRenderer.enabled = false;

        //魔法陣の線を完全に非表示
        _magicCircleTrailRenderer.enabled = false;

        _isProcessing = false;
    }

    //魔法陣の透明度をセットする
    void SetMagicCircleAlpha(float alpha)
    {
        var magicCircleColor = _magicCircleRenderer.color;
        magicCircleColor.a = alpha;
        _magicCircleRenderer.color = magicCircleColor;
    }
}
