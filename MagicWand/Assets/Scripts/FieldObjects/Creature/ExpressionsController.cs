using UnityEngine;

//作成者:杉山
//でか生き物の表情差分コントローラー

public class ExpressionsController : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRendererReference _expressionsRenderer;

    [Tooltip("表情番号")]
    [SerializeField]
    int _defaultExpressionsIndex;

    [Tooltip("表情差分のテクスチャで横(x)と縦(y)で何個差分があるか")]
    [SerializeField]
    Vector2Int _expressionsTexCount;

    [Tooltip("左上の最初の番号(0)にあたる表情差分のテクスチャ位置")]
    [SerializeField]
    Vector2 _texPosFirstExpression;

    [Tooltip("表情差分同士のテクスチャ上の距離")]
    [SerializeField]
    Vector2 _texDistance;

    static readonly int _baseMapSTID = Shader.PropertyToID("_BaseMap_ST");

    public int ExpressionsCount { get { return _expressionsTexCount.x * _expressionsTexCount.y; } }

    private void OnValidate()
    {
        if (_expressionsRenderer == null)
        {
            Debug.Log("ExpressionsRendererが入ってません！");
            return;
        }

        ChangeExpression(_defaultExpressionsIndex);
    }

    void Start()
    {
        //表情の初期化
        ChangeExpression(_defaultExpressionsIndex);
    }

    //テクスチャの左上の方から、0から順に数えた表情差分の番号を指定してそれに対応した表情に変更する
    public void ChangeExpression(int expressionNum)
    {
        // 番号が範囲外であれば弾く
        if (!MathfExtension.IsInRange(expressionNum, 0, ExpressionsCount - 1))
        {
            Debug.LogError("表情差分番号が範囲外です！");
            return;
        }

        // 表情番号からOffsetを計算
        Vector2 offset = GetExpressionOffset(expressionNum);

        // Offsetを適用
        ApplyTextureOffset(offset);
    }

    // 表情番号からテクスチャのOffsetを取得
    Vector2 GetExpressionOffset(int expressionNum)
    {
        int xNum = expressionNum % _expressionsTexCount.x;
        int yNum = expressionNum / _expressionsTexCount.x;

        float x = _texPosFirstExpression.x + xNum * _texDistance.x;
        float y = _texPosFirstExpression.y + yNum * _texDistance.y;

        return new Vector2(x, y);
    }

    // MaterialPropertyBlockを使ってOffsetを適用
    void ApplyTextureOffset(Vector2 offset)
    {
        var renderer = _expressionsRenderer.SkinnedMeshRenderer;

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(block);

        // 現在のTilingとOffsetを取得
        Vector4 st = renderer.sharedMaterial.GetVector(_baseMapSTID);

        // 既存のPropertyBlockに設定があれば、そちらを優先
        if (block.HasVector(_baseMapSTID))
        {
            st = block.GetVector(_baseMapSTID);
        }

        // Offsetだけ変更
        st.z = offset.x;
        st.w = offset.y;

        block.SetVector(_baseMapSTID, st);
        renderer.SetPropertyBlock(block);
    }
}
