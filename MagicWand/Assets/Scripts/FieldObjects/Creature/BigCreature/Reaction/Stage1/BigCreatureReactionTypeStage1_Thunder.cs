using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Playables;

//作成者:杉山
//巨大生物のステージ1の雷魔法に対してのリアクション

public class BigCreatureReactionTypeStage1_Thunder : BigCreatureReactionTypeBase
{
    [SerializeField]
    PlayableDirector _thunderReactionDirecter;

    [Tooltip("でかい生き物の土の量を変更する機能")] [SerializeField]
    ShifterBigCreatureSoilMaterial _shifterBigCreatureSoil;

    [SerializeField]
    SkinnedMeshRendererReference _bigCreatureBodyMeshRenderer;

    [SerializeField]
    Material _electricShockMat;

    Material _defaultMaterial;//元のマテリアル

    public void ChangeToElectricShockMaterial()
    {
        //感電マテリアルに変更
        _defaultMaterial = _bigCreatureBodyMeshRenderer.SkinnedMeshRenderer.material;
        _bigCreatureBodyMeshRenderer.SkinnedMeshRenderer.sharedMaterial = _electricShockMat;
    }

    public void BackToDefaultMaterial()
    {
        if (_defaultMaterial==null) return;

        //元のマテリアルに変更
        _bigCreatureBodyMeshRenderer.SkinnedMeshRenderer.material = _defaultMaterial;
    }

    public void RemoveBigCreatureSoil()
    {
        _shifterBigCreatureSoil.RemoveSoil();
    }

    public override async UniTask TakeReactionAsync()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _thunderReactionDirecter.Play();

        await _thunderReactionDirecter.WaitForStoppedAsync(ct);
    }
}
