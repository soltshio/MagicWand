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
    SerializableDictionary<EMagic, Magic> _magicsDictionary;

    public event Action<EMagic> OnMagicActived;//魔法が発動した時のイベント

    bool _isActiveMagicCircle = false;//魔法陣が起動しているか

    public bool IsActiveMagicCircle { get => _isActiveMagicCircle; }

    //魔法陣を起動
    public void ActivateMagicCircle()
    {
        if (_isActiveMagicCircle) return;

        MagicCircleAsync2(this.GetCancellationTokenOnDestroy()).Forget();
    }

    async UniTask MagicCircleAsync2(CancellationToken token)
    {
        _isActiveMagicCircle = true;

        //魔法陣の線を消す
        _magicSphereTrail.Clear();

        //魔法の初期化
        InitAllMagic();

        //魔法陣をなぞった時の処理
        //魔法が発動するまで待つ
        await CastMagicAsync(token);

        _isActiveMagicCircle = false;
    }

    async UniTask CastMagicAsync(CancellationToken token)
    {
        //現在発動の可能性がある魔法リストの作成
        Dictionary<EMagic, Magic> castableMagicDic = new Dictionary<EMagic, Magic>(_magicsDictionary);

        while (true)
        {
            //発動可能性のある魔法から、次になぞるべき球をアクティブにする
            List<int> activeMagicSphereIndexList = ActivateNextTraceMagicSphere(castableMagicDic);

            //杖がいずれかの球に触れるまで待つ&触れた球のインデックスを取得
            int touchedMagicSphereindex = -1;
            await UniTask.WaitUntil(() => IsTouchedAnyMagicSphere(activeMagicSphereIndexList, out touchedMagicSphereindex), cancellationToken: token);

            //杖が触れた球のインデックスを魔法に伝える
            bool isAnyMagicActived = CallTouchedIndexToMagics(castableMagicDic,touchedMagicSphereindex);//いずれかの魔法が発動したか

            //球を全て非アクティブにする
            AllMagicSpheresToDeactive();

            //なぞった球の位置を魔法陣の線の描画機能に伝える
            _magicSphereTrail.Add(_magicSpheres[touchedMagicSphereindex].transform.localPosition);

            //既に発動した魔法があれば、魔法陣をなぞる処理を終える
            if (isAnyMagicActived) break;

            //発動可能性のない魔法をリストから消す
            RemoveIncastableMagic(castableMagicDic);
        }
    }

    //発動可能性の無い魔法を発動可能性のある魔法リストから消す
    void RemoveIncastableMagic(Dictionary<EMagic, Magic> castableMagicDic)
    {
        foreach (var magicPair in _magicsDictionary)
        {
            if (!magicPair.Value.SpellIsValid)
            {
                castableMagicDic.Remove(magicPair.Key);
            }
        }
    }

    //杖が触れた球のインデックスを魔法に伝える(それにより次になぞる球の番号の更新、魔法の発動処理を行う)
    //いずれかの魔法が発動すればtrueを返す
    bool CallTouchedIndexToMagics(Dictionary<EMagic, Magic> castableMagicDic,int touchedMagicSphereindex)
    {
        bool isAnyMagicActived = false;//いずれかの魔法が発動したか

        foreach (var magicPair in castableMagicDic)
        {
            bool magicIsActived = magicPair.Value.CallSpell(touchedMagicSphereindex);//魔法が発動したか

            if (magicIsActived)
            {
                OnMagicActived?.Invoke(magicPair.Key);
                isAnyMagicActived = true;
            }
        }

        return isAnyMagicActived;
    }

    //発動可能性のある魔法から、次になぞるべき球をアクティブにする
    //アクティブにした球のインデックスリストを返す
    List<int> ActivateNextTraceMagicSphere(Dictionary<EMagic, Magic> castableMagicDic)
    {
        List<int> activeMagicSphereIndexList = new();

        foreach (var magicPair in castableMagicDic)
        {
            int nextIndex = magicPair.Value.NextMagicSphereIndex;

            if (nextIndex == -1) continue;

            _magicSpheres[nextIndex].ToActive(magicPair.Value.MagicSphereMaterial);
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
    void InitAllMagic()
    {
        foreach (var magic in _magicsDictionary.Values)
        {
            magic.Initialize();
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
