using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンのステートマシン

public class HoverAutoClickButtonStateMachine : IPointerEnterHandler, IPointerExitHandler
{
    HoverAutoClickButtonStateTypeBase _currentState;
    HoverAutoClickButtonEState _currentEState;

    public HoverAutoClickButtonEState CurrentState { get => _currentEState; }

    public HoverAutoClickButtonStateMachine()
    {
        _currentEState = HoverAutoClickButtonEState.Idle;
    }

    public void ChangeState(HoverAutoClickButtonEState newState)
    {
        _currentEState = newState;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
