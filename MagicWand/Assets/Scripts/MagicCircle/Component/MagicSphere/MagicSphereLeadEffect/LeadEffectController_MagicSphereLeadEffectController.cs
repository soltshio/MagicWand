using System.Collections.Generic;
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
        float _leadEffectLifeTime = 7f;

        MagicSpheresList _magicSpheresList;

        public void Awake(MagicSpheresList magicSpheresList)
        {
            _magicSpheresList = magicSpheresList;
        }

        //誘導エフェクトを動かす前にする初期化
        public void InitLeadEffect(int? preActiveSphereIndex, List<(EMagic magic, int index)> activeSphereIndex_MagicList,float activeDuration)
        {
            //誘導エフェクトのスタート地点を求める
            Vector3 start = CalcStartPos(preActiveSphereIndex);

            //誘導エフェクトの初期化
            InstantiateLeadEffect(activeSphereIndex_MagicList, start,activeDuration);
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

        LeadEffect[] InstantiateLeadEffect(List<(EMagic magic, int index)> activeSphereIndex_MagicList, Vector3 start,float activeDuration)
        {
            LeadEffect[] leadEffects = new LeadEffect[activeSphereIndex_MagicList.Count];

            for (int i = 0; i < activeSphereIndex_MagicList.Count; i++)
            {
                //終点を求める
                Vector3 end = CalcEndPos(activeSphereIndex_MagicList[i].index);

                var leadEffectInstance = Instantiate(_leadEffectPrefab);
                leadEffectInstance.Initialize(activeDuration,start, end);
                Destroy(leadEffectInstance, _leadEffectLifeTime);

                leadEffects[i] = leadEffectInstance;
            }

            return leadEffects;
        }
    }
}
