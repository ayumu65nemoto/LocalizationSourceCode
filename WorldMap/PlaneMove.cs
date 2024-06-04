using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlaneMove : MonoBehaviour
{
    [SerializeField]
    private AudioClip _se;  //SE

    [SerializeField]
    private Vector2 _startScale;  //�J�n���̑傫��
    private float _maxScaleRate = 1.5f; //�ő�̑傫���{��

    /// <summary>
    /// ��s�@�̈ړ�����
    /// </summary>
    /// <param name="start">�J�n�ʒu</param>
    /// <param name="end">�����ʒu</param>
    /// <param name="duration">����</param>
    /// <returns></returns>
    public async UniTask MapMove(Vector2 start, Vector2 end, float duration)
    {
        //SE�Đ�
        SoundManager.Instance.PlaySE(_se, false);

        // start��end�̒��ԓ_���v�Z���A���̓_��������ɃI�t�Z�b�g����
        Vector2 half = (start + end) / 2 + Vector2.up * 2.0f; // ���2�P�ʃI�t�Z�b�g

        float startTime = Time.timeSinceLevelLoad;
        float rate = 0f;

        var maxScale = _startScale * _maxScaleRate;

        while (rate < 1.0f)
        {
            float diff = Time.timeSinceLevelLoad - startTime;
            rate = diff / duration;
            transform.position = CalcLerpPoint(start, half, end, rate);

            //�T�C�Y��ς���
            if (rate < 0.5f)
            {
                // �J�n�n�_���璆�ԓ_�܂ő傫������
                transform.localScale = Vector3.Lerp(_startScale, maxScale, rate * 2);
            }
            else
            {
                // ���ԓ_����I���n�_�܂Ō��̑傫���ɖ߂�
                transform.localScale = Vector3.Lerp(maxScale, _startScale, (rate - 0.5f) * 2);
            }

            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        // �Ō��end�ʒu���m�肳����
        transform.position = end;
        //�T�C�Y�����̑傫���ɖ߂�
        transform.localScale = _startScale;
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
