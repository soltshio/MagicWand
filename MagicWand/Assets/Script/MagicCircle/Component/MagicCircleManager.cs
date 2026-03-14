using System.Collections.Generic;
using System.Collections;
using UnityEngine;

//作成者:杉山
//魔法陣の本体(次にどの球をアクティブにするかなどを決めるマネージャー)

public class MagicCircleManager : MonoBehaviour
{
    [Tooltip("12時の方向から時計回りに入れるようにしてください")] [SerializeField]
    MagicSphere[] _magicSpheres; //魔法陣上の球の配列

    [Tooltip("次にアクティブにする球をランダムにするかどうか\n二連続で同じのが来ないようには抽選されます(ただし重複はあります)")] [SerializeField]
    bool _isRandom = true;

    [Tooltip("ランダム抽選の場合、何回分抽選を行うか")] [SerializeField]
    int _activeCount;

    [Tooltip("球のアクティブ化の順番\nMagicSpheresの要素番号(0～配列の要素数-1)を入力してください")] [SerializeField]
    List<int> _activeOrderIndexList;

    private void Awake()
    {
        TryDecideOrder();
    }

    void TryDecideOrder()
    {
        if (!_isRandom) return;

        _activeOrderIndexList.Clear();

        int previous = -1;

        //順番を決定(2連続で同じ値が抽選されないようにする)
        for (int i=0; i<_activeCount ;i++)
        {
            int lotteryNum;

            //最初の1回は普通に抽選
            if (previous==-1)
            {
                lotteryNum = Random.Range(0, _magicSpheres.Length);
            }
            //2回目以降は前回の値が出ないよう抽選
            else
            {
                lotteryNum = Random.Range(0, _magicSpheres.Length - 1);

                if (lotteryNum >= previous)
                {
                    lotteryNum++;
                }
            }

            _activeOrderIndexList.Add(lotteryNum);
            previous = lotteryNum;
        }
    }

    private void Start()
    {
        StartCoroutine(MagicCircleCoroutine());
    }

    IEnumerator MagicCircleCoroutine()
    {

        for(int i=0; i<_activeOrderIndexList.Count ;i++)
        {
            int index = _activeOrderIndexList[i];

            if (!MathfExtension.IsInRange(index, 0, _magicSpheres.Length - 1)) continue;

            _magicSpheres[index].SwitchActive(true);

            //杖が球に触れて非アクティブになるまで待つ
            yield return new WaitUntil(() => !_magicSpheres[index].IsActive);
        }

        Debug.Log("クリア！");
    }
}
