using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//移動平均を取るクラス(Vector2用)

public class Vector2MovingAverage
{
    Queue<Vector2> _buffer = new Queue<Vector2>();
    int _windowSize;

    Vector2 _sum;// 現在の合計値を保持しておくことで、平均の計算を高速化する
    const int _minSize = 1;

    public Vector2MovingAverage(int size)
    {
        size = Mathf.Max(size, _minSize);//サイズが0になることを防ぐ

        _windowSize = size;
        _sum = Vector2.zero;
    }

    public void Clear()
    {
        _buffer.Clear();
        _sum = Vector2.zero;
    }

    public Vector2 AddValue(Vector2 value)
    {
        _sum += value;
        _buffer.Enqueue(value);

        DequeueOldestValue();

        Vector2 avg = Average();

        return avg;
    }

    void DequeueOldestValue()
    {
        if (_buffer.Count <= _windowSize) return;

        Vector2 dequeuedValue = _buffer.Dequeue();
        _sum -= dequeuedValue;// 古い値を合計から引いておく
    }

    Vector2 Average()
    {
        return _sum / _buffer.Count;
    }
}
