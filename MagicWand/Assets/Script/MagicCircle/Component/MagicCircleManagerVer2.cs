using System.Collections.Generic;
using System.Collections;
using UnityEngine;

//作成者:杉山
//魔法陣の本体(次にどの球をアクティブにするかなどを決めるマネージャー)
//違う魔法が出せるようにしたver

public class MagicCircleManagerVer2 : MonoBehaviour
{
    [Tooltip("12時の方向から時計回りに入れるようにしてください")] [SerializeField]
    MagicSphereVer2[] _magicSpheres; //魔法陣上の球の配列

    [Tooltip("魔法陣のなぞった線を描画する機能")] [SerializeField]
    MagicSphereTrail _magicSphereTrail;

    [SerializeField]
    Magic[] _magics;

    float _magicCoolTime = 2f;

    Coroutine castMagicCoroutine = null;

    private void Start()
    {
        StartCoroutine(MagicCircleCoroutine());
    }

    IEnumerator MagicCircleCoroutine()
    {
        while (true)
        {
            //魔法陣の線を消す
            _magicSphereTrail.Clear();

            //魔法の初期化(同時に現在発動の可能性がある魔法リストも作成)
            InitAllMagic();

            List<Magic> castableMagicList = MakeCastableMagicList();

            //魔法陣をなぞった時の処理
            castMagicCoroutine = StartCoroutine(CastMagicCoroutine(castableMagicList));

            yield return new WaitUntil(() => castMagicCoroutine == null);

            //少し待ってから魔法陣をまたなぞれるようにする
            yield return new WaitForSeconds(_magicCoolTime);
        }
    }

    IEnumerator CastMagicCoroutine(List<Magic> castableMagicList)
    {
        while (true)
        {
            //発動可能性のある魔法から、次になぞるべき球をアクティブにする
            List<int> activeMagicSphereIndexList = new();

            foreach (var magic in castableMagicList)
            {
                int nextIndex = magic.NextMagicSphereIndex;
    
                if (nextIndex == -1) continue;
    
                _magicSpheres[nextIndex].ToActive(magic.MagicSphereMaterial);
                activeMagicSphereIndexList.Add(nextIndex);
            }

            //杖がどれかの球に触れるまで待つ&触れた球のインデックスを取得
            int touchedMagicSphereindex = -1;
            yield return new WaitUntil(() => IsTouchedAnyMagicSphere(activeMagicSphereIndexList,out touchedMagicSphereindex));

            //杖に触れた球のインデックスを伝える
            foreach (var magic in castableMagicList)
            {
                magic.CallSpell(touchedMagicSphereindex);
            }

            //球を全て非アクティブにする
            foreach (var magicSphere in _magicSpheres)
            {
                magicSphere.ToDeactive();
            }

            //なぞった球の位置を魔法陣の線の描画機能に伝える
            _magicSphereTrail.Add(_magicSpheres[touchedMagicSphereindex].transform.localPosition);

            //リストの中に発動した魔法があるか確認
            if (IsExistCasted(castableMagicList)) break;

            //発動可能性のない魔法をリストから消す
            castableMagicList.RemoveAll(magic => !magic.SpellIsValid);
        }

        castMagicCoroutine = null;
    }

    bool IsTouchedAnyMagicSphere(List<int> activeMagicSphereIndexList,out int touchedMagicSphereindex)
    {
        touchedMagicSphereindex = -1;

        foreach(var i in activeMagicSphereIndexList)
        {
            if(!MathfExtension.IsInRange(i,0,_magicSpheres.Length-1)) continue;

            if (!_magicSpheres[i].IsActive)
            {
                touchedMagicSphereindex = i;
                return true;
            }
        }

        return false;
    }

    bool IsExistCasted(List<Magic> castableMagicList)
    {
        foreach (var magic in castableMagicList)
        {
            if (magic.IsSpellCasted)
            {
                return true;
            }
        }

        return false;
    }

    List<Magic> MakeCastableMagicList()
    {
        List<Magic> castableMagicList = new List<Magic>();

        foreach (var magic in _magics)
        {
            castableMagicList.Add(magic);
        }

        return castableMagicList;
    }

    void InitAllMagic()
    {
        foreach (var magic in _magics)
        {
            magic.Initialize();
        }
    }
}
