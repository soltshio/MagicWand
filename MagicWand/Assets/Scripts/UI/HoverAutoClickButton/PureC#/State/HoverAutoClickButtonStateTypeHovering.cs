using UnityEngine;
using UnityEngine.EventSystems;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンのカーソルが合っている状態の振る舞い

public class HoverAutoClickButtonStateTypeHovering : HoverAutoClickButtonStateTypeBase, IPointerExitHandler
{
    private float _hoveringTime = 0f;//カーソルが合わさっている時間
    HoverAutoClickButtonStateMachine _stateMachine;

    public float HoveringTime { get => _hoveringTime; }//カーソルが合わさっている時間

    public void SetStateMachine(HoverAutoClickButtonStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void OnEnter(HoverAutoClickButtonParameter parameter)
    {
        _hoveringTime = 0f;

        var hoveringAudioSource = parameter.HoveringAudioSource;
        if(hoveringAudioSource != null) hoveringAudioSource.Play();
    }

    public void OnUpdate(HoverAutoClickButtonParameter parameter)
    {
        //数秒間カーソルが合っていたら、クリック状態に遷移
        _hoveringTime += Time.deltaTime;

        if (_hoveringTime < parameter.HoverDurationToClick) return;
        
        _stateMachine.ChangeState(HoverAutoClickButtonEState.Clicked);
    }

    public void OnExit(HoverAutoClickButtonParameter parameter)
    {
        var hoveringAudioSource = parameter.HoveringAudioSource;
        if (hoveringAudioSource != null) hoveringAudioSource.Stop();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _stateMachine.ChangeState(HoverAutoClickButtonEState.Idle);
    }
}
