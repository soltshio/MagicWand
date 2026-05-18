using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンのステートの基底クラス

public interface HoverAutoClickButtonStateTypeBase
{
    public void OnEnter(HoverAutoClickButtonStateMachine stateMachine, HoverAutoClickButtonParameter parameter);//ステートに入ったときの処理
    public void OnUpdate(HoverAutoClickButtonStateMachine stateMachine, HoverAutoClickButtonParameter parameter);//ステートにいるときの処理
    public void OnExit(HoverAutoClickButtonStateMachine stateMachine, HoverAutoClickButtonParameter parameter);//ステートから出るときの処理
}
