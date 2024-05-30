using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlaneMove : MonoBehaviour
{
    /// <summary>
    /// ��s�@�̈ړ�����
    /// </summary>
    /// <param name="start">�J�n�ʒu</param>
    /// <param name="end">�����ʒu</param>
    /// <param name="duration">����</param>
    /// <returns></returns>
    public async UniTask MapMove(Vector2 start, Vector2 end, float duration)
    {
        // start��end�̒��ԓ_���v�Z���A���̓_��������ɃI�t�Z�b�g����
        Vector2 half = (start + end) / 2 + Vector2.up * 2.0f; // ���2�P�ʃI�t�Z�b�g

        float startTime = Time.timeSinceLevelLoad;
        float rate = 0f;

        while (rate < 1.0f)
        {
            float diff = Time.timeSinceLevelLoad - startTime;
            rate = diff / duration;
            transform.position = CalcLerpPoint(start, half, end, rate);

            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        // �Ō��end�ʒu���m�肳����
        transform.position = end;
    }

    /// <summary>
    /// �ړ����̓������v�Z
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
