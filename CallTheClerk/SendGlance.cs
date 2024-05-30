using UnityEngine;
using UniRx;
using System;

public class SendGlance
{
    private ResultView _resultView;
    private Clerk _clerk;
    private CallClerk _callClerk;
    private IDisposable _disposable;    //リソース管理用

    public SendGlance(ResultView resultManager, Clerk clerk, CallClerk callClerk)
    {
        _resultView = resultManager;
        _clerk = clerk;
        _callClerk = callClerk;
    }

    /// <summary>
    /// 初期化メソッド
    /// </summary>
    public void Initialize()
    {
        // クリックイベントを監視
        _disposable = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0)) // 左クリックを監視
            .Subscribe(_ => OnMouseDown());
    }

    /// <summary>
    /// マウスを押した際の処理
    /// </summary>
    private async void OnMouseDown()
    {
        //すでにリザルトが出ていれば処理をしない
        if (_resultView.gameObject.activeSelf) return;

        //クリック位置を取得
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

        //クリック位置にあるコライダーを取得
        RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);

        //これ以上クリックさせない
        _disposable.Dispose();

        //タイマーを止める
        GameManager.Instance.GameTimer.Dispose();

        //クリックしたコライダーが特定のSpriteのコライダーかどうかを確認
        if (hit.collider != null)
        {
            var clerk = hit.collider.GetComponent<Clerk>();
            //正面を向いていればクリア
            if (clerk.IsFront)
            {
                await _callClerk.RiseArm();
                clerk.Reaction(true);
            }
            //正面を向いていない状態で目元をクリックしたらゲームオーバー
            else
            {
                await _callClerk.Speak();
                clerk.Reaction(false);
            }
        }
        //目元以外をクリックしたらゲームオーバー
        else
        {
            await _callClerk.Speak();
            _clerk.Reaction(false);
        }
    }

    /// <summary>
    /// リソース解放
    /// </summary>
    public void Dispose()
    {
        _disposable.Dispose();
    }
}
