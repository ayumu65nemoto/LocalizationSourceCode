using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoyFaceSwitching : MonoBehaviour
{
    [SerializeField] Sprite _nomalFace; //�ʏ펞�̊�摜
    [SerializeField] Sprite _blowFace; //�@���������鎞�̊�摜

    [SerializeField] Sprite _winFace; //�������̊�摜
    [SerializeField] Sprite _loseFace; //�s�k���̊�摜

    private SpriteRenderer _currentSprite; //���ݕ\�����Ă���X�v���C�g

    [SerializeField] GameObject _runnyNoseGravityCenter; //�@���̏d�S
    private GravityCenterMove _gravityCenterMove; //�N���X

    [SerializeField] GameObject _runnyNoseManagerObj; //�Q�[���}�l�[�W���[
    private RunnyNoseManager _runnyNoseManager; //�N���X

    private bool _isStart = false; //��ăX�^�[�g�p�̕ϐ�
    public void SetIsStart(bool value)
    {
        _isStart = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        //SpriteRenderer�R���|�[�l���g���擾
        _currentSprite = transform.GetComponent<SpriteRenderer>();
        //�d�S�̃N���X���Q��
        _gravityCenterMove = _runnyNoseGravityCenter.GetComponent<GravityCenterMove>();
        //�Q�[���}�l�[�W���[�̃N���X���Q��
        _runnyNoseManager = _runnyNoseManagerObj.GetComponent<RunnyNoseManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //�@�����������Ă����ԂɂȂ�����
        if (_gravityCenterMove.GetSniffling() == true)
        {
            //�@�����������Ă����̃X�v���C�g�ɐ؂�ւ���
            StartCoroutine("FaceSwitching");    
        }

        ToggleToResultFace();
    }

    private void ToggleToResultFace()
    {
        //�������A�s�k���ɁA���ꂼ��̊�摜�ɐ؂�ւ���
        if (_runnyNoseManager.GetWinning() == true)
        {
            _currentSprite.sprite = _winFace;
        }
        else if (_runnyNoseManager.GetLosing() == true)
        {
            _currentSprite.sprite = _loseFace;
        }
    }

    /// <summary>
    /// �X�v���C�g�؂�ւ�
    /// </summary>
    /// <returns></returns>
    IEnumerator FaceSwitching()
    {
        //�Q�[���p�����Ȃ�
        if (!_runnyNoseManager.GetWinning() && !_runnyNoseManager.GetLosing())
        {
            //�@������ł����̃X�v���C�g�ɐ؂�ւ���
            _currentSprite.sprite = _blowFace;

            //0.2�b��~
            yield return new WaitForSeconds(0.1f);

            //���̃X�v���C�g�ɖ߂�
            _currentSprite.sprite = _nomalFace;

            //�@������ł��Ȃ���Ԃɖ߂�
            _gravityCenterMove.SetSniffling(false);
        }
    }
}
