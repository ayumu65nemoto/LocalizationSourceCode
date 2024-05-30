using System;
using UniRx;
using TMPro;

public class GameTimer
{
    private IDisposable _finishDisposable;
    private IDisposable _timeDisposable;

    /// <summary>
    /// �C���Q�[���Ŏg���^�C�}�[�@�\
    /// </summary>
    /// <param name="time">�^�C�}�[�ݒ莞��</param>
    /// <param name="remainingText">�^�C�}�[�p�̃e�L�X�g</param>
    public void TimerMethod(float time, Action action, TMP_Text remainingText)
    {
        //������
        Dispose();

        //�^�C�}�[�e�L�X�g���Z�b�g
        UpdateTimerUI(time, remainingText);

        // �^�C�}�[���쐬���A���Ԋu�Ŏc�莞�Ԃ��X�V����
        _finishDisposable = Observable.Timer(TimeSpan.FromSeconds(time))
            .Subscribe(_ => TimerFinished(action, remainingText));

        // ���t���[���A�c�莞�Ԃ��X�V����
        _timeDisposable = Observable.Interval(TimeSpan.FromMilliseconds(100))
            .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(time)))
            .Subscribe(x =>
            {
                float remainingTime = time - (x + 1) * 0.1f;
                UpdateTimerUI(remainingTime, remainingText);
            });
    }

    /// <summary>
    /// ���ԏI�����̏���
    /// </summary>
    private void TimerFinished(Action action, TMP_Text remainingText)
    {
        //��{�I�ɂ̓Q�[���I�[�o�[����
        action.Invoke();
        //�^�C�}�[�\�����O�ɂ���
        UpdateTimerUI(0, remainingText);
    }

    /// <summary>
    /// �e�L�X�g�X�V����
    /// </summary>
    /// <param name="remainingTime">�^�C�}�[�̎c�莞��</param>
    /// <param name="remainingText">�^�C�}�[�p�̃e�L�X�g</param>
    private void UpdateTimerUI(float remainingTime, TMP_Text remainingText)
    {
        if (remainingText != null)
        {
            remainingText.text = remainingTime.ToString("F1");
        }
    }

    public void Dispose()
    {
        _finishDisposable?.Dispose();
        _timeDisposable?.Dispose();
    }
}
