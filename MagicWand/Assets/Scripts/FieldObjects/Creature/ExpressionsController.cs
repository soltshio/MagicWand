using UnityEngine;

//作成者:杉山
//でか生き物の表情差分コントローラー

public class ExpressionsController : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRenderer _expressionsRenderer;

    [Tooltip("デフォルト(初期)の表情番号")] [SerializeField]
    int _defaultExpressionNum;

    [Tooltip("表情差分のテクスチャで横(x)と縦(y)で何個差分があるか")] [SerializeField]
    Vector2Int _expressionsTexCount;

    [Tooltip("左上の最初の番号(0)にあたる表情差分のテクスチャ位置")] [SerializeField]
    Vector2 _texPosFirstExpression;

    [Tooltip("表情差分同士のテクスチャ上の距離")] [SerializeField]
    Vector2 _texDistance;

    Material _expressionsMat;

    static readonly int _baseMapID = Shader.PropertyToID("_BaseMap");

    public int ExpressionsCount { get { return _expressionsTexCount.x * _expressionsTexCount.y; } }

    void Start()
    {
        _expressionsMat = _expressionsRenderer.material;

        //表情の初期化
        ChangeExpression(Mathf.Clamp(_defaultExpressionNum, 0, ExpressionsCount-1));
    }

    //テクスチャの左上の方から、0から順に数えた表情差分の番号を指定してそれに対応した表情に変更する
    public void ChangeExpression(int expressionNum)
    {
        //番号が範囲外であれば弾く
        if (!MathfExtension.IsInRange(expressionNum, 0, ExpressionsCount - 1))
        {
            Debug.Log("表情差分番号が範囲外です！");
            return;
        }

        int xNum = expressionNum % _expressionsTexCount.x;
        int yNum = expressionNum / _expressionsTexCount.x;

        float x = _texPosFirstExpression.x + (xNum * _texDistance.x);
        float y = _texPosFirstExpression.y + (yNum * _texDistance.y);

        Vector2 offset = new Vector2(x, y);

        _expressionsMat.SetTextureOffset(_baseMapID, offset);
    }
}
