using Cysharp.Threading.Tasks;
using UnityEngine;

//作成者:杉山
//発動開始のコマンドを送るマネージャー
//魔法陣を閉じる時の紋章が出てくるところを発動開始とする

public class SendStartInvokeCommandManager : MonoBehaviour
{
    [SerializeField]
    MagicCircleCloseManager _magicCircleCloseManager;

    SendManager _sendManagerInstance;

    async void Start()
    {
        await UniTask.WaitUntil(() => SendManager.Instance != null, cancellationToken: this.GetCancellationTokenOnDestroy());

        _sendManagerInstance = SendManager.Instance;
    }


    private void OnEnable()
    {
        _magicCircleCloseManager.OnStartClosingMagicCircle += SendStartInvokeToMicon;
    }

    private void OnDisable()
    {
        _magicCircleCloseManager.OnStartClosingMagicCircle -= SendStartInvokeToMicon;
    }

    void SendStartInvokeToMicon(EMagic castMagic)
    {
        if(_sendManagerInstance == null) return;

        switch (castMagic)
        {
            case EMagic.Sun: _sendManagerInstance.AddMessage(EMiconCommand.InvokeSun); break;
            case EMagic.Star: _sendManagerInstance.AddMessage(EMiconCommand.InvokeStar); break;
            case EMagic.Thunder: _sendManagerInstance.AddMessage(EMiconCommand.InvokeThunder); break;
            case EMagic.Rain: _sendManagerInstance.AddMessage(EMiconCommand.InvokeWater); break;
            case EMagic.Time: _sendManagerInstance.AddMessage(EMiconCommand.InvokeTime); break;
        }
    }
}
