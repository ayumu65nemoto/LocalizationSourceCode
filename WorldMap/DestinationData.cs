using System.Collections.Generic;
using UnityEngine;

public class DestinationData
{
    private List<Vector2> _destinationList = new List<Vector2>();

    public Vector2 CurrentPosition { get; private set; }    //���݂̔�s�@�ʒu
    public Vector2 NextDestination { get; private set; }    //���̔�s�@�ʒu

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public DestinationData()
    {
        //�ړI�n�f�[�^�쐬
        _destinationList = new()
        {
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(6.51f, 0.18f),
            new Vector2(4.2f, 2.5f)
        };

        //�����l��ݒ�
        CurrentPosition = _destinationList[1];
        NextDestination = _destinationList[2];
    }

    /// <summary>
    /// ���݂̔�s�@�ʒu��ݒ�
    /// </summary>
    /// <param name="num">�V�[���^�C�v�̔ԍ�</param>
    public void SetCurrentPosition(int num)
    {
        CurrentPosition = _destinationList[num];
    }

    /// <summary>
    /// ���̔�s�@�ʒu��ݒ�
    /// </summary>
    /// <param name="num">�V�[���^�C�v�̔ԍ�</param>
    public void SetNextDestination(int num)
    {
        NextDestination = _destinationList[num];
    }
}
