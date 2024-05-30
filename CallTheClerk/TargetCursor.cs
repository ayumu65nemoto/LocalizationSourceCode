using System;
using UniRx;
using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    private IDisposable _disposable;

    /// <summary>
    /// �J�[�\���摜��ݒ肷��
    /// </summary>
    /// <param name="cursor">�J�[�\���摜</param>
    public void SetCursor()
    {
        // �}�E�X�̈ʒu���X�g���[���Ƃ��Ď擾
        _disposable = Observable.EveryUpdate()
            .Select(_ => Input.mousePosition)
            .Subscribe(mousePosition =>
            {
                // �}�E�X�̃X�N���[�����W�����[���h���W�ɕϊ�
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                worldPosition.z = 0; // Z���̈ʒu��0�ɌŒ�
                transform.position = worldPosition;
            })
            .AddTo(this);
    }

    /// <summary>
    /// �J�[�\�������ɖ߂�
    /// </summary>
    private void OnDestroy()
    {
        _disposable.Dispose();
    }
}
