using UnityEngine;

//作成者:杉山
//カーソルの位置の調整シーンの北陽レーザーの中心の値の補正処理

[System.Serializable]
public class AdjustHokuyoCenter_CursorAdjust
{
    HokuyoDataTransmitter _hokuyoDataTransmitter;
    HokuyoDataReceiver _hokuyoDataReceiver;

    readonly Vector2 _trueCenterBlobPos = new(0.5f, 0.5f);

    public void Initialize(HokuyoDataTransmitter hokuyoDataTransmitter,HokuyoDataReceiver hokuyoDataReceiver)
    {
        _hokuyoDataTransmitter = hokuyoDataTransmitter;
        _hokuyoDataReceiver = hokuyoDataReceiver;
    }

    public void AdjustHokuyoCenter(Vector2 currentCenterBlobPos)
    {
        Vector2 deltaCenter = currentCenterBlobPos - _trueCenterBlobPos;
        Vector2 deltaCenterM = CursorPosWithHokuyoPosTransformHandler.FromHokuyoPosToDetectionRangeMeterPos(deltaCenter, _hokuyoDataReceiver.SizeScale, _hokuyoDataReceiver.Size);

        Vector2 trueCenterM = _hokuyoDataReceiver.Center + deltaCenterM;

        _hokuyoDataTransmitter.SendCenter(trueCenterM);
    }
}
