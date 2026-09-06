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
        MagicInvoker magicInvoker;

        MagicSpheresList _magicSpheresList;
        MagicList _magicList;

        public void Awake(MagicSpheresList magicSpheresList,MagicList spellCastList)
        {
            _magicSpheresList = magicSpheresList;
            _magicList = spellCastList;
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

        void InstantiateLeadEffect(List<(EMagic magic, int index)> activeSphereIndex_MagicList, Vector3 start,float activeDuration)
        {
            for (int i = 0; i < activeSphereIndex_MagicList.Count; i++)
            {
                //パーティクルのエミッションの色を取得する
                if (!TryGetSpellCast(activeSphereIndex_MagicList[i].magic, out var spellCast)) continue;
                Color leadEffectEmissionColor = spellCast.LeadEffectEmissionColor;

                //終点を求める
                Vector3 end = CalcEndPos(activeSphereIndex_MagicList[i].index);

                var leadEffectInstance = Instantiate(_leadEffectPrefab);
                leadEffectInstance.Initialize(activeDuration,start, end,leadEffectEmissionColor,magicInvoker);
            }
        }

        //特定魔法の詠唱の機能を取得、取得に失敗した場合はfalseを返す
        bool TryGetSpellCast(EMagic magic, out SpellCast spellCast)
        {
            spellCast = _magicList.GetComponentFromMagic<SpellCast>(magic);

            return spellCast != null;
        }
    }
}
