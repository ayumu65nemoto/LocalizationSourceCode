using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ResultView : MonoBehaviour
{
    [SerializeField]
    private GameObject _resultUIGroup;  //���U���g��ʂ�UI
    [SerializeField]
    private TMP_Text _resultText;   //���U���g��ʂ̃e�L�X�g
    [SerializeField]
    private Image _resultImage; //���U���g��ʂ̉摜
    [SerializeField]
    private Button _returnMainButton;   //���[���h�}�b�v�֖߂�{�^��
    [SerializeField]
    private AudioClip _clearBGM;    //�N���ABGM
    [SerializeField]
    private AudioClip _failedBGM;   //�Q�[���I�[�o�[BGM

    private float _waitTime = 1.0f; //���U���g���o���҂�����

    public IObservable<Unit> OnClickReturnMainButton => _returnMainButton.OnClickAsObservable();

    public void Initialize()
    {
        _resultUIGroup.SetActive(false);
        _resultText.text = "";
    }

    /// <summary>
    /// ���U���g�\��
    /// </summary>
    /// <param name="success"></param>
    public async Task OpenAsync(bool success, CancellationToken cancellationToken = default)
    {
        //�\����҂�
        await UniTask.Delay(TimeSpan.FromSeconds(_waitTime), cancellationToken: cancellationToken);

        _resultUIGroup.SetActive(true);
        _returnMainButton.transform.DOPunchScale(Vector3.one * 0.1f, 1.5f, 1).SetLoops(-1, LoopType.Restart);

        if (success)
        {
            _resultText.text = "�Q�[���N���A�I";
            SoundManager.Instance.PlayBGM(_clearBGM);
        }
        else
        {
            _resultText.text = "�Q�[���I�[�o�[";
            SoundManager.Instance.PlayBGM(_failedBGM);
        }
    }

    public void SetResultImage(Sprite sprite)
    {
        _resultImage.sprite = sprite;
    }
}
