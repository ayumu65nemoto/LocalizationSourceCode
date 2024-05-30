using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlaneMove : MonoBehaviour
{
    /// <summary>
    /// 飛行機の移動処理
    /// </summary>
    /// <param name="start">開始位置</param>
    /// <param name="end">到着位置</param>
    /// <param name="duration">時間</param>
    /// <returns></returns>
    public async UniTask MapMove(Vector2 start, Vector2 end, float duration)
    {
        // startとendの中間点を計算し、その点を少し上にオフセットする
        Vector2 half = (start + end) / 2 + Vector2.up * 2.0f; // 上に2単位オフセット

        float startTime = Time.timeSinceLevelLoad;
        float rate = 0f;

        while (rate < 1.0f)
        {
            float diff = Time.timeSinceLevelLoad - startTime;
            rate = diff / duration;
            transform.position = CalcLerpPoint(start, half, end, rate);

            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        // 最後にend位置を確定させる
        transform.position = end;
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
