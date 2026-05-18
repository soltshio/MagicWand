using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンの待機状態の振る舞い

public class HoverAutoClickButtonStateTypeIdle : HoverAutoClickButtonStateTypeBase, IPointerEnterHandler
{
    bool _finished = false;

    public void OnEnter(HoverAutoClickButtonStateMachine stateMachine, HoverAutoClickButtonParameter parameter)
   {
       
   }

   public void OnUpdate(HoverAutoClickButtonStateMachine stateMachine, HoverAutoClickButtonParameter parameter)
   {
        if (_finished)
        {
            stateMachine.ChangeState(HoverAutoClickButtonEState.Hovering);
        }
    }

   public void OnExit(HoverAutoClickButtonStateMachine stateMachine, HoverAutoClickButtonParameter parameter)
   {

   }

   public void OnPointerEnter(PointerEventData eventData)
   {
        _finished = true;
    }
}
