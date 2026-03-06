using System.Collections.Generic;
using UnityEngine;

//クォータニオンの移動平均を取るクラス

public class QuaternionMovingAverage
{
    Queue<Quaternion> buffer = new Queue<Quaternion>();
    int windowSize;

    public QuaternionMovingAverage(int size)
    {
        windowSize = size;
    }

    public Quaternion AddValue(Quaternion q)
    {
        buffer.Enqueue(q);

        if (buffer.Count > windowSize)
            buffer.Dequeue();

        Quaternion avg = Average(buffer);

        return avg;
    }

    Quaternion Average(IEnumerable<Quaternion> quats)
    {
        Vector4 sum = Vector4.zero;
        Quaternion first = Quaternion.identity;
        bool firstSet = false;

        foreach (var q in quats)
        {
            Quaternion qq = q;

            if (!firstSet)
            {
                first = q;
                firstSet = true;
            }

            if (Quaternion.Dot(first, qq) < 0)
            {
                qq = new Quaternion(-qq.x, -qq.y, -qq.z, -qq.w);
            }

            sum += new Vector4(qq.x, qq.y, qq.z, qq.w);
        }

        Quaternion result = new Quaternion(sum.x, sum.y, sum.z, sum.w);
        result = Quaternion.Normalize(result);

        return result;
    }
}