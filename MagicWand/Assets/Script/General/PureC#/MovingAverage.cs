using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//移動平均を取るクラス(Vector3用)

public class Vector3MovingAverage
{
    Queue<Vector3> _buffer = new Queue<Vector3>();
    int _windowSize;

    Vector3 _sum;// 現在の合計値を保持しておくことで、平均の計算を高速化する

    public Vector3MovingAverage(int size)
    {
        _windowSize = size;
        _sum = Vector3.zero;
    }

    public Vector3 AddValue(Vector3 value)
    {
        _buffer.Enqueue(value);

        DequeueOldestValue();

        Vector3 avg = Average();

        return avg;
    }

    void DequeueOldestValue()
    {
        if (_buffer.Count > _windowSize)
        {
            Vector3 dequeuedValue = _buffer.Dequeue();
            _sum -= dequeuedValue;// 古い値を合計から引いておく
        }
    }

    Vector3 Average()
    {
        return _sum / _buffer.Count;
    }
}
