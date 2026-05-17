using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

//作成者:杉山
//数秒間カーソルを合わせていると、自動クリックするボタンのテスト

[RequireComponent(typeof(Button))]
public class HoverAutoClickButton_Test : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    [Tooltip("何秒間カーソルを合わせていると自動クリックされるか")] [SerializeField]
    private float _hoverTime = 2f;

    [SerializeField]
    private float _pressDuration = 0.1f;

    private bool _isHovering=false;
    private bool _hasClicked=false;
    private float _timer=0f;

    private void Update()
    {
        if (!_isHovering || _hasClicked)
            return;

        _timer += Time.deltaTime;

        if (_timer >= _hoverTime)
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
        _isHovering = true;
        _timer = 0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;
        _hasClicked = false;
        _timer = 0f;
    }
}
