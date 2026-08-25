using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

//作成者:杉山
//魔法陣の展開の処理をするクラス

public class MagicCircleDeploymentManager : MonoBehaviour
{
    [System.Serializable]
    class DeployButton
    {
        [SerializeField]
        HoverAutoClickButton _deployButton_HoverAutoClickButton;

        [SerializeField]
        Button _deployButton_Button;

        public event Action OnPushed;

        public void OnEnable()
        {
            _deployButton_Button.onClick.AddListener(()=>OnPushed());
        }

        public void OnDisable()
        {
            _deployButton_Button.onClick.RemoveListener(() => OnPushed());
        }

        public void SwitchButtonEnabled(bool enabled)
        {
            _deployButton_Button.enabled = enabled;
            _deployButton_HoverAutoClickButton.enabled = enabled;
        }

        public void SwitchVisible(bool isVisible)
        {
            _deployButton_Button.gameObject.SetActive(isVisible);
        }
    }

    [Tooltip("魔法陣のなぞった線を描画する機能")] [SerializeField]
    MagicSphereTrail _magicSphereTrail;

    [Tooltip("魔法陣の表示・非表示をする機能")] [SerializeField]
    MagicCircleActiveHandler _magicCircleActiveHandler;

    [SerializeField]
    DeployButton _deployButton;

    bool _isPushed = false;

    public async UniTask DeployAsync()
    {
        var token = this.GetCancellationTokenOnDestroy();

        _isPushed = false;

        //ボタンを表示・有効化
        _deployButton.SwitchVisible(true);
        _deployButton.SwitchButtonEnabled(true);

        //ボタンが押されるまで待つ
        await WaitForButtonPushedAsync(token);

        //一度ボタンを無効化
        _deployButton.SwitchButtonEnabled(false);

        //魔法陣を表示(展開)する
        await DeployMagicCircleAsync(token);

        //ボタンを非表示
        _deployButton.SwitchVisible(false);
    }

    async UniTask WaitForButtonPushedAsync(CancellationToken ct)
    {
        _deployButton.OnPushed += ReceiveIsPushed;

        await UniTask.WaitUntil(() => _isPushed, cancellationToken:ct);

        _deployButton.OnPushed -= ReceiveIsPushed;
    }

    async UniTask DeployMagicCircleAsync(CancellationToken ct)
    {
        //魔法陣の線を消す
        _magicSphereTrail.ResetTrail();

        //魔法陣を表示する
        await _magicCircleActiveHandler.ActivateMagicCircleAsync(ct);
    }

    void ReceiveIsPushed()
    {
        _isPushed = true;
    }

    void OnEnable()
    {
        _deployButton.OnEnable();
    }

    void OnDisable()
    {
        _deployButton.OnDisable();
    }
}
