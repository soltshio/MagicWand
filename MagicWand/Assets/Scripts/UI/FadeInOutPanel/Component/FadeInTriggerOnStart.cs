using System;
using Unity.VisualScripting;
using UnityEngine;

//作成者:杉山
//シーン開始時にフェードインを開始する

public class FadeInTriggerOnStart : MonoBehaviour
{
    [SerializeField] 
    FadeInOutPanel _fadeInOutPanel;

    void Start()
    {
        _fadeInOutPanel.FadeTrigger(FadeInOutPanel.FadeEType.FadeIn);
    }
}
