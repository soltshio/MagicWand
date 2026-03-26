using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//魔法の発動条件(魔法陣のなぞる順番と魔法の内容を格納)

public class Magic : MonoBehaviour
{
    [Tooltip("球のアクティブ化の順番\nMagicSpheresの要素番号(0～配列の要素数-1)を入力してください")] [SerializeField]
    int[] _activeOrderIndexs;

    [SerializeField]
    string _magicLog;

    [SerializeField]
    Material _magicSphereMaterial;

    bool _spellIsValid=true;
    int _currentIndex=0;
    
    public bool IsSpellCasted { get { return _currentIndex >= _activeOrderIndexs.Length; } }//魔法が発動したか
    public Material MagicSphereMaterial { get { return _magicSphereMaterial; } }//魔法球のマテリアル
    
    //次にどの球をなぞるべきか
    public int NextMagicSphereIndex
    {
        get 
        {
            //既に魔法を発動済み、もしくは既になぞる順番を間違えている場合は次にどの球をなぞるべきかを表示する必要はない
            if (IsSpellCasted || !_spellIsValid) return -1;

            return _activeOrderIndexs[_currentIndex]; 
        }
    }

    //初期化
    public void Initialize()
    {
        _spellIsValid = true;
        _currentIndex = 0;
    }

    //なぞった魔法球の要素番号を伝え、もし間違えたら魔法が発動しない、全て正しい順番でなぞったら魔法を発動させる
    //なぞった場所が正しかったらtrueを返す、間違っていたらfalseを返す
    public bool CallSpell(int magicSphereIndex)
    {
        if(IsSpellCasted) return false;
        if (!_spellIsValid) return false;

        //番号が間違えていたら、魔法が発動しないようにする
        if (_activeOrderIndexs[_currentIndex] != magicSphereIndex)
        {
            _spellIsValid = false;
        }

        _currentIndex++;

        //魔法発動条件を確かめて魔法を発動させる
        if(_spellIsValid && IsSpellCasted)
        {
            Debug.Log(_magicLog);
        }

        return true;
    }
}
