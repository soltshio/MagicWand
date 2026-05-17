using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンの待機状態の振る舞い

public class HoverAutoClickButtonStateTypeIdle : HoverAutoClickButtonStateTypeBase, IPointerEnterHandler
{
   public void OnEnter(HoverAutoClickButtonStateMachine stateMachine)
   {
       
   }

   public void OnUpdate(HoverAutoClickButtonStateMachine stateMachine)
   {
      
   }

   public void OnExit(HoverAutoClickButtonStateMachine stateMachine)
   {

   }

   public void OnPointerEnter(PointerEventData eventData)
   {
         
   }
}
