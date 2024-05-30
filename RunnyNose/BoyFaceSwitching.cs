using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoyFaceSwitching : MonoBehaviour
{
    [SerializeField] Sprite _nomalFace; //���ʂ̊�摜
    [SerializeField] Sprite _blowFace; //�ȂȂ߂̊�摜

    [SerializeField] Sprite _winFace; //�������̊�摜
    [SerializeField] Sprite _loseFace; //�s�k���̊�摜

    private SpriteRenderer _currentSprite; //���ݕ\�����Ă���X�v���C�g

    [SerializeField] GameObject _runnyNoseGravityCenter; //�@���̏d�S
    private GravityCenterMove _gravityCenterMove; //�N���X

    [SerializeField] GameObject _gameManager; //�Q�[���}�l�[�W���[
    private RunnyNoseManager _runnyNoseManager; //�N���X

    // Start is called before the first frame update
    void Start()
    {
        // SpriteRenderer�R���|�[�l���g���擾
        _currentSprite = GetComponent<SpriteRenderer>();

        _gravityCenterMove = _runnyNoseGravityCenter.GetComponent<GravityCenterMove>(); //�d�S�̃N���X���Q��

        _runnyNoseManager = _gameManager.GetComponent<RunnyNoseManager>(); //�Q�[���}�l�[�W���[�̃N���X���Q��
    }

    // Update is called once per frame
    void Update()
    {
        //�X�y�[�X�������ꂽ��
        if (_gravityCenterMove.GetSniffling() == true)
        {
            if (!_runnyNoseManager.GetWinning() && !_runnyNoseManager.GetLosing())
            {
                //�@������ł����̃X�v���C�g�ɐ؂�ւ���
                StartCoroutine("FaceSwitching");
            }
        }

        if(_runnyNoseManager.GetWinning() == true)
        {
            _currentSprite.sprite = _winFace;
        }
        else if (_runnyNoseManager.GetLosing() == true)
        {
            _currentSprite.sprite = _loseFace;
        }

        //transform.DOShakePosition(1f, 3f, 1, 1, false, false);
    }

    /// <summary>
    /// �X�v���C�g�؂�ւ�
    /// </summary>
    /// <returns></returns>
    IEnumerator FaceSwitching()
    {
        //�@������ł����̃X�v���C�g�ɐ؂�ւ���
        _currentSprite.sprite = _blowFace;

        //0.2�b��~
        yield return new WaitForSeconds(0.2f);

        //���̃X�v���C�g�ɖ߂�
        _currentSprite.sprite = _nomalFace;

        //�@������ł��Ȃ���Ԃɖ߂�
        _gravityCenterMove.SetSniffling(false);
    }
}
