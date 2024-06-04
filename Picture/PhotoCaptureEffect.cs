using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCaptureEffect : MonoBehaviour
{
    [SerializeField] private Image _photoDisplay;  //�I�������|�[�Y��\������UI�摜
    [SerializeField] private GameObject _photoFrame;  //�t�H�g�t���[����\������UI GameObject
    [SerializeField] private Image _shutterEffect;  //�V���b�^�[���ʗpUI�摜
    [SerializeField] private float _displayDelay = 7.0f;  //�|�[�Y��\������܂ł̒x������
    [SerializeField] private AudioClip _shutterSound;  //�|�[�Y�\�����ɍĐ�����V���b�^�[��
    [SerializeField] private float _moveDuration = 1.0f;  //�����Ɉړ����鎞��

    private RectTransform _photoFrameRect;  //�t�H�g�t���[����RectTransform
    private Vector3 _originalPosition;  //�t�H�g�t���[���̌��̈ʒu
    private Vector3 _originalScale;  //�t�H�g�t���[���̌��̃X�P�[��

    private void Start()
    {
        // �ʐ^�g�ƃV���b�^�[�G�t�F�N�g���\���ɂ���
        _photoFrame.SetActive(false);
        _shutterEffect.gameObject.SetActive(false);

        // RectTransform���擾
        _photoFrameRect = _photoFrame.GetComponent<RectTransform>();
        _originalPosition = _photoFrameRect.localPosition;
        _originalScale = _photoFrameRect.localScale;

        // �|�[�Y�̕\�����J�n
        StartCoroutine(DisplayPose());
    }

    private IEnumerator DisplayPose()
    {
        yield return new WaitForSeconds(_displayDelay);

        // ���ɐݒ肳��Ă���|�[�Y��photoDisplay�ɓK�p
        // ��FphotoDisplay.sprite = characterPose;

        // �V���b�^�[�����Đ�
        if (_shutterSound != null)
        {
            AudioSource.PlayClipAtPoint(_shutterSound, Camera.main.transform.position);
        }

        // �V���b�^�[�G�t�F�N�g��\��
        StartCoroutine(ShutterAnimation());

        // �ʐ^�g��\��
        _photoFrame.SetActive(true);

        // �����Ɉړ�����A�j���[�V�������J�n
        StartCoroutine(MoveToCenter());
    }

    private IEnumerator ShutterAnimation()
    {
        _shutterEffect.gameObject.SetActive(true);
        Color originalColor = _shutterEffect.color;
        float elapsedTime = 0f;

        while (elapsedTime < 0.5f)
        {
            _shutterEffect.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0, 1, elapsedTime / 0.5f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _shutterEffect.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        yield return new WaitForSeconds(0.1f);

        elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            _shutterEffect.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, elapsedTime / 0.5f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _shutterEffect.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        _shutterEffect.gameObject.SetActive(false);
    }

    private IEnumerator MoveToCenter()
    {
        Vector3 targetPosition = Vector3.zero;  // �����̍��W
        Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);  // �g��k������ꍇ�̖ڕW�X�P�[��
        float elapsedTime = 0f;

        while (elapsedTime < _moveDuration)
        {
            _photoFrameRect.localPosition = Vector3.Lerp(_originalPosition, targetPosition, elapsedTime / _moveDuration);
            _photoFrameRect.localScale = Vector3.Lerp(_originalScale, targetScale, elapsedTime / _moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _photoFrameRect.localPosition = targetPosition;  // �ŏI�ʒu��ݒ�
        _photoFrameRect.localScale = targetScale;  // �ŏI�X�P�[����ݒ�
    }
}
