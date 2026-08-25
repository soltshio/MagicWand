using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンのクリックされた状態の振る舞い

public class HoverAutoClickButtonStateTypeClicked : HoverAutoClickButtonStateTypeBase, IPointerExitHandler
{
    HoverAutoClickButtonStateMachine _stateMachine;

    public void SetStateMachine(HoverAutoClickButtonStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnEnter(HoverAutoClickButtonParameter parameter)
    {
        AutoClickAsync(parameter).Forget();
    }

    public void OnUpdate(HoverAutoClickButtonParameter parameter)
    {

    }

    public void OnExit(HoverAutoClickButtonParameter parameter)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _stateMachine.ChangeState(HoverAutoClickButtonEState.Idle);
    }

    private async UniTask AutoClickAsync(HoverAutoClickButtonParameter parameter)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);

        eventData.pointerEnter = parameter.ButtonUIObject;
        eventData.pointerPress = parameter.ButtonUIObject;

        // 押す
        ExecuteEvents.Execute(
            parameter.ButtonUIObject,
            eventData,
            ExecuteEvents.pointerDownHandler);

        var token = parameter.ButtonUIObject.GetCancellationTokenOnDestroy();

        // 押されている時間
        await UniTask.Delay(TimeSpan.FromSeconds(parameter.PressDuration), cancellationToken: token);

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
