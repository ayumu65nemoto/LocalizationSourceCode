using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlaneMove : MonoBehaviour
{
    [SerializeField]
    private AudioClip _se;  //SE

    [SerializeField]
    private Vector2 _startScale;  //開始時の大きさ
    private float _maxScaleRate = 1.5f; //最大の大きさ倍率

    /// <summary>
    /// 飛行機の移動処理
    /// </summary>
    /// <param name="start">開始位置</param>
    /// <param name="end">到着位置</param>
    /// <param name="duration">時間</param>
    /// <returns></returns>
    public async UniTask MapMove(Vector2 start, Vector2 end, float duration)
    {
        //SE再生
        SoundManager.Instance.PlaySE(_se, false);

        // startとendの中間点を計算し、その点を少し上にオフセットする
        Vector2 half = (start + end) / 2 + Vector2.up * 2.0f; // 上に2単位オフセット

        float startTime = Time.timeSinceLevelLoad;
        float rate = 0f;

        var maxScale = _startScale * _maxScaleRate;

        while (rate < 1.0f)
        {
            float diff = Time.timeSinceLevelLoad - startTime;
            rate = diff / duration;
            transform.position = CalcLerpPoint(start, half, end, rate);

            //サイズを変える
            if (rate < 0.5f)
            {
                // 開始地点から中間点まで大きくする
                transform.localScale = Vector3.Lerp(_startScale, maxScale, rate * 2);
            }
            else
            {
                // 中間点から終了地点まで元の大きさに戻す
                transform.localScale = Vector3.Lerp(maxScale, _startScale, (rate - 0.5f) * 2);
            }

            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        // 最後にend位置を確定させる
        transform.position = end;
        //サイズも元の大きさに戻す
        transform.localScale = _startScale;
    }

    /// <summary>
    /// 移動時の動き方計算
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private Vector2 CalcLerpPoint(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        var a = Vector2.Lerp(p0, p1, t);
        var b = Vector2.Lerp(p1, p2, t);
        return Vector2.Lerp(a, b, t);
    }
}
