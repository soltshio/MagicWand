using UnityEngine;

//作成者:杉山
//カーソルの座標と北陽レーザー上の座標の変換を行うハンドラー
//北陽レーザーの方では実際の画面よりも少し大きめに検知範囲をを広くしている(画面外でも当たり判定を取れるようにするため)ので、それを考慮しての座標変換を行う

public static class CursorPosWithHokuyoPosTransformHandler
{
    const float _centerDetectionPortPosValue = 0.5f;
    const float _edgeDetectionPortDistanceFromCenter = 0.5f;

    //北陽レーザー上の位置座標(xy共に0～1)と、実際の画面に対する北陽レーザーの検知範囲の拡大率
    //から、ビューポート座標に変換する
    public static Vector2 FromDetectionPortPosToViewPortPos(Vector2 detectionPortPos,float scaleRate)
    {
        float edgeOnScreenDistanceFromCenter = _edgeDetectionPortDistanceFromCenter / scaleRate;

        float detectionPortPosOnScreenMin = _centerDetectionPortPosValue - (edgeOnScreenDistanceFromCenter);
        float detectionPortPosOnScreenMax = _centerDetectionPortPosValue + (edgeOnScreenDistanceFromCenter);

        float viewPortX = MathfExtension.Remap(detectionPortPos.x, detectionPortPosOnScreenMin, detectionPortPosOnScreenMax, 0f, 1f);
        float viewPortY = MathfExtension.Remap(detectionPortPos.y, detectionPortPosOnScreenMin, detectionPortPosOnScreenMax, 0f, 1f);

        return new Vector2(viewPortX,viewPortY);
    }

    //北陽レーザー上の位置座標(xy共に0～1)と、実際の画面に対する北陽レーザーの検知範囲の拡大率と、画面のサイズ(メートル)
    //から、実際の大きさに即したレーザー検知範囲内の座標(メートル)に変換する
    public static Vector2 FromDetectionPortPosToDetectionMeterPos(Vector2 detectionPortPos, float scaleRate,Vector2 sizeM)
    {
        return detectionPortPos * sizeM * scaleRate;
    }
}
