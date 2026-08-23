using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Linq;

//作成者:杉山
//魔法陣を起動させると、魔法が発動するまで魔法陣をなぞらせる処理をする
//魔法が発動すると、発動した魔法の内容を通知すると共に魔法陣を非アクティブにする

public class MagicCircleCastManager : MonoBehaviour
{
    [SerializeField]
    MagicSpheresList _magicSpheresList;

    [SerializeField]
    CastPatternManager _castPatternManager;

    [Tooltip("魔法陣のなぞった線を描画する機能")] [SerializeField]
    MagicSphereTrail _magicSphereTrail;

    [Tooltip("魔法一覧")] [SerializeField]
    SpellCastList _spellCastList;

    [Tooltip("誘導エフェクトをコントロールする機能")] [SerializeField]
    MagicSphereLeadEffectController _magicSphereLeadEffectController;

    MagicSphereTouchChecker _magicSphereTouchChecker;

    PassedSphereIndexHistory _passedSphereIndexHistory = new();//通った球の番号の履歴

    public event Action<EMagic,int> OnSuccessToCast;//発動手順が合っていたことの通知、第一引数に魔法の内容、第二引数に触れた球のインデックスを入れている
    public event Action OnStartToCast;//魔法の発動が始まったことの通知

    //魔法陣の処理、処理が終わったら魔法の内容を返す
    public async UniTask<EMagic[]> MagicCircleAsync()
    {
        var token = this.GetCancellationTokenOnDestroy();

        //魔法発動の初期化
        InitAllSpellCast();

        //新しい履歴を作成
        _passedSphereIndexHistory.CreateNewHistory();

        OnStartToCast?.Invoke();

        //何かしらの魔法が発動可能になるまで待つ
        //発動可能魔法を受け取る
        var invokableMagics = await CastMagicAsync(token);

        return invokableMagics;
    }

    async UniTask<EMagic[]> CastMagicAsync(CancellationToken token)
    {
        //現在発動の可能性がある魔法リストの作成
        CastableMagics castableMagics = new(_spellCastList.SpellCasts);
        castableMagics.OnSuccessToCast += OnSuccessToCast;

        while (true)
        {
            //発動可能性のある魔法から、次になぞるべき球をリストアップする
            List<(EMagic magic, int index)> activeSphereIndex_MagicList = castableMagics.ActivateNextTraceMagicSphere(_magicSpheresList);

            //最後に触れた球からリストアップした球に誘導演出を行う(一番最初に球に触れる場合は真ん中から誘導演出を行う)
            _magicSphereLeadEffectController.ActiveLeadAsync(PreActiveSphereIndex(), activeSphereIndex_MagicList).Forget();

            //杖がいずれかの球に触れるまで待つ&触れた球のインデックスを取得
            List<int> activeSphereIndexList = activeSphereIndex_MagicList.Select(x => x.index).ToList();
            int touchedMagicSphereindex = await _magicSphereTouchChecker.WaitUntilTouchAnyMagicSphere(activeSphereIndexList, token);

            //履歴に番号を追加
            _passedSphereIndexHistory.AddIndex(touchedMagicSphereindex);

            //杖が触れた球のインデックスを魔法に伝える
            var invokableMagics = castableMagics.CastTouchedIndexToMagics(touchedMagicSphereindex);//発動可能な魔法

            //なぞった球の位置を魔法陣の線の描画機能に伝える
            _magicSphereTrail.Add(_magicSpheresList.MagicSphereObjects[touchedMagicSphereindex].transform.localPosition);

            //球を全て非アクティブにする
            _magicSphereLeadEffectController.DeactiveLeadAsync().Forget();

            //発動可能な魔法があれば、それを返し、魔法陣をなぞる処理を終える
            if (invokableMagics.Length > 0)
            {
                return invokableMagics;
            }

            //発動可能性のない魔法をリストから消す
            castableMagics.RemoveIncastableMagic();
        }
    }

    private void Awake()
    {
        _magicSphereTouchChecker = new(_magicSpheresList);
    }

    //魔法発動の初期化
    void InitAllSpellCast()
    {
        //発動パターンを決定
        var castPatterns = _castPatternManager.DecideActiveOrderIndexs();

        foreach (var spellCast in _spellCastList.SpellCasts)
        {
            if(!castPatterns.TryGetValue(spellCast.Key,out var orderIndexs))
            {
                Debug.Log("発動パターンの取得に失敗！");
                continue;
            }

            spellCast.Value.Initialize(orderIndexs);
        }
    }

    int? PreActiveSphereIndex()
    {
        return _passedSphereIndexHistory.TryGetLastIndex(out int index) ? index : null;
    }
}
