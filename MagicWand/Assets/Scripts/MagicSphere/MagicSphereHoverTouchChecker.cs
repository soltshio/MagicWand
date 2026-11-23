using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;

//作成者:杉山
//いずれかの魔法球のホバー選択をした際に、どの魔法球を決定したか判定するクラス

public class MagicSphereHoverTouchChecker
{
    class MagicSphereHoverTouchedNotifier
    {
        int _index;//魔法球の番号
        HoverTouchedReceiver _receiver;
        public event Action<int> ReceiveIndexOnHoverTouch;

        public MagicSphereHoverTouchedNotifier(int index, HoverTouchedReceiver receiver)
        {
            _index = index;
            _receiver = receiver;

            _receiver.OnSubmitted += NotifyTouchedIndexOnTouch;
        }

        public void Dispose()
        {
            _receiver.OnSubmitted -= NotifyTouchedIndexOnTouch;
        }

        //杖が魔法球に触れた際、どの番号に触れたかを通知する
        void NotifyTouchedIndexOnTouch()
        {
            ReceiveIndexOnHoverTouch?.Invoke(_index);
        }
    }

    int _submittedSphereIndex;//決定された球の番号

    const int _defaultSubmittedSphereIndex = -1;

    MagicSpheresList _magicSpheresList;

    public MagicSphereHoverTouchChecker(MagicSpheresList magicSpheresList)
    {
        _magicSpheresList = magicSpheresList;
    }

    //いずれかの球のホバー選択が決定されるまで待つ
    //決定されたら、その魔法球のインデックスを返す。
    public async UniTask<int> WaitUntilSubmitAnyMagicSphere(CancellationToken ct)
    {
        _submittedSphereIndex = _defaultSubmittedSphereIndex;

        int magicSphereCount = _magicSpheresList.MagicSphereObjects.Length;//魔法球の数

        //決定された球の番号を受け取れるようにする
        MagicSphereHoverTouchedNotifier[] magicSphereHoverTouchedNotifiers = new MagicSphereHoverTouchedNotifier[magicSphereCount];

        for (int i = 0; i < magicSphereCount; i++)
        {
            var hoverTouchedReceiver = _magicSpheresList.GetComponentFromMagicSphere<HoverTouchedReceiver>(i);

            if (hoverTouchedReceiver == null) continue;

            magicSphereHoverTouchedNotifiers[i] = new(i, hoverTouchedReceiver);
            magicSphereHoverTouchedNotifiers[i].ReceiveIndexOnHoverTouch += ReceiveTouchedIndex;
        }

        //触れた球の番号がデフォルトのものから変わるまで待つ
        await UniTask.WaitUntil(() => _submittedSphereIndex != _defaultSubmittedSphereIndex, cancellationToken: ct);

        //MagicSphereTouchedNotifierの破棄処理を呼び出す
        for (int i = 0; i < magicSphereHoverTouchedNotifiers.Length; i++)
        {
            if (magicSphereHoverTouchedNotifiers[i] == null) continue;
            magicSphereHoverTouchedNotifiers[i].Dispose();
        }

        return _submittedSphereIndex;
    }

    void ReceiveTouchedIndex(int index)
    {
        _submittedSphereIndex = index;
    }
}
