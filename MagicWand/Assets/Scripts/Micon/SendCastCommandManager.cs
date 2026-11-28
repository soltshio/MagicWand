using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//詠唱関係のコマンドを送るマネージャー

public class SendCastCommandManager : MonoBehaviour
{
    [SerializeField]
    MagicCircleCastManager_New _magicCircleCastManager;

    [SerializeField]
    MagicCircleSelectManager _magicCircleSelectManager;

    SendManager _sendManagerInstance;

    const int _startCastMagicCount = 1;

    async void Start()
    {
        await UniTask.WaitUntil(() => SendManager.Instance != null, cancellationToken: this.GetCancellationTokenOnDestroy());

        _sendManagerInstance = SendManager.Instance;
    }

    void OnEnable()
    {
        _magicCircleCastManager.OnTraceMagicSphere += SendCastStateToMicon;
        _magicCircleSelectManager.OnSelectMagic += SendSelectMagicToMicon;
    }

    void OnDisable()
    {
        _magicCircleCastManager.OnTraceMagicSphere -= SendCastStateToMicon;
        _magicCircleSelectManager.OnSelectMagic -= SendSelectMagicToMicon;
    }

    void SendSelectMagicToMicon(EMagic selectedMagic,int selectedSphereIndex)
    {
        if (_sendManagerInstance == null) return;

        const int castLevel = 0;

        //魔法の詠唱を始めたことをマイコンに通知した直後に現在の詠唱の進行度(一番最初なのでレベルは０)を送る
        switch (selectedMagic)
        {
            case EMagic.Sun: 
                _sendManagerInstance.AddMessage(EMiconCommand.StartSun);
                _sendManagerInstance.AddMessage(EMiconCommand.CastSun, castLevel);
                break;

            case EMagic.Star:
                _sendManagerInstance.AddMessage(EMiconCommand.StartStar);
                _sendManagerInstance.AddMessage(EMiconCommand.CastStar, castLevel);
                break;

            case EMagic.Thunder:
                _sendManagerInstance.AddMessage(EMiconCommand.StartThunder);
                _sendManagerInstance.AddMessage(EMiconCommand.CastThunder, castLevel);
                break;

            case EMagic.Rain:
                _sendManagerInstance.AddMessage(EMiconCommand.StartWater);
                _sendManagerInstance.AddMessage(EMiconCommand.CastWater, castLevel);
                break;

            case EMagic.Time:
                _sendManagerInstance.AddMessage(EMiconCommand.StartTime);
                _sendManagerInstance.AddMessage(EMiconCommand.CastTime, castLevel);
                break;
        }
    }

    void SendCastStateToMicon(EMagic castMagic, int touchedMagicSphereindex, int touchSphereCount)
    {
        if(_sendManagerInstance == null) return;

        //詠唱の状態をマイコンに通知
        int castLevel = touchSphereCount;

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
