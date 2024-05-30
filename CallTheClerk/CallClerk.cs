using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class CallClerk : MonoBehaviour
{
    private Vector2 _rotateValue = Vector2.zero;
    private Vector2 _imageSize = new Vector2(0.3f, 0.3f);

    [SerializeField]
    private GameObject _riseArm;    //�r���グ��摜
    [SerializeField]
    private GameObject _speak;  //�����o��

    /// <summary>
    /// ����������
    /// </summary>
    public void Initialize()
    {
        _riseArm.SetActive(false);
        _speak.SetActive(false);
    }

    /// <summary>
    /// �r���グ�鏈��
    /// </summary>
    public async UniTask RiseArm()
    {
        _riseArm.SetActive(true);
        Tween tween =  _riseArm.transform.DORotate(_rotateValue, 1.0f);
        // �A�j���[�V�����̊�����ҋ@
        await tween.AsyncWaitForCompletion();
    }

    /// <summary>
    /// �����グ�鏈��
    /// </summary>
    public async UniTask Speak()
    {
        _speak.SetActive(true);
        Tween tween = _speak.transform.DOScale(_imageSize, 1.0f);
        // �A�j���[�V�����̊�����ҋ@
        await tween.AsyncWaitForCompletion();
    }
}
