using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンのクリックされた状態の振る舞い

public class HoverAutoClickButtonStateTypeClicked : HoverAutoClickButtonStateTypeBase, IPointerExitHandler
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

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
