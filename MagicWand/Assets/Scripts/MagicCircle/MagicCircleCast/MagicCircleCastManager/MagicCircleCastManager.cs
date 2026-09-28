using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Linq;

//作成者:杉山
//魔法陣を起動させると、魔法が発動するまで魔法陣をなぞらせる処理をする
//魔法が発動すると、発動した魔法の内容を通知すると共に魔法陣を非アクティブにする
//TODO処理をキャンセルした場合の処理を書く

public class MagicCircleCastManager : MonoBehaviour
{
    [SerializeField]
    MagicSpheresList _magicSpheresList;

    [Tooltip("魔法陣のなぞった線を描画する機能")] [SerializeField]
    MagicSphereTrail _magicSphereTrail;

    [Tooltip("魔法一覧")] [SerializeField]
    MagicList _magicList;

    [Tooltip("誘導エフェクトをコントロールする機能")] [SerializeField]
    MagicSphereLeadEffectController _magicSphereLeadEffectController;

    MagicSphereTouchChecker _magicSphereTouchChecker;

    PassedSphereIndexHistory _passedSphereIndexHistory = new();//通った球の番号の履歴

    public event Action<EMagic,int> OnSuccessToCast;//発動手順が合っていたことの通知、第一引数に魔法の内容、第二引数に触れた球のインデックスを入れている
    public event Action OnStartToCast;//魔法の発動が始まったことの通知

    //魔法陣の処理、処理が終わったら魔法の内容を返す
    //TODO:キャストパターンを引数に入れることで指定出来るようにする
    public async UniTask<EMagic> MagicCircleAsync(Dictionary<EMagic, int[]> castPatterns,CancellationToken ct)
    {
        //新しい履歴を作成
        _passedSphereIndexHistory.CreateNewHistory();

        OnStartToCast?.Invoke();

        //球を全て非アクティブにする
        _magicSphereLeadEffectController.DeactiveLeadAsync().Forget();

        //何かしらの魔法が発動可能になるまで待つ
        //発動可能魔法を受け取る
        var invokableMagic = await CastMagicAsync(ct, castPatterns);

        return invokableMagic;
    }

    async UniTask<EMagic> CastMagicAsync(CancellationToken ct, Dictionary<EMagic, int[]> castPatterns)
    {
        try
        {
            if(!TryGetCastableSpellCastsFromCastPatterns(castPatterns,out var castableSpellCasts)) return EMagic.None;

            //現在発動の可能性がある魔法リストの作成
            CastableMagics castableMagics = new(castableSpellCasts);
            castableMagics.OnSuccessToCast += OnSuccessToCast;

            while (true)
            {
                //発動可能性のある魔法から、次になぞるべき球をリストアップする
                List<(EMagic magic, int index)> activeSphereIndex_MagicList = castableMagics.ActivateNextTraceMagicSphere();

                //最後に触れた球からリストアップした球に誘導演出を行う(一番最初に球に触れる場合は真ん中から誘導演出を行う)
                _magicSphereLeadEffectController.ActiveLeadAsync(PreActiveSphereIndex(), activeSphereIndex_MagicList).Forget();

                //杖がいずれかの球に触れるまで待つ&触れた球のインデックスを取得
                List<int> activeSphereIndexList = activeSphereIndex_MagicList.Select(x => x.index).ToList();
                int touchedMagicSphereindex = await _magicSphereTouchChecker.WaitUntilTouchAnyMagicSphere(activeSphereIndexList, ct);

                //履歴に番号を追加
                _passedSphereIndexHistory.AddIndex(touchedMagicSphereindex);

                //杖が触れた球のインデックスを魔法に伝える
                var invokableMagic = castableMagics.CastTouchedIndexToMagics(touchedMagicSphereindex);//発動可能な魔法

                //なぞった球の位置を魔法陣の線の描画機能に伝える
                _magicSphereTrail.Add(_magicSpheresList.MagicSphereObjects[touchedMagicSphereindex].transform.localPosition);

                //球を全て非アクティブにする
                _magicSphereLeadEffectController.DeactiveLeadAsync().Forget();

                //発動可能な魔法があれば、それを返し、魔法陣をなぞる処理を終える
                if (invokableMagic != EMagic.None)
                {
                    return invokableMagic;
                }

                //発動可能性のない魔法をリストから消す
                castableMagics.RemoveIncastableMagic();
            }
        }
        catch(OperationCanceledException)//途中で詠唱がキャンセルされた場合
        {
            return EMagic.None;
        }
    }

    private void Awake()
    {
        _magicSphereTouchChecker = new(_magicSpheresList);
    }

    //詠唱パターンにある全魔法から、魔法発動状態を取り出し初期化してcastableMagicsに入れて返す
    bool TryGetCastableSpellCastsFromCastPatterns(Dictionary<EMagic, int[]> castPatterns,out Dictionary<EMagic, SpellCast> castableSpellCasts)
    {
        castableSpellCasts = new();

        //全魔法の詠唱状況を取得
        var spellCasts = _magicList.GetComponentsDictionaryFromMagics<SpellCast>();

        if(spellCasts == null || spellCasts.Count == 0)
        {
            castableSpellCasts = null;
            return false;
        }

        foreach(var castPattern in castPatterns)
        {
            //詠唱パターンごとから、その魔法に対応した詠唱状況を取得する
            if (!spellCasts.TryGetValue(castPattern.Key, out var spellCast)) continue;

            spellCast.Initialize(castPattern.Value);//詠唱状況の初期化
            castableSpellCasts.Add(castPattern.Key, spellCast);//詠唱可能な魔法の詠唱状況に追加
        }

        return true;
    }

    int? PreActiveSphereIndex()
    {
        return _passedSphereIndexHistory.TryGetLastIndex(out int index) ? index : null;
    }
}
