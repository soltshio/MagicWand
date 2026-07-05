using UnityEngine;

//作成者:杉山
//魔法の発動管理

public class SpellCast : MonoBehaviour
{
    int[] _activeOrderIndexs;

    [SerializeField]
    string _magicLog;

    [SerializeField]
    Material _magicSphereMaterial;

    bool _spellIsValid = true;
    int _currentIndex = 0;

    public bool SpellIsValid { get { return _spellIsValid; } }//魔法の発動手順が合っているか
    public bool IsReadyToInvoke { get { return _currentIndex >= _activeOrderIndexs.Length; } }//魔法が発動可能か

    public Material MagicSphereMaterial { get { return _magicSphereMaterial; } }//魔法球のマテリアル

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

    //詠唱(なぞった球の要素番号を入力する、間違えたら初期化しない限り二度と魔法は発動しない)
    public void Cast(int magicSphereIndex)
    {
        if (IsReadyToInvoke) return;
        if (!_spellIsValid) return;

        //番号が間違えていたら、魔法の発動手順を間違えたということにする
        if (_activeOrderIndexs[_currentIndex] != magicSphereIndex)
        {
            _spellIsValid = false;
        }

        _currentIndex++;

        //以下はデバッグ用処理
        if (!IsReadyToInvoke) return;
        Debug.Log(_magicLog);
    }
}
