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
        _fadeInOutPanel.FadeTrigger(false);

        await UniTask.Delay(TimeSpan.FromSeconds(2));

        _fadeInOutPanel.FadeTrigger(true);

        await UniTask.Delay(TimeSpan.FromSeconds(1));

        Destroy(_fadeInOutPanel.gameObject);
    }
}
