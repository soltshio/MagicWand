using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

//作成者:杉山
//魔法陣を起動させると、魔法が発動するまで魔法陣をなぞらせる処理をする
//魔法が発動すると、発動した魔法の内容を通知すると共に魔法陣を非アクティブにする

public class MagicCircleManagerVer3 : MonoBehaviour
{
    [Tooltip("12時の方向から時計回りに入れるようにしてください")] [SerializeField]
    MagicSphereVer3[] _magicSpheres; //魔法陣上の球の配列

    [Tooltip("魔法陣のなぞった線を描画する機能")] [SerializeField]
    MagicSphereTrail _magicSphereTrail;

    [Tooltip("魔法一覧")] [SerializeField]
    SerializableDictionary<EMagic, SpellCast> _spellCastsDictionary;

    [Tooltip("魔法陣の表示・非表示をする機能")] [SerializeField]
    MagicCircleActiveHandler _magicCircleActiveHandler;

    bool _isActiveMagicCircle = false;//魔法陣が起動しているか

    public bool IsActiveMagicCircle { get => _isActiveMagicCircle; }

    //魔法陣の処理、処理が終わったら魔法の内容を返す
    //そもそも処理途中なのを無理やり呼び出したらNoneを
    public async UniTask<EMagic[]> MagicCircleAsync()
    {
        if (_isActiveMagicCircle) return null;

        var token = this.GetCancellationTokenOnDestroy();

        _isActiveMagicCircle = true;

        //魔法陣の線を消す
        _magicSphereTrail.Clear();

        //魔法の初期化
        InitAllSpellCast();

        //魔法陣を表示する
        await _magicCircleActiveHandler.ActivateMagicCircleAsync(token);

        //魔法陣をなぞった時の処理
        //魔法が発動するまで待つ
        //発動可能魔法を受け取る
        var invokableMagics = await CastMagicAsync(token);

        //魔法陣と魔法陣の線を非表示にする
        await _magicCircleActiveHandler.DeActivateMagicCircleAsync(token);

        _isActiveMagicCircle = false;

        return invokableMagics;
    }

    async UniTask<EMagic[]> CastMagicAsync(CancellationToken token)
    {
        //現在発動の可能性がある魔法リストの作成
        Dictionary<EMagic, SpellCast> castableMagicDic = new Dictionary<EMagic, SpellCast>(_spellCastsDictionary);

        while (true)
        {
            //発動可能性のある魔法から、次になぞるべき球をアクティブにする
            List<int> activeMagicSphereIndexList = ActivateNextTraceMagicSphere(castableMagicDic);

            //杖がいずれかの球に触れるまで待つ&触れた球のインデックスを取得
            int touchedMagicSphereindex = -1;
            await UniTask.WaitUntil(() => IsTouchedAnyMagicSphere(activeMagicSphereIndexList, out touchedMagicSphereindex), cancellationToken: token);

            //杖が触れた球のインデックスを魔法に伝える
            var invokableMagics = CastTouchedIndexToMagics(castableMagicDic, touchedMagicSphereindex);//いずれかの魔法が発動したか

            //球を全て非アクティブにする
            AllMagicSpheresToDeactive();

            //なぞった球の位置を魔法陣の線の描画機能に伝える
            _magicSphereTrail.Add(_magicSpheres[touchedMagicSphereindex].transform.localPosition);

            //発動可能な魔法があれば、それ返して魔法陣をなぞる処理を終える
            if (invokableMagics.Length > 0)
            {
                return invokableMagics;
            }

            //発動可能性のない魔法をリストから消す
            RemoveIncastableMagic(castableMagicDic);
        }
    }

    //発動可能性の無い魔法を発動可能性のある魔法リストから消す
    void RemoveIncastableMagic(Dictionary<EMagic, SpellCast> castableMagicDic)
    {
        foreach (var spellCastPair in _spellCastsDictionary)
        {
            if (!spellCastPair.Value.SpellIsValid)
            {
                castableMagicDic.Remove(spellCastPair.Key);
            }
        }
    }

    //杖が触れた球のインデックスを魔法に伝える(それにより次になぞる球の番号の更新、魔法の発動処理を行う)
    //発動可能な魔法を伝える
    EMagic[] CastTouchedIndexToMagics(Dictionary<EMagic, SpellCast> castableMagicDic,int touchedMagicSphereindex)
    {
        List<EMagic> invokableMagicsList = new ();

        foreach (var spellCastPair in castableMagicDic)
        {
            spellCastPair.Value.Cast(touchedMagicSphereindex);//触れた球のインデックスを魔法に伝える

            if (spellCastPair.Value.IsReadyToInvoke)
            {
                invokableMagicsList.Add(spellCastPair.Key);
            }
        }

        return invokableMagicsList.ToArray();
    }

    //発動可能性のある魔法から、次になぞるべき球をアクティブにする
    //アクティブにした球のインデックスリストを返す
    List<int> ActivateNextTraceMagicSphere(Dictionary<EMagic, SpellCast> castableMagicDic)
    {
        List<int> activeMagicSphereIndexList = new();

        foreach (var spellCastPair in castableMagicDic)
        {
            int nextIndex = spellCastPair.Value.NextMagicSphereIndex;

            if (nextIndex == -1) continue;

            _magicSpheres[nextIndex].ToActive(spellCastPair.Value.MagicSphereMaterial);
            activeMagicSphereIndexList.Add(nextIndex);
        }

        return activeMagicSphereIndexList;
    }

    //いずれかの球に杖がタッチしたか
    bool IsTouchedAnyMagicSphere(List<int> activeMagicSphereIndexList, out int touchedMagicSphereindex)
    {
        touchedMagicSphereindex = -1;

        foreach (var i in activeMagicSphereIndexList)
        {
            if (!MathfExtension.IsInRange(i, 0, _magicSpheres.Length - 1)) continue;

            if (!_magicSpheres[i].IsActive)
            {
                touchedMagicSphereindex = i;
                return true;
            }
        }

        return false;
    }

    //魔法の初期化
    void InitAllSpellCast()
    {
        foreach (var spellCast in _spellCastsDictionary.Values)
        {
            spellCast.Initialize();
        }
    }

    //球を全て非アクティブにする
    void AllMagicSpheresToDeactive()
    {
        foreach (var magicSphere in _magicSpheres)
        {
            magicSphere.ToDeactive();
        }
    }
}
