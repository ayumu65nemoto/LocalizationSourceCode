using UnityEngine;
using UniRx;
using System;

public class SendGlance
{
    private ResultView _resultView;
    private Clerk _clerk;
    private CallClerk _callClerk;
    private IDisposable _disposable;    //���\�[�X�Ǘ��p

    public SendGlance(ResultView resultManager, Clerk clerk, CallClerk callClerk)
    {
        _resultView = resultManager;
        _clerk = clerk;
        _callClerk = callClerk;
    }

    /// <summary>
    /// ���������\�b�h
    /// </summary>
    public void Initialize()
    {
        // �N���b�N�C�x���g���Ď�
        _disposable = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0)) // ���N���b�N���Ď�
            .Subscribe(_ => OnMouseDown());
    }

    /// <summary>
    /// �}�E�X���������ۂ̏���
    /// </summary>
    private async void OnMouseDown()
    {
        //���łɃ��U���g���o�Ă���Ώ��������Ȃ�
        if (_resultView.gameObject.activeSelf) return;

        //�N���b�N�ʒu���擾
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

        //�N���b�N�ʒu�ɂ���R���C�_�[���擾
        RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);

        //����ȏ�N���b�N�����Ȃ�
        _disposable.Dispose();

        //�^�C�}�[���~�߂�
        GameManager.Instance.GameTimer.Dispose();

        //�N���b�N�����R���C�_�[�������Sprite�̃R���C�_�[���ǂ������m�F
        if (hit.collider != null)
        {
            var clerk = hit.collider.GetComponent<Clerk>();
            //���ʂ������Ă���΃N���A
            if (clerk.IsFront)
            {
                await _callClerk.RiseArm();
                clerk.Reaction(true);
            }
            //���ʂ������Ă��Ȃ���ԂŖڌ����N���b�N������Q�[���I�[�o�[
            else
            {
                await _callClerk.Speak();
                clerk.Reaction(false);
            }
        }
        //�ڌ��ȊO���N���b�N������Q�[���I�[�o�[
        else
        {
            await _callClerk.Speak();
            _clerk.Reaction(false);
        }
    }

    /// <summary>
    /// ���\�[�X���
    /// </summary>
    public void Dispose()
    {
        _disposable.Dispose();
    }
}
