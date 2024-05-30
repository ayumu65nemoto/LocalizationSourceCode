using UnityEngine;

public class HighlightableObject : MonoBehaviour
{
    private Color _originalColor;  //元の色
    public Color _highlightColor;  //ハイライト時の色

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        _originalColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);
        _highlightColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        GetComponent<SpriteRenderer>().color = _originalColor;
    }

    /// <summary>
    /// ハイライト時の処理
    /// </summary>
    /// <param name="highlight">ハイライトされたかどうか</param>
    public void Highlight(bool highlight)
    {
        if (highlight)
        {
            GetComponent<SpriteRenderer>().color = _highlightColor;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = _originalColor;
        }
    }
}
