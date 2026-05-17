using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンのステートの基底クラス

public abstract class HoverAutoClickButtonStateTypeBase
{
    public abstract void OnEnter(HoverAutoClickButtonStateMachine stateMachine);//ステートに入ったときの処理
    public abstract void OnUpdate(HoverAutoClickButtonStateMachine stateMachine);//ステートにいるときの処理
    public abstract void OnExit(HoverAutoClickButtonStateMachine stateMachine);//ステートから出るときの処理
}
