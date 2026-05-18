using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//作成者:杉山
//ボタン決定時に音を鳴らす
//ボタン本体につける

public class PlaySound_OnClick : MonoBehaviour, IPointerClickHandler, ISubmitHandler
{
    [SerializeField]
    AudioSource _audioSource;

    [Tooltip("interactableがtrueの時のみ効果音をならすようにしたいか")] [SerializeField]
    bool _playOnlyIsInteractable=false;

    [Tooltip("鳴らしたい音源")] [SerializeField] 
    AudioClip _clip;

    Selectable _selectable;

    void Awake()
    {
        if (_playOnlyIsInteractable)
        {
            _selectable = GetComponent<Selectable>();

            if(_selectable==null) Debug.Log("インタラクト可能なUIが見つかりません！");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsInteractable()) return;

        _audioSource.PlayOneShot(_clip);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (!IsInteractable()) return;

        _audioSource.PlayOneShot(_clip);
    }

    bool IsInteractable()
    {
        if(!_playOnlyIsInteractable) return true;

        if (_selectable == null)
        {
            Debug.Log("インタラクト可能なUIが見つかりません！");
            return false;
        }

        return _selectable.IsInteractable();
    }
}
    