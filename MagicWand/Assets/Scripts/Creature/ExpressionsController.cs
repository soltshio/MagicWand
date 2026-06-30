using UnityEngine;

//作成者:杉山
//でか生き物の表情差分コントローラー

public class ExpressionsController : MonoBehaviour
{
    [SerializeField]
    MeshRenderer _expressionsRenderer;

    [Tooltip("表情差分のテクスチャで横(x)と縦(y)で何個差分があるか")] [SerializeField]
    Vector2Int _expressionsTexCount; 

    [SerializeField]
    Vector2 _distance;

    Material _expressionsMat;

    static readonly int _baseMapID = Shader.PropertyToID("_BaseMap");

    public int ExpressionsCount { get { return _expressionsTexCount.x * _expressionsTexCount.y; } }

    void Start()
    {
        _expressionsMat = _expressionsRenderer.material;
    }

    //テクスチャの左上の方から、0から順に数えた表情差分の番号を指定してそれに対応した表情に変更する
    public void ChangeExpression(int expressionNum)
    {
        //番号が範囲外であれば弾く
        if (!MathfExtension.IsInRange(expressionNum, 0, ExpressionsCount - 1)) return;

        int xNum = expressionNum % ExpressionsCount;
        int yNum = expressionNum / ExpressionsCount;
    }
}
