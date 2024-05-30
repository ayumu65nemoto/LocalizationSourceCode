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
        //�X�^�[�g�{�^���̏�����ݒ�
        GameManager.Instance.SceneData.SetNextSceneType(SceneData.SceneType.Start);
        _switchSceneButton.OnClickStartButton.Subscribe(_ => GameManager.Instance.SceneData.BackToWorldMap());
    }
}
