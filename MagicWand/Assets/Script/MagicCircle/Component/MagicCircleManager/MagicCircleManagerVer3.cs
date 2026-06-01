using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class MagicCircleManagerVer3 : MonoBehaviour
{
    [Tooltip("12時の方向から時計回りに入れるようにしてください")] [SerializeField]
    MagicSphereVer3[] _magicSpheres; //魔法陣上の球の配列

    [Tooltip("魔法陣のなぞった線を描画する機能")] [SerializeField]
    MagicSphereTrail _magicSphereTrail;

    [Tooltip("魔法一覧")] [SerializeField]
    SerializableDictionary<EMagic, Magic> _magicsDictionary;

    float _magicCoolTime = 2f;

    public event Action<EMagic> OnMagicActived;//魔法が発動した時のイベント

    private void Start()
    {
        MagicCircleAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    async UniTask MagicCircleAsync(CancellationToken token)
    {
        while (true)
        {
            //魔法陣の線を消す
            _magicSphereTrail.Clear();

            //魔法の初期化(同時に現在発動の可能性がある魔法リストも作成)
            InitAllMagic();

            Dictionary<EMagic, Magic> castableMagicDic = new Dictionary<EMagic, Magic>(_magicsDictionary);

            //魔法陣をなぞった時の処理
            //魔法が発動するまで待つ
            await CastMagicAsync(castableMagicDic, token);

            //少し待ってから魔法陣をまたなぞれるようにする
            await UniTask.Delay(TimeSpan.FromSeconds(_magicCoolTime), cancellationToken: token);
        }
    }

    async UniTask CastMagicAsync(Dictionary<EMagic, Magic> castableMagicDic, CancellationToken token)
    {
        while (true)
        {
            bool isAnyMagicActived = false;//いずれかの魔法が発動したか

            //発動可能性のある魔法から、次になぞるべき球をアクティブにする
            List<int> activeMagicSphereIndexList = new();

            foreach (var magicPair in castableMagicDic)
            {
                int nextIndex = magicPair.Value.NextMagicSphereIndex;

                if (nextIndex == -1) continue;

                _magicSpheres[nextIndex].ToActive(magicPair.Value.MagicSphereMaterial);
                activeMagicSphereIndexList.Add(nextIndex);
            }

            //杖がいずれかの球に触れるまで待つ&触れた球のインデックスを取得
            int touchedMagicSphereindex = -1;
            await UniTask.WaitUntil(() => IsTouchedAnyMagicSphere(activeMagicSphereIndexList, out touchedMagicSphereindex), cancellationToken: token);

            //杖に触れた球のインデックスを伝える
            foreach (var magicPair in castableMagicDic)
            {
                bool magicIsActived = magicPair.Value.CallSpell(touchedMagicSphereindex);//魔法が発動したか

                if (magicIsActived)
                {
                    OnMagicActived?.Invoke(magicPair.Key);
                    isAnyMagicActived = true;
                }
            }

            //球を全て非アクティブにする
            foreach (var magicSphere in _magicSpheres)
            {
                magicSphere.ToDeactive();
            }

            //なぞった球の位置を魔法陣の線の描画機能に伝える
            _magicSphereTrail.Add(_magicSpheres[touchedMagicSphereindex].transform.localPosition);

            //既に発動した魔法があれば、一旦魔法陣をなぞる処理を終える
            if (isAnyMagicActived) break;

            //発動可能性のない魔法をリストから消す
            foreach (var magicPair in _magicsDictionary)
            {
                if (!magicPair.Value.SpellIsValid)
                {
                    castableMagicDic.Remove(magicPair.Key);
                }
            }
        }
    }

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

    void InitAllMagic()
    {
        foreach (var magic in _magicsDictionary.Values)
        {
            magic.Initialize();
        }
    }
}
