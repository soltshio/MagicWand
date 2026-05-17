using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンのステートマシン

public class HoverAutoClickButtonStateMachine : IPointerEnterHandler, IPointerExitHandler
{
    HoverAutoClickButtonStateTypeBase _currentState;
    HoverAutoClickButtonEState _currentEState;

    Dictionary<HoverAutoClickButtonEState,HoverAutoClickButtonStateTypeBase> _stateDic;

    public HoverAutoClickButtonEState CurrentState { get => _currentEState; }

    public HoverAutoClickButtonStateMachine()
    {
            //_stateDic = new Dictionary<HoverAutoClickButtonEState, HoverAutoClickButtonStateTypeBase>()
            //{
            //    { HoverAutoClickButtonEState.Idle, new HoverAutoClickButtonStateTypeIdle() },
            //    { HoverAutoClickButtonEState.Hovering, new HoverAutoClickButtonStateTypeHovering() },
            //    { HoverAutoClickButtonEState.Clicked, new HoverAutoClickButtonStateTypeClicked() },
            //};

        _currentEState = HoverAutoClickButtonEState.Idle;
    }

    public void ChangeState(HoverAutoClickButtonEState newState)
    {
        _currentEState = newState;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_currentState is IPointerEnterHandler pointerEnterHandler)
        {
            pointerEnterHandler.OnPointerEnter(eventData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_currentState is IPointerExitHandler pointerExitHandler)
        {
            pointerExitHandler.OnPointerExit(eventData);
        }
    }
}
