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

    private float _switchingSpan = 3.0f; //�U������X�p���A����3.0�ŌŒ�
    private float _currentTime = 0f; //�^�C�}�[

    private int _status = 1; //0=�����������Ă�A1=�������Ă�A2=�т������

    private bool _isLooking; //�����������Ă��邩�ǂ���
    public bool GetLooking()
    {
        return _isLooking;
    }
    private void SetLooking(bool value)
    {
        _isLooking = value;
    }

    void Start()
    {
        //SpriteRenderer�R���|�[�l���g���擾
        _currentImage = GetComponent<SpriteRenderer>();

        //�����������Ă��Ȃ�
        SetLooking(false);
    }

    void Update()
    {
        //���������Ă��鎞
        if(_status == 1)
        {
            //�^�C�}�[�N��
            _currentTime += Time.deltaTime;
        }      

        //_switchingSpan�𖞂�������@���@���������Ă��鎞
        if (_currentTime > _switchingSpan && _status == 1)
        {
            //�O����������
            StartCoroutine("FaceForward");

            //�^�C�}�[���Z�b�g
            _currentTime = 0f;
        }
    }

    /// <summary>
    /// �O������
    /// </summary>
    /// <returns></returns>
    IEnumerator FaceForward()
    {
        //�摜�؂�ւ�
        _currentImage.sprite = _nanameSprite;

        //
        yield return new WaitForSeconds(1f);

        //�摜�؂�ւ�
        _currentImage.sprite = _syoumenSprite;

        //�O�������Ă�����
        _status = 0;

        //�����������Ă���
        SetLooking(true);

        //������������
        StartCoroutine("BackToBackward");
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <returns></returns>
    IEnumerator BackToBackward()
    {
        // �b��~
        yield return new WaitForSeconds(0.5f);

        //�摜�؂�ւ�
        _currentImage.sprite = _naname2Sprite;

        //�����������Ă��Ȃ�
        SetLooking(false);

        yield return new WaitForSeconds(0.2f);

        //�摜�؂�ւ�
        _currentImage.sprite = _uragawaSprite;

        //���������Ă�����
        _status = 1;
    }
}
