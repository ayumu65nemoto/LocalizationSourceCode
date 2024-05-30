using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StartScene : MonoBehaviour
{
    [SerializeField]
    private SwitchSceneButton _switchSceneButton;

    private void Start()
    {
        //スタートボタンの処理を設定
        GameManager.Instance.SceneData.SetNextSceneType(SceneData.SceneType.Start);
        _switchSceneButton.OnClickStartButton.Subscribe(_ => GameManager.Instance.SceneData.BackToWorldMap());
    }
}
