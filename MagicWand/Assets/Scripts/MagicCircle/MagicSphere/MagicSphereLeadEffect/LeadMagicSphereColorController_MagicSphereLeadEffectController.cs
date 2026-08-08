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

        SpellCastList _spellCastList;

        public void Awake(MagicSpheresList magicSpheresList, SpellCastList spellCastList)
        {
            _magicSpheresList = magicSpheresList;
            _spellCastList = spellCastList;
        }

        //次になぞるべき魔法球に、魔法に対応した色を塗る
        public void PaintMagicColorToMagicSphere(List<(EMagic magic, int index)> activeSphereIndex_MagicList)
        {
            for (int i = 0; i < activeSphereIndex_MagicList.Count; i++)
            {
                var index = activeSphereIndex_MagicList[i].index;
                var magic = activeSphereIndex_MagicList[i].magic;

                //色を取得
                if (!_spellCastList.TryGetSpellCast(magic, out var spellCast)) continue;

                var activeMagicSphereMaterialProperty = spellCast.ActiveMagicSphereMaterialProperty;

                var magicSphereElementColorController = _magicSpheresList.GetComponentFromMagicSphere<MagicSphereElementColorController>(index);

                if (magicSphereElementColorController == null) continue;

                magicSphereElementColorController.ToActiveAsync(activeMagicSphereMaterialProperty).Forget();
            }
        }

        //全ての魔法球を元の色に戻す
        public void PaintDefaultColorToAllMagicSphere()
        {
            var magicSphereElementColorControllers = _magicSpheresList.GetComponentsArrayFromMagicSpheres<MagicSphereElementColorController>();

            for (int i = 0; i < magicSphereElementColorControllers.Length; i++)
            {
                var controller = magicSphereElementColorControllers[i];

                if (controller == null) continue;

                controller.ToDeactiveAsync().Forget();
            }
        }
    }
}
