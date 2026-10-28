using UnityEngine;

//作成者:杉山
//発動開始のコマンドを送るマネージャー
//魔法陣を閉じる時の紋章が出てくるところを発動開始とする

public class SendStartInvokeCommandManager : MonoBehaviour
{
    [SerializeField]
    MagicCircleCloseManager _magicCircleCloseManager;

    [SerializeField]
    SendManager _sendManager;


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
        switch (castMagic)
        {
            case EMagic.Sun: _sendManager.AddMessage(EMiconCommand.InvokeSun); break;
            case EMagic.Star: _sendManager.AddMessage(EMiconCommand.InvokeStar); break;
            case EMagic.Thunder: _sendManager.AddMessage(EMiconCommand.InvokeThunder); break;
            case EMagic.Rain: _sendManager.AddMessage(EMiconCommand.InvokeWater); break;
            case EMagic.Time: _sendManager.AddMessage(EMiconCommand.InvokeTime); break;
        }
    }
}
