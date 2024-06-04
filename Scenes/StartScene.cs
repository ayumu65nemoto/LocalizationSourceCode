using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StartScene : MonoBehaviour
{
    [SerializeField]
    private SwitchSceneButton _switchSceneButton;
    [SerializeField]
    private AudioClip _bgm;

    private void Start()
    {
        //BGM再生
        SoundManager.Instance.PlayBGM(_bgm);

        //スタートボタンの処理を設定
        GameManager.Instance.SceneData.SetNextSceneType(SceneData.SceneType.Start);
        GameManager.Instance.DestinationData.SetCurrentPosition(1);
        GameManager.Instance.DestinationData.SetNextDestination(2);
        _switchSceneButton.OnClickStartButton.Subscribe(_ => GameManager.Instance.SceneData.BackToWorldMap());
    }
}
