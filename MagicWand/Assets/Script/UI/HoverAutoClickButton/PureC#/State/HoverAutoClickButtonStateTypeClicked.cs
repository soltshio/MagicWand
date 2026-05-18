using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンのクリックされた状態の振る舞い

public class HoverAutoClickButtonStateTypeClicked : HoverAutoClickButtonStateTypeBase, IPointerExitHandler
{
    bool _finished = false;

    public void OnEnter(HoverAutoClickButtonStateMachine stateMachine, HoverAutoClickButtonParameter parameter)
    {
        _finished = false;
        AutoClickAsync(parameter).Forget();
    }

    public void OnUpdate(HoverAutoClickButtonStateMachine stateMachine, HoverAutoClickButtonParameter parameter)
    {
        if(_finished)
        {
            stateMachine.ChangeState(HoverAutoClickButtonEState.Idle);
        }
    }

    public void OnExit(HoverAutoClickButtonStateMachine stateMachine, HoverAutoClickButtonParameter parameter)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _finished = true;
    }

    private async UniTask AutoClickAsync(HoverAutoClickButtonParameter parameter)
    {
        PointerEventData eventData =
            new PointerEventData(EventSystem.current);

        eventData.pointerEnter = parameter.ButtonUIObject;
        eventData.pointerPress = parameter.ButtonUIObject;

        // 押す
        ExecuteEvents.Execute(
            parameter.ButtonUIObject,
            eventData,
            ExecuteEvents.pointerDownHandler);

        // 押されている時間
        await UniTask.Delay(
            TimeSpan.FromSeconds(parameter.PressDuration));

        if (parameter.ButtonUIObject == null) return;//待ってる間にシーン遷移などによりオブジェクトが消えてしまった時にエラーにならないように

        // 離す
        ExecuteEvents.Execute(
            parameter.ButtonUIObject,
            eventData,
            ExecuteEvents.pointerUpHandler);

        // クリック成立
        ExecuteEvents.Execute(
            parameter.ButtonUIObject,
            eventData,
            ExecuteEvents.pointerClickHandler);
    }
}
