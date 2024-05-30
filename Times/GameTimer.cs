using System;
using UniRx;
using TMPro;

public class GameTimer
{
    private IDisposable _finishDisposable;
    private IDisposable _timeDisposable;

    /// <summary>
    /// インゲームで使うタイマー機能
    /// </summary>
    /// <param name="time">タイマー設定時間</param>
    /// <param name="remainingText">タイマー用のテキスト</param>
    public void TimerMethod(float time, Action action, TMP_Text remainingText)
    {
        //初期化
        Dispose();

        //タイマーテキストをセット
        UpdateTimerUI(time, remainingText);

        // タイマーを作成し、一定間隔で残り時間を更新する
        _finishDisposable = Observable.Timer(TimeSpan.FromSeconds(time))
            .Subscribe(_ => TimerFinished(action, remainingText));

        // 毎フレーム、残り時間を更新する
        _timeDisposable = Observable.Interval(TimeSpan.FromMilliseconds(100))
            .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(time)))
            .Subscribe(x =>
            {
                float remainingTime = time - (x + 1) * 0.1f;
                UpdateTimerUI(remainingTime, remainingText);
            });
    }

    /// <summary>
    /// 時間終了時の処理
    /// </summary>
    private void TimerFinished(Action action, TMP_Text remainingText)
    {
        //基本的にはゲームオーバー処理
        action.Invoke();
        //タイマー表示も０にする
        UpdateTimerUI(0, remainingText);
    }

    /// <summary>
    /// テキスト更新処理
    /// </summary>
    /// <param name="remainingTime">タイマーの残り時間</param>
    /// <param name="remainingText">タイマー用のテキスト</param>
    private void UpdateTimerUI(float remainingTime, TMP_Text remainingText)
    {
        if (remainingText != null)
        {
            remainingText.text = remainingTime.ToString("F1");
        }
    }

    public void Dispose()
    {
        _finishDisposable?.Dispose();
        _timeDisposable?.Dispose();
    }
}
