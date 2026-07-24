using UnityEngine;

//作成者:杉山
//魔法の発動管理

public class SpellCast : MonoBehaviour
{
    int[] _activeOrderIndexs;

    [SerializeField]
    Color _magicSphereColor;

    bool _spellIsValid = true;
    int _currentIndex = 0;

    public bool SpellIsValid { get { return _spellIsValid; } }//魔法の発動手順が合っているか
    public bool IsReadyToInvoke { get { return _currentIndex >= _activeOrderIndexs.Length; } }//魔法が発動可能か

    public Color MagicSphereColor { get { return _magicSphereColor; } }//魔法球の色

    //初期化
    public void Initialize(int[] activeOrderIndexs)
    {
        _activeOrderIndexs = activeOrderIndexs;
        _spellIsValid = true;
        _currentIndex = 0;
    }

    //次にどの球をなぞるべきか
    public int NextMagicSphereIndex
    {
        get
        {
            //既に魔法を発動可能、もしくは既になぞる順番を間違えている場合は次にどの球をなぞるべきかを表示する必要はない
            if (IsReadyToInvoke || !_spellIsValid) return -1;

            return _activeOrderIndexs[_currentIndex];
        }
    }

    //詠唱(なぞった球の要素番号を入力する、番号が違っていたらfalseを返し、初期化しない限り二度と魔法は発動しない。合っていたらtrueを返す)
    public bool Cast(int magicSphereIndex)
    {
        if (IsReadyToInvoke) return false;
        if (!_spellIsValid) return false;

        //番号が違っていたら、魔法の発動手順を間違えたということにする(初期化しない限り二度と発動しない)
        if (_activeOrderIndexs[_currentIndex] != magicSphereIndex)
        {
            _spellIsValid = false;
            return false;
        }

        //合っていた場合次の番号へ
        _currentIndex++;
        return true;
    }
}
