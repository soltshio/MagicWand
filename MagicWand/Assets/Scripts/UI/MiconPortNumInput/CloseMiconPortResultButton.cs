using System;
using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//マイコンポート接続の結果表示のUIを閉じるボタン

public class CloseMiconPortResultButton : MonoBehaviour
{
    [SerializeField]
    Canvas _miconPortResultCanvas;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    AudioClip _closeSound;

    public void CloseMiconPortResultPanel()
    {
        _miconPortResultCanvas.enabled = false;
        _audioSource.PlayOneShot(_closeSound);
    }
}
