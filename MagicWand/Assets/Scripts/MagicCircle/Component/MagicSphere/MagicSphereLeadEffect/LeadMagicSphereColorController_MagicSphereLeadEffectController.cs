using System.Collections.Generic;
using UnityEngine;

//作成者:杉山
//なぞる誘導演出の魔法球の色をコントロールする機能

public partial class MagicSphereLeadEffectController
{
    [System.Serializable]
    class LeadMagicSphereColorController
    {
        [SerializeField]
        MagicSphereMaterialProperty _deactiveMaterialProperty;

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
            var magicSphereMaterialControllers = _magicSpheresList.GetComponentsArrayFromMagicSpheres<MagicSphereMaterialController>();

            for (int i = 0; i < activeSphereIndex_MagicList.Count; i++)
            {
                var index = activeSphereIndex_MagicList[i].index;
                var magic = activeSphereIndex_MagicList[i].magic;

                //色を取得
                if (!_spellCastList.TryGetSpellCast(magic, out var spellCast)) continue;

                var activeMagicSphereMaterialProperty = spellCast.ActiveMagicSphereMaterialProperty;

                //色を変える球のマテリアルコントローラーを取得
                var magicSphereMaterialController = _magicSpheresList.GetComponentFromMagicSphere<MagicSphereMaterialController>(index);

                if (magicSphereMaterialController == null) continue;

                magicSphereMaterialController.SetColor(activeMagicSphereMaterialProperty);
                magicSphereMaterialController.SetTexture(activeMagicSphereMaterialProperty);
                magicSphereMaterialController.SetMarkAlphaClipThreshold(0f);
            }
        }

        //全ての魔法球を元の色に戻す
        public void PaintDefaultColorToAllMagicSphere()
        {
            var magicSphereMaterialControllers = _magicSpheresList.GetComponentsArrayFromMagicSpheres<MagicSphereMaterialController>();

            for (int i = 0; i < magicSphereMaterialControllers.Length; i++)
            {
                var matController = magicSphereMaterialControllers[i];

                if (matController == null) continue;

                matController.SetColor(_deactiveMaterialProperty);
                matController.SetTexture(_deactiveMaterialProperty);
                matController.SetMarkAlphaClipThreshold(1f);
            }
        }
    }
}
