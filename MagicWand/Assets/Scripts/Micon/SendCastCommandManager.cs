using UnityEngine;

//作成者:杉山
//詠唱関係のコマンドを送るマネージャー

public class SendCastCommandManager : MonoBehaviour
{
    [SerializeField]
    MagicCircleCastManager _magicCircleCastManager;

    [SerializeField]
    SendManager _sendManager;

    const int _startCastMagicCount = 1;

    void OnEnable()
    {
        _magicCircleCastManager.OnSuccessToCast += SendCastStateToMicon;
    }

    void OnDisable()
    {
        _magicCircleCastManager.OnSuccessToCast -= SendCastStateToMicon;
    }

    void SendCastStateToMicon(EMagic castMagic, int touchedMagicSphereindex, int touchSphereCount)
    {
        //１詠唱中の最初に触れた球だった場合、魔法を始めたことをマイコンに通知
        if(touchSphereCount == _startCastMagicCount)
        {
            switch(castMagic)
            {
                case EMagic.Sun: _sendManager.AddMessage(EMiconCommand.StartSun); break;
                case EMagic.Star: _sendManager.AddMessage(EMiconCommand.StartStar); break;
                case EMagic.Thunder: _sendManager.AddMessage(EMiconCommand.StartThunder); break;
                case EMagic.Rain: _sendManager.AddMessage(EMiconCommand.StartWater); break;
                case EMagic.Time: _sendManager.AddMessage(EMiconCommand.StartTime); break;
            }
        }

        //詠唱の状態をマイコンに通知
        int castLevel = touchSphereCount - 1;

        switch (castMagic)
        {
            case EMagic.Sun: _sendManager.AddMessage(EMiconCommand.CastSun,castLevel); break;
            case EMagic.Star: _sendManager.AddMessage(EMiconCommand.CastStar,castLevel); break;
            case EMagic.Thunder: _sendManager.AddMessage(EMiconCommand.CastThunder,castLevel); break;
            case EMagic.Rain: _sendManager.AddMessage(EMiconCommand.CastWater,castLevel); break;
            case EMagic.Time: _sendManager.AddMessage(EMiconCommand.CastTime,castLevel); break;
        }
    }
}
