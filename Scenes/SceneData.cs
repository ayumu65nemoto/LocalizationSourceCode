using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneData
{
	private Dictionary<SceneType, string> _sceneTypeDict;   //�V�[���^�C�v�ƃV�[�����̕R�Â�

	public enum SceneType   //�V�[���̃^�C�v
	{
		Start = 0,	//�X�^�[�g���
		WorldMap = 1,	//���[���h�}�b�v
		CallTheClerk = 2,   //�X�����Ă�
		RunnyNose = 3,	//�@���Q�[��
	}
	public SceneType CurrentSceneType { get; private set; } //���݂̃V�[���^�C�v
	public SceneType NextSceneType { get; private set; }	//���̃V�[���^�C�v

	/// <summary>
	/// �R���X�g���N�^
	/// �V�[���^�C�v��Dictionary��string�R�Â�
	/// </summary>
	public SceneData()
    {
		_sceneTypeDict = new()
		{
			[SceneType.Start] = "Start",
			[SceneType.WorldMap] = "WorldMap",
			[SceneType.CallTheClerk] = "CallTheClerk",
			[SceneType.RunnyNose] = "RunnyNose"
		};

		//�����l�ݒ�
		CurrentSceneType = SceneType.Start;
    }

	/// <summary>
	/// �V�[���̓ǂݍ���
	/// </summary>
	/// <param name="sceneType">�J�ڂ������V�[���^�C�v</param>
	public void LoadScene(SceneType sceneType)
    {
		//���݂̃V�[���^�C�v���X�V
		CurrentSceneType = sceneType;
		//�V�[�������[�h
		SceneManager.LoadScene(_sceneTypeDict[sceneType]);
    }

	/// <summary>
	/// ���̃V�[���^�C�v���Z�b�g����
	/// </summary>
	/// <param name="current">���݂̃V�[���^�C�v</param>
	public void SetNextSceneType(SceneType current)
	{
		// SceneType�̑S�Ă̒l��z��Ɏ擾
		SceneType[] values = (SceneType[])Enum.GetValues(typeof(SceneType));

		// ���݂̒l�̃C���f�b�N�X���擾
		int currentIndex = Array.IndexOf(values, current);

		// ���̃C���f�b�N�X���v�Z
		int nextIndex = currentIndex + 1;

		// ���̃C���f�b�N�X���z��͈͓̔����ǂ������m�F
		if (nextIndex < values.Length)
		{
			NextSceneType =  values[nextIndex];
		}
        else
        {
			NextSceneType = SceneType.WorldMap;
        }
	}

	/// <summary>
	/// ���[���h�}�b�v�ɖ߂鏈��
	/// </summary>
	public void BackToWorldMap()
    {
		LoadScene(SceneType.WorldMap);
    }
}
