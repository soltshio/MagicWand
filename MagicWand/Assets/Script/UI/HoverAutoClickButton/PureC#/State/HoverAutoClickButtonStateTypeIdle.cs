using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンの待機状態の振る舞い

public class HoverAutoClickButtonStateTypeIdle : HoverAutoClickButtonStateTypeBase, IPointerEnterHandler
{
    HoverAutoClickButtonStateMachine _stateMachine;

    public void SetStateMachine(HoverAutoClickButtonStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnEnter(HoverAutoClickButtonParameter parameter)
   {
       
   }

   public void OnUpdate(HoverAutoClickButtonParameter parameter)
   {
    }

   public void OnExit(HoverAutoClickButtonParameter parameter)
   {

   }

   public void OnPointerEnter(PointerEventData eventData)
   {
        _stateMachine.ChangeState(HoverAutoClickButtonEState.Hovering);
    }
}
