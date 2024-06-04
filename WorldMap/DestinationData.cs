using System.Collections.Generic;
using UnityEngine;

public class DestinationData
{
    private List<Vector2> _destinationList = new List<Vector2>();
    private List<string> _destinationStringList = new List<string>();

    public Vector2 CurrentPosition { get; private set; }    //���݂̔�s�@�ʒu
    public Vector2 NextDestination { get; private set; }    //���̔�s�@�ʒu
    public string CurrentPositionName { get; private set; } //���݂̍��̖��O
    public string NextDestinationName { get; private set; } //���̍��̖��O

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public DestinationData()
    {
        //�ړI�n�f�[�^�쐬
        _destinationList = new()
        {
            new Vector2(6.51f, 0.18f),
            new Vector2(6.51f, 0.18f),  //���{
            new Vector2(-0.79f, 1.41f), //�C�M���X
            //new Vector2(-5.42f, 1f),    //�A�����J
            new Vector2(-0.2f, 1.22f),  //�h�C�c
            new Vector2(0.42f, 0.29f)   //�M���V��
        };

        //�����f�[�^�쐬�i��L�ړI�n�ɍ��킹��悤�ɁI�I�j
        _destinationStringList = new()
        {
            "",
            "���{",
            "�C�M���X",
            //"�A�����J",
            "�h�C�c",
            "�M���V��"
        };

        //�����l��ݒ�
        CurrentPosition = _destinationList[1];
        NextDestination = _destinationList[2];
        CurrentPositionName = _destinationStringList[1];
        NextDestinationName = _destinationStringList[2];
    }

    /// <summary>
    /// ���݂̔�s�@�ʒu��ݒ�
    /// </summary>
    /// <param name="num">�V�[���^�C�v�̔ԍ�</param>
    public void SetCurrentPosition(int num)
    {
        CurrentPosition = _destinationList[num];
        CurrentPositionName = _destinationStringList[num];
    }

    /// <summary>
    /// ���̔�s�@�ʒu��ݒ�
    /// </summary>
    /// <param name="num">�V�[���^�C�v�̔ԍ�</param>
    public void SetNextDestination(int num)
    {
        NextDestination = _destinationList[num];
        NextDestinationName = _destinationStringList[num];
    }
}
