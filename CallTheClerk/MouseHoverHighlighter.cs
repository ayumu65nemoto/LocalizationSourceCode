using System;
using System.Linq;
using UniRx;
using UnityEngine;

public class MouseHoverHighlighter
{
    private HighlightableObject _lastHighlightedObject = null;
    private IDisposable _disposable;

    /// <summary>
    /// ハイライト処理
    /// </summary>
    public void SetHighlight()
    {
        // 毎フレームのマウス位置をストリームとして取得
        Observable.EveryUpdate()
            .Select(_ => Camera.main.ScreenToWorldPoint(Input.mousePosition))
            .Select(worldPos => Physics2D.OverlapPointAll(new Vector2(worldPos.x, worldPos.y)))
            .Select(colliders => colliders.Select(collider => collider.GetComponent<HighlightableObject>()).FirstOrDefault(obj => obj != null))
            .DistinctUntilChanged()
            .Subscribe(highlightableObject =>
            {
                // 前回のハイライトを解除
                if (_lastHighlightedObject != null)
                {
                    _lastHighlightedObject.Highlight(false);
                }

                // 新しいオブジェクトをハイライト
                if (highlightableObject != null)
                {
                    highlightableObject.Highlight(true);
                }

                // 現在のオブジェクトを保存
                _lastHighlightedObject = highlightableObject;
            });
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
