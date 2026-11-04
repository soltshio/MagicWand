using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//詠唱関係のコマンドを送るマネージャー

public class SendCastCommandManager : MonoBehaviour
{
    [SerializeField]
    MagicCircleCastManager _magicCircleCastManager;

    SendManager _sendManagerInstance;

    const int _startCastMagicCount = 1;

    async void Start()
    {
        await UniTask.WaitUntil(() => SendManager.Instance != null, cancellationToken: this.GetCancellationTokenOnDestroy());

        _sendManagerInstance = SendManager.Instance;
    }

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
        if(_sendManagerInstance == null) return;

        //１詠唱中の最初に触れた球だった場合、魔法を始めたことをマイコンに通知
        if(touchSphereCount == _startCastMagicCount)
        {
            switch(castMagic)
            {
                case EMagic.Sun: _sendManagerInstance.AddMessage(EMiconCommand.StartSun); break;
                case EMagic.Star: _sendManagerInstance.AddMessage(EMiconCommand.StartStar); break;
                case EMagic.Thunder: _sendManagerInstance.AddMessage(EMiconCommand.StartThunder); break;
                case EMagic.Rain: _sendManagerInstance.AddMessage(EMiconCommand.StartWater); break;
                case EMagic.Time: _sendManagerInstance.AddMessage(EMiconCommand.StartTime); break;
            }
        }

        //詠唱の状態をマイコンに通知
        int castLevel = touchSphereCount - 1;

        switch (castMagic)
        {
            case EMagic.Sun: _sendManagerInstance.AddMessage(EMiconCommand.CastSun,castLevel); break;
            case EMagic.Star: _sendManagerInstance.AddMessage(EMiconCommand.CastStar,castLevel); break;
            case EMagic.Thunder: _sendManagerInstance.AddMessage(EMiconCommand.CastThunder,castLevel); break;
            case EMagic.Rain: _sendManagerInstance.AddMessage(EMiconCommand.CastWater,castLevel); break;
            case EMagic.Time: _sendManagerInstance.AddMessage(EMiconCommand.CastTime,castLevel); break;
        }
    }
}
