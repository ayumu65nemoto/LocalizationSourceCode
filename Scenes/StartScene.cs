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
        //BGM�Đ�
        SoundManager.Instance.PlayBGM(_bgm);

        //�X�^�[�g�{�^���̏�����ݒ�
        GameManager.Instance.SceneData.SetNextSceneType(SceneData.SceneType.Start);
        GameManager.Instance.DestinationData.SetCurrentPosition(1);
        GameManager.Instance.DestinationData.SetNextDestination(2);
        _switchSceneButton.OnClickStartButton.Subscribe(_ => GameManager.Instance.SceneData.BackToWorldMap());
    }
}
