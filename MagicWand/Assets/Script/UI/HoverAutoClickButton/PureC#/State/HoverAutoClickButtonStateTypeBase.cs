using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンのステートの基底クラス

public interface HoverAutoClickButtonStateTypeBase
{
    public void OnEnter(HoverAutoClickButtonStateMachine stateMachine);//ステートに入ったときの処理
    public void OnUpdate(HoverAutoClickButtonStateMachine stateMachine);//ステートにいるときの処理
    public void OnExit(HoverAutoClickButtonStateMachine stateMachine);//ステートから出るときの処理
}
