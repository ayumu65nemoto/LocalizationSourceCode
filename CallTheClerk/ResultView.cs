using System;
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
    private Button _returnMainButton;

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
    public void Open(bool success)
    {
        _resultUIGroup.SetActive(true);

        if (success)
        {
            _resultText.text = "�Q�[���N���A�I";
        }
        else
        {
            _resultText.text = "�Q�[���I�[�o�[";
        }
    }
}
