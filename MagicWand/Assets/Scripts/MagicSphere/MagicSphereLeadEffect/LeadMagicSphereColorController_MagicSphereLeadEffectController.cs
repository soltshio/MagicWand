using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//なぞる誘導演出の魔法球の色をコントロールする機能

public partial class MagicSphereLeadEffectController
{
    [System.Serializable]
    class LeadMagicSphereColorController
    {
        MagicSpheresList _magicSpheresList;

        public void Awake(MagicSpheresList magicSpheresList)
        {
            _magicSpheresList = magicSpheresList;
        }

        //次になぞるべき魔法球に、魔法に対応した色を塗る
        public void PaintMagicColorToMagicSphere(EMagic[] preTrace_SelectableMagics, EMagic[] currentTrace_SelectableMagics)
        {
            //次になぞるべき魔法球に色を付ける
            for (int sphereIndex = 0; sphereIndex < currentTrace_SelectableMagics.Length; sphereIndex++)
            {
                EMagic preSelectableMagic = preTrace_SelectableMagics[sphereIndex];
                EMagic currentSelectableMagic = currentTrace_SelectableMagics[sphereIndex];

                //前のなぞりと今回のなぞりでその魔法球が同じ種類の魔法が選択可能だった場合は球の色を変えない
                if (currentSelectableMagic == preSelectableMagic) continue;

                var magicSphereAnimator = _magicSpheresList.GetComponentFromMagicSphere<Animator>(sphereIndex);

                if (magicSphereAnimator == null) continue;
                if (!MagicSphereAnimatorProperty.TryGetMagicTriggerName(currentSelectableMagic, out string triggerName)) continue;

                //魔法球の色を変える
                magicSphereAnimator.SetTrigger(triggerName);
            }
        }

        //全ての魔法球を元の色に戻す
        public void PaintDefaultColorToAllMagicSphere()
        {
            var magicSphereAnimators = _magicSpheresList.GetComponentsArrayFromMagicSpheres<Animator>();

            for (int i = 0; i < magicSphereAnimators.Length; i++)
            {
                var animator = magicSphereAnimators[i];

                if (animator == null) continue;

                animator.SetTrigger(MagicSphereAnimatorProperty.DefaultTriggerName);
            }
        }
    }
}
