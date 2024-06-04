using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TextScroll : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scrollText;  //�X�N���[���Ώ�

    private float _scrollDuration = 3f; //�X�N���[���ɂ����鎞��
    private Vector2 _startPosition = new Vector2(0, 0); //�J�n�ʒu
    private Vector2 _endPosition = new Vector2(-200, 0);    //�I���ʒu

    public void Initialize()
    {
        var destinationData = GameManager.Instance.DestinationData;
        _scrollText.text = destinationData.CurrentPositionName + "�@���@" + destinationData.NextDestinationName;
        ScrollText();
    }

    /// <summary>
    /// �e�L�X�g���X�N���[������
    /// </summary>
    private void ScrollText()
    {
        _scrollText.transform.localPosition = _startPosition;
        _scrollText.transform.DOLocalMove(_endPosition, _scrollDuration)
            .SetEase(Ease.Linear);
    }
}
