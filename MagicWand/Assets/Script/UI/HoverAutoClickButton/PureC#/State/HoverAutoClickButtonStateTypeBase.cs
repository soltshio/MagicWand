using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンのステートの基底クラス

public interface HoverAutoClickButtonStateTypeBase
{
    public void SetStateMachine(HoverAutoClickButtonStateMachine stateMachine);//ステートマシンをセット
    public void OnEnter(HoverAutoClickButtonParameter parameter);//ステートに入ったときの処理
    public void OnUpdate(HoverAutoClickButtonParameter parameter);//ステートにいるときの処理
    public void OnExit(HoverAutoClickButtonParameter parameter);//ステートから出るときの処理
}
