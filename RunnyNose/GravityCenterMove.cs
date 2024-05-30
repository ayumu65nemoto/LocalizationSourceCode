using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCenterMove : MonoBehaviour
{
    private Rigidbody2D _rb; //���g�̃��W�b�h�{�f�B

    private bool _sniffling; //�@���������Ă��邩�ǂ���
    public bool GetSniffling()
    {
        return _sniffling;
    }
    public void SetSniffling(bool value)
    {
        _sniffling = value;
    }

    [SerializeField] GameObject _gameManager; //�Q�[���}�l�[�W���[
    private RunnyNoseManager _runnyNoseManager; //�N���X

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); //Rigidbody2D�R���|�[�l���g���擾

        SetSniffling(false);

        _runnyNoseManager = _gameManager.GetComponent<RunnyNoseManager>(); //�Q�[���}�l�[�W���[�̃N���X���Q��
    }

    void Update()
    {
        //�Q�[���I�����i����or�s�k�j�ӊO�ł͕@���̑�����\�ɂ���
        if(!_runnyNoseManager.GetLosing() && !_runnyNoseManager.GetWinning())
        {
            RunnyNoseJump(); //�@�������������̓���
        }
    }

    /// <summary>
    /// �@�������������̓���
    /// </summary>
    private void RunnyNoseJump()
    {
        //�X�y�[�X�L�[����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //��x������Ă���͂����Z�b�g
            _rb.velocity = Vector3.zero;

            //������ɏu�ԓI�ɗ͂�������
            _rb.AddForce(Vector3.up * 5, ForceMode2D.Impulse);

            //�@�����������Ă����ԂɈڍs
            SetSniffling(true);
        }
    }
}
