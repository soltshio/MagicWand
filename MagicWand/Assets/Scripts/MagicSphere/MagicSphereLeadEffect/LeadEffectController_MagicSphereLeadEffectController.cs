using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//なぞる誘導演出の誘導エフェクトをコントロールする機能

public partial class MagicSphereLeadEffectController
{
    [System.Serializable]
    class LeadEffectController
    {
        [SerializeField]
        LeadEffect _leadEffectPrefab;

        [Tooltip("魔法陣の中心位置")] [SerializeField]
        Transform _magicCircleCenter;

        [SerializeField]
        MagicInvoker magicInvoker;

        MagicSpheresList _magicSpheresList;
        MagicList _magicList;

        public void Awake(MagicSpheresList magicSpheresList,MagicList spellCastList)
        {
            _magicSpheresList = magicSpheresList;
            _magicList = spellCastList;
        }

        //誘導エフェクトを作成
        public void CreateLeadEffect(int? preActiveSphereIndex, EMagic[] currentTrace_SelectableMagics, float activeDuration)
        {
            //誘導エフェクトのスタート地点を求める
            Vector3 start = CalcStartPos(preActiveSphereIndex);

            //誘導エフェクトの実体化
            InstantiateLeadEffect(currentTrace_SelectableMagics, start,activeDuration);
        }

        Vector3 CalcStartPos(int? preActiveSphereIndex)
        {
            if (preActiveSphereIndex == null)
            {
                return _magicCircleCenter.position;
            }
            else
            {
                return _magicSpheresList.MagicSphereObjects[(int)preActiveSphereIndex].transform.position;
            }
        }

        Vector3 CalcEndPos(int nextActiveSphereIndex)
        {
            return _magicSpheresList.MagicSphereObjects[nextActiveSphereIndex].transform.position;
        }

        void InstantiateLeadEffect(EMagic[] currentSelectableMagics, Vector3 start,float activeDuration)
        {
            for (int sphereIndex = 0; sphereIndex < currentSelectableMagics.Length; sphereIndex++)
            {
                EMagic selectableMagic = currentSelectableMagics[sphereIndex];

                //選択できる魔法が無ければスキップ
                if (selectableMagic == EMagic.None) continue;

                //パーティクルのエミッションの色を取得する
                var leadEffectProperty = _magicList.GetComponentFromMagic<LeadEffectProperty>(selectableMagic);

                if (leadEffectProperty == null) continue;

                Color leadEffectEmissionColor = leadEffectProperty.LeadEffectEmissionColor;

                //終点を求める
                Vector3 end = CalcEndPos(sphereIndex);

                var leadEffectInstance = Instantiate(_leadEffectPrefab);
                leadEffectInstance.Initialize(activeDuration,start, end,leadEffectEmissionColor,magicInvoker);
            }
        }
    }
}
