using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TextScroll : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scrollText;  //スクロール対象

    private float _scrollDuration = 3f; //スクロールにかかる時間
    private Vector2 _startPosition = new Vector2(0, 0); //開始位置
    private Vector2 _endPosition = new Vector2(-200, 0);    //終了位置

    public void Initialize()
    {
        var destinationData = GameManager.Instance.DestinationData;
        _scrollText.text = destinationData.CurrentPositionName + "　→　" + destinationData.NextDestinationName;
        ScrollText();
    }

    /// <summary>
    /// テキストをスクロールする
    /// </summary>
    private void ScrollText()
    {
        _scrollText.transform.localPosition = _startPosition;
        _scrollText.transform.DOLocalMove(_endPosition, _scrollDuration)
            .SetEase(Ease.Linear);
    }
}
