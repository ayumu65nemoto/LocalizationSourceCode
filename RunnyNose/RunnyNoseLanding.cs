using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RunnyNoseLanding : MonoBehaviour
{
    [SerializeField] GameObject _runnyNoseImg; //�@���摜
    [SerializeField] GameObject _runnyNoseGravityCenter; //�@���̏d�S

    private Vector3 _imageScale; //�@���摜�̑傫��

    private Vector3 _firstGravityCenterPosition; //�ŏ��̏d�S�̈ʒu
    private Vector3 _currentGravityCenterPosition; //���݂̏d�S�̈ʒu

    [SerializeField] GameObject _gameManager; //�Q�[���}�l�[�W���[
    private RunnyNoseManager _runnyNoseManager; //�N���X

    // Start is called before the first frame update
    void Start()
    {
        _imageScale = _runnyNoseImg.transform.localScale; //�@���̃T�C�Y

        _firstGravityCenterPosition = _runnyNoseGravityCenter.transform.position; //�ŏ��̏d�S�̈ʒu��ۑ�

        _runnyNoseManager = _gameManager.GetComponent<RunnyNoseManager>(); //�Q�[���}�l�[�W���[�̃N���X���Q��
    }

    // Update is called once per frame
    void Update()
    {
        ExpansionRunnyNose(); //�L�тĂ����@����\��

        if(_imageScale.y < 0)
        {
            _imageScale.y = 0;
        }

        if (_runnyNoseManager.GetWinning())
        {
            //�d�S�̈ʒu�ŕ@���̃X�P�[�������킹��
            _runnyNoseImg.transform.localScale = new Vector3(_imageScale.x, 0.4f, _imageScale.z);

            transform.DORotate(new Vector3(0, 0, 360), 2, RotateMode.WorldAxisAdd);

        }
        else
        {
            //�d�S�̈ʒu�ŕ@���̃X�P�[�������킹��
            _runnyNoseImg.transform.localScale = new Vector3(_imageScale.x, _imageScale.y / 15.0f, _imageScale.z);
        }
    }

    /// <summary>
    /// �d�S�ƕ@���T�C�Y�̓���
    /// �L�тĂ����@����\��
    /// </summary>
    private void ExpansionRunnyNose()
    {
        //�d�S�̏����ʒu�ɂ��@���̃T�C�Y����
        if (_firstGravityCenterPosition.y < 0)
        {
            _imageScale.y = -_runnyNoseGravityCenter.transform.position.y - _firstGravityCenterPosition.y;
        }
        else if (_firstGravityCenterPosition.y > 0)
        {
            _imageScale.y = -_runnyNoseGravityCenter.transform.position.y + _firstGravityCenterPosition.y;
        }
        else if (_firstGravityCenterPosition.y == 0)
        {
            _imageScale.y = -_runnyNoseGravityCenter.transform.position.y;
        }
    }
}
