using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//作成者:杉山
//いずれかの魔法球に振れた際に、どの魔法球に触れたかを判定するクラス

public class MagicSphereTouchChecker
{
    class MagicSphereTouchedNotifier
    {
        int _index;//魔法球の番号
        MagicSphereTouchedReceiver _receiver;
        public event Action<int> ReceiveIndexOnTouch;

        public MagicSphereTouchedNotifier(int index,MagicSphereTouchedReceiver receiver)
        {
            _index = index;
            _receiver = receiver;

            _receiver.OnTouchedEnter += NotifyTouchedIndexOnTouch;
        }

        public void Dispose()
        {
            _receiver.OnTouchedEnter -= NotifyTouchedIndexOnTouch;
        }

        //杖が魔法球に触れた際、どの番号に触れたかを通知する
        void NotifyTouchedIndexOnTouch()
        {
            ReceiveIndexOnTouch?.Invoke(_index);
        }
    }


    int _touchedSphereIndex;//触れた球の番号

    const int _defaultTouchedSphereIndex = -1;

    MagicSpheresList _magicSpheresList;

    public MagicSphereTouchChecker(MagicSpheresList magicSpheresList)
    {
        _magicSpheresList = magicSpheresList;
    }

    //いずれかの球に杖が触れるまで待つ
    //触れたら、触れた魔法球のインデックスを返す。
    public async UniTask<int> WaitUntilTouchAnyMagicSphere(List<int> activeMagicSphereIndexList,CancellationToken ct)
    {
        _touchedSphereIndex = _defaultTouchedSphereIndex;

        //触れた球の番号を受け取れるようにする
        MagicSphereTouchedNotifier[] magicSphereTouchedNotifiers = new MagicSphereTouchedNotifier[activeMagicSphereIndexList.Count];

        for(int i=0; i<activeMagicSphereIndexList.Count ;i++)
        {
            var activeMagicSphereIndex = activeMagicSphereIndexList[i];

            var touchedReceiver = _magicSpheresList.GetComponentFromMagicSphere<MagicSphereTouchedReceiver>(activeMagicSphereIndex);

            if (touchedReceiver == null) continue;

            magicSphereTouchedNotifiers[i] = new(activeMagicSphereIndex,touchedReceiver);
            magicSphereTouchedNotifiers[i].ReceiveIndexOnTouch += ReceiveTouchedIndex;
        }

        //触れた球の番号がデフォルトのものから変わるまで待つ
        await UniTask.WaitUntil(() => _touchedSphereIndex != _defaultTouchedSphereIndex,cancellationToken:ct);

        //MagicSphereTouchedNotifierの破棄処理を呼び出す
        for(int i=0; i< magicSphereTouchedNotifiers.Length; i++)
        {
            if (magicSphereTouchedNotifiers[i] == null) continue;
            magicSphereTouchedNotifiers[i].Dispose();
        }

        return _touchedSphereIndex;
    }

    void ReceiveTouchedIndex(int index)
    {
        _touchedSphereIndex = index;
    }
}
