using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンのテスト

[RequireComponent(typeof(Button))]
public class HoverAutoClickButton_Test : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    [Tooltip("何秒間カーソルを合わせていると自動クリックされるか")] [SerializeField]
    private float _hoverDurationToClick = 2f;

    [SerializeField]
    private float _pressDuration = 0.1f;

    public event Action OnAutoClick; // 自動クリックされたときのイベント
    public event Action OnCursorEnter; // マウスカーソルがボタンに合わさったときのイベント
    public event Action OnCursorExit; // マウスカーソルがボタンから離れたときのイベント

    private bool _isHovering=false;//マウスカーソルがボタンに合わさっているか
    private bool _hasClicked=false;//クリックされたか
    private float _hoveringTime=0f;//カーソルが合わさっている時間

    public float HoveringTime { get => _hoveringTime; }//カーソルが合わさっている時間
    public float HoverDurationToClick { get => _hoverDurationToClick; }//何秒間カーソルを合わせていると自動クリックされるか

    private void Update()
    {
        if (!_isHovering || _hasClicked) return;


        _hoveringTime += Time.deltaTime;

        if (_hoveringTime >= _hoverDurationToClick)
        {
            StartCoroutine(AutoClickCoroutine());

            _hasClicked = true;
        }
    }

    private IEnumerator AutoClickCoroutine()
    {
        PointerEventData eventData =
            new PointerEventData(EventSystem.current);

        eventData.pointerEnter = gameObject;
        eventData.pointerPress = gameObject;

        // 押す
        ExecuteEvents.Execute(
            gameObject,
            eventData,
            ExecuteEvents.pointerDownHandler);

        // 押されている時間
        yield return new WaitForSeconds(_pressDuration);

        // 離す
        ExecuteEvents.Execute(
            gameObject,
            eventData,
            ExecuteEvents.pointerUpHandler);

        // クリック成立
        ExecuteEvents.Execute(
            gameObject,
            eventData,
            ExecuteEvents.pointerClickHandler);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnCursorEnter?.Invoke();

        _isHovering = true;
        _hoveringTime = 0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnCursorExit?.Invoke();

        _isHovering = false;
        _hasClicked = false;
        _hoveringTime = 0f;
    }
}
