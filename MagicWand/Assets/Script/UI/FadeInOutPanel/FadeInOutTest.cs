using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class FadeInOutTest : MonoBehaviour
{
    [SerializeField]
    FadeInOutPanel _fadeInOutPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.4f));

        _fadeInOutPanel.FadeTrigger(true);
    }
}
