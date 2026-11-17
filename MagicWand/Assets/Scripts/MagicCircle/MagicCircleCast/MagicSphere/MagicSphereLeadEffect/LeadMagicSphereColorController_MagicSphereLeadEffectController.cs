using Cysharp.Threading.Tasks;
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
        public void PaintMagicColorToMagicSphere(int? preActiveSphereIndex,List<(EMagic magic, int index)> activeSphereIndex_MagicList)
        {
            //TODO:前に色を付けた魔法球をデフォルトの状態に戻す

            //次になぞるべき魔法球に色を付ける
            for (int i = 0; i < activeSphereIndex_MagicList.Count; i++)
            {
                var index = activeSphereIndex_MagicList[i].index;
                var magic = activeSphereIndex_MagicList[i].magic;

                var magicSphereAnimator = _magicSpheresList.GetComponentFromMagicSphere<Animator>(index);

                if (magicSphereAnimator == null) continue;
                if (!MagicSphereAnimatorProperty.TryGetMagicTriggerName(magic, out string triggerName)) continue;

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
