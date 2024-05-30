using System;
using UniRx;
using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    private IDisposable _disposable;

    /// <summary>
    /// カーソル画像を設定する
    /// </summary>
    /// <param name="cursor">カーソル画像</param>
    public void SetCursor()
    {
        // マウスの位置をストリームとして取得
        _disposable = Observable.EveryUpdate()
            .Select(_ => Input.mousePosition)
            .Subscribe(mousePosition =>
            {
                // マウスのスクリーン座標をワールド座標に変換
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                worldPosition.z = 0; // Z軸の位置を0に固定
                transform.position = worldPosition;
            })
            .AddTo(this);
    }

    /// <summary>
    /// カーソルを元に戻す
    /// </summary>
    private void OnDestroy()
    {
        _disposable.Dispose();
    }
}
