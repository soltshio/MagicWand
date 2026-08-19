using UnityEngine;

//作成者:杉山
//カーソルの位置の調整シーンの北陽レーザーの中心の値の補正処理

[System.Serializable]
public class AdjustHokuyoSize_CursorAdjust
{
    HokuyoDataTransmitter _hokuyoDataTransmitter;
    HokuyoDataReceiver _hokuyoDataReceiver;

    public void Initialize(HokuyoDataTransmitter hokuyoDataTransmitter, HokuyoDataReceiver hokuyoDataReceiver)
    {
        _hokuyoDataTransmitter = hokuyoDataTransmitter;
        _hokuyoDataReceiver = hokuyoDataReceiver;
    }

    public void AdjustHokuyoSize(Vector2 currentRightUp,Vector2 currentRightDown,Vector2 currentLeftDown)
    {
        Vector2 currentScaledSize = _hokuyoDataReceiver.SizeM * _hokuyoDataReceiver.SizeScale;

        float trueScaledSizeY = Mathf.Abs(currentRightUp.y - currentRightDown.y) * currentScaledSize.y;
        float trueScaledSizeX = Mathf.Abs(currentRightDown.x - currentLeftDown.x) * currentScaledSize.x;

        Vector2 trueScaledSize = new Vector2(trueScaledSizeX, trueScaledSizeY);

        Vector2 trueSize = trueScaledSize / _hokuyoDataReceiver.SizeScale;

        _hokuyoDataTransmitter.SendSizeM(trueSize);
    }
}
