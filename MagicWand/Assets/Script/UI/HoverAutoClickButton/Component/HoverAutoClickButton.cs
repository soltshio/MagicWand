using System;
using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタン

public class HoverAutoClickButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField]
    private HoverAutoClickButtonParameter _parameter;

    HoverAutoClickButtonStateMachine _stateMachine;

    public event Action<HoverAutoClickButtonEState> OnStateChanged;// 状態が変化したときのイベント

    public HoverAutoClickButtonParameter Parameter { get => _parameter; }//パラメーター
    public HoverAutoClickButtonEState CurrentState { get => _stateMachine.CurrentState; }//現在の状態
    public float HoveringTime { get => _stateMachine.HoveringTime; }//カーソルが合わさっている時間

    void Awake()
    {
        _stateMachine = new HoverAutoClickButtonStateMachine(_parameter);
    }

    void OnEnable()
    {
        _stateMachine.OnStateChanged += OnStateChangedTrigger;
    }

    void OnDisable()
    {
        _stateMachine.OnStateChanged -= OnStateChangedTrigger;
    }

    void OnStateChangedTrigger(HoverAutoClickButtonEState state)
    {
        OnStateChanged?.Invoke(state);
    }

    void Update()
    {
        _stateMachine.Update();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _stateMachine.OnPointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _stateMachine.OnPointerExit(eventData);
    }

}
