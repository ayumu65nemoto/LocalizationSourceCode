using UnityEngine;

public class HighlightableObject : MonoBehaviour
{
    private Color _originalColor;  //���̐F
    public Color _highlightColor;  //�n�C���C�g���̐F

    /// <summary>
    /// ����������
    /// </summary>
    public void Initialize()
    {
        _originalColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);
        _highlightColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        GetComponent<SpriteRenderer>().color = _originalColor;
    }

    /// <summary>
    /// �n�C���C�g���̏���
    /// </summary>
    /// <param name="highlight">�n�C���C�g���ꂽ���ǂ���</param>
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
