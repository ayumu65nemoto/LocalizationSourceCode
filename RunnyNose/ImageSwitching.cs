using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitching : MonoBehaviour
{
    [SerializeField] Sprite _syoumenSprite; //����
    [SerializeField] Sprite _nanameSprite; //���ʂ������ۂ̍���

    [SerializeField] Sprite _uragawaSprite; //����
    [SerializeField] Sprite _naname2Sprite; //���������ۂ̍���

    [SerializeField] Sprite _bikkuriSprite; //�т�����

    private SpriteRenderer _currentImage;

    private float _switchingSpan = 5.0f; //�U������X�p���A����3.0�ŌŒ�
    private float _currentTime = 0f; //�^�C�}�[

    private bool _isLooking; //�����������Ă��邩�ǂ���
    public bool GetLooking()
    {
        return _isLooking;
    }
    private void SetLooking(bool value)
    {
        _isLooking = value;
    }

    private bool _isStart = false; //��ăX�^�[�g�p�̕ϐ�
    public void SetIsStart(bool value)
    {
        _isStart = value;
    }

    Coroutine _someCoroutine; //�R���[�`����~�p�̔�

    private SoundManager _soundManager;

    [SerializeField]
    private AudioClip _noticeSE;    //�@��������SE

    [SerializeField]
    private AudioClip _angrySE;    //�@��������SE

    void Start()
    {
        //SpriteRenderer�R���|�[�l���g���擾
        _currentImage = GetComponent<SpriteRenderer>();
        //�ŏ��͌�����������
        _currentImage.sprite = _uragawaSprite;

        //�����������Ă��Ȃ�
        SetLooking(false);

        _soundManager = SoundManager.Instance;
    }

    void Update()
    {
        if (_isStart)
        {
            TurnAroundPerTime();
        }
    }

    private void TurnAroundPerTime()
    {
        //���������Ă��鎞
        if (!_isLooking)
        {
            //�^�C�}�[�N��
            _currentTime += Time.deltaTime;

            //_switchingSpan�𖞂�������
            if (_currentTime > _switchingSpan)
            {
                //�O����������
                _someCoroutine = StartCoroutine(FaceForward());

                //�^�C�}�[���Z�b�g
                _currentTime = 0f;
            }
        }
    }

    /// <summary>
    /// �O������
    /// </summary>
    /// <returns></returns>
    IEnumerator FaceForward()
    {
        //�������ɋC�Â�SE
        _soundManager.PlaySE(_noticeSE, false);

        //�摜�؂�ւ�
        _currentImage.sprite = _nanameSprite;

        //
        yield return new WaitForSeconds(0.2f);

        //�摜�؂�ւ�
        _currentImage.sprite = _syoumenSprite;

        //�����������Ă���
        SetLooking(true);

        // �b��~
        yield return new WaitForSeconds(0.5f);

        //�摜�؂�ւ�
        _currentImage.sprite = _naname2Sprite;

        //�����������Ă��Ȃ�
        SetLooking(false);

        yield return new WaitForSeconds(0.2f);

        if (_isStart)
        {
            //�摜�؂�ւ�
            _currentImage.sprite = _uragawaSprite;
        }
        else if (!_isStart)
        {
            _currentImage.sprite = _bikkuriSprite; //�{��̃I���j�X�v���C�g
        }
    }

    /// <summary>
    /// �Q�[�����s���ɌĂ΂��A�ϋq�̓{�艉�o����
    /// </summary>
    public void SetAngryFace()
    {
        _isStart = false; //�U������܂ł̎��Ԍv�����~

        //StopCoroutine(_someCoroutine); //�U��������~

        _soundManager.PlaySE(_angrySE, false); //���X�̓{�艹��

        _currentImage.sprite = _bikkuriSprite; //�{��̃I���j�X�v���C�g
    }
}
