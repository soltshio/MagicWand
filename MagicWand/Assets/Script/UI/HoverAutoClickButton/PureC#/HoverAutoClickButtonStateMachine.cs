using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンのステートマシン

public class HoverAutoClickButtonStateMachine : IPointerEnterHandler, IPointerExitHandler
{
    HoverAutoClickButtonStateTypeBase _currentState;
    HoverAutoClickButtonEState _currentEState;
    HoverAutoClickButtonParameter _parameter;

    Dictionary<HoverAutoClickButtonEState,HoverAutoClickButtonStateTypeBase> _stateDic;

    public event Action<HoverAutoClickButtonEState> OnStateChanged; // 状態が変化したときのイベント

    public HoverAutoClickButtonEState CurrentState { get => _currentEState; }
    public float HoveringTime//カーソルが合わさっている時間
    {
        get
        {
            if (_currentState is HoverAutoClickButtonStateTypeHovering hoveringState)
            {
                return hoveringState.HoveringTime;
            }
            else
            {
                return 0f;
            }
        }
    }


    public HoverAutoClickButtonStateMachine(HoverAutoClickButtonParameter parameter)
    {
        _parameter = parameter;

        _stateDic = new Dictionary<HoverAutoClickButtonEState, HoverAutoClickButtonStateTypeBase>()
        {
             { HoverAutoClickButtonEState.Idle, new HoverAutoClickButtonStateTypeIdle() },
             { HoverAutoClickButtonEState.Hovering, new HoverAutoClickButtonStateTypeHovering() },
             { HoverAutoClickButtonEState.Clicked, new HoverAutoClickButtonStateTypeClicked() },
        };

        _currentEState = HoverAutoClickButtonEState.Idle;
        ChangeState(_currentEState);
    }

    //毎フレーム処理
    public void Update()
    {
        _currentState.OnUpdate(this,_parameter);
    }

    //ステート変更
    public void ChangeState(HoverAutoClickButtonEState newState)
    {
        if (!_stateDic.TryGetValue(newState, out var newStateInstance)) return;

        if(_currentState != null) _currentState.OnExit(this,_parameter);

        //状態を変更
        _currentEState = newState;
        _currentState = newStateInstance;
        OnStateChanged?.Invoke(newState);

        if (_currentState != null) _currentState.OnEnter(this,_parameter);
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
