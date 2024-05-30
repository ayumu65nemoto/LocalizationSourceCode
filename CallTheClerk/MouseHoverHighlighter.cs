using System;
using System.Linq;
using UniRx;
using UnityEngine;

public class MouseHoverHighlighter
{
    private HighlightableObject _lastHighlightedObject = null;
    private IDisposable _disposable;

    /// <summary>
    /// �n�C���C�g����
    /// </summary>
    public void SetHighlight()
    {
        // ���t���[���̃}�E�X�ʒu���X�g���[���Ƃ��Ď擾
        Observable.EveryUpdate()
            .Select(_ => Camera.main.ScreenToWorldPoint(Input.mousePosition))
            .Select(worldPos => Physics2D.OverlapPointAll(new Vector2(worldPos.x, worldPos.y)))
            .Select(colliders => colliders.Select(collider => collider.GetComponent<HighlightableObject>()).FirstOrDefault(obj => obj != null))
            .DistinctUntilChanged()
            .Subscribe(highlightableObject =>
            {
                // �O��̃n�C���C�g������
                if (_lastHighlightedObject != null)
                {
                    _lastHighlightedObject.Highlight(false);
                }

                // �V�����I�u�W�F�N�g���n�C���C�g
                if (highlightableObject != null)
                {
                    highlightableObject.Highlight(true);
                }

                // ���݂̃I�u�W�F�N�g��ۑ�
                _lastHighlightedObject = highlightableObject;
            });
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
