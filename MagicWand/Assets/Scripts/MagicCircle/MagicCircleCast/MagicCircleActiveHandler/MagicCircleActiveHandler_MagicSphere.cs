using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//MagicCircleActiveHandlerの魔法球の表示・非表示の処理をする

[System.Serializable]
public class MagicCircleActiveHandler_MagicSphere
{
    [SerializeField]
    MagicSpheresList _magicSphereList;

    [Tooltip("隠してから完全に非表示するまで待つ時間")] [SerializeField]
    float _waitDurationFromHideToDeactive = 1.5f;

    public void Start()
    {
        DeactiveAllMagicSpheres();
    }

    //全ての魔法球をデフォルト状態で表示するアニメーションを開始
    public void ShowDefaultAllMagicSpheres()
    {
        var animators = _magicSphereList.GetComponentsArrayFromMagicSpheres<Animator>();

        //アクティブ状態にしてから表示アニメーションを開始
        for(int i=0; i<animators.Length ;i++)
        {
            animators[i].SetTrigger(MagicSphereAnimatorProperty.DefaultTriggerName);
        }
    }

    //全ての魔法球の非表示アニメーションを開始
    public async UniTask HideAllMagicSpheres(CancellationToken ct)
    {
        var animators = _magicSphereList.GetComponentsArrayFromMagicSpheres<Animator>();

        //非表示アニメーション開始
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetTrigger(MagicSphereAnimatorProperty.HideTriggerName);
        }

        //少し待つ
        await UniTask.Delay(TimeSpan.FromSeconds(_waitDurationFromHideToDeactive), cancellationToken: ct);

        //完全に非表示
        DeactiveAllMagicSpheres();
    }

    void DeactiveAllMagicSpheres()
    {
        var magicSpheres = _magicSphereList.MagicSphereObjects;

        for (int i = 0; i < magicSpheres.Length; i++)
        {
            magicSpheres[i].SetActive(false);
        }
    }
}
