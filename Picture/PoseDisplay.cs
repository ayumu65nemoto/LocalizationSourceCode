using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PoseDisplay : MonoBehaviour
{
    public Image photoDisplay;  // UI Image to display the selected pose
    public GameObject photoFrame;  // UI GameObject to display the photo frame
    public float displayDelay = 7.0f;  // Delay before displaying the pose
    public AudioClip shutterSound;  // Shutter sound to play when displaying the pose
    public float moveDuration = 1.0f;  // Duration for moving to the center

    private RectTransform photoFrameRect;  // RectTransform of the photo frame
    private Vector3 originalPosition;  // Original position of the photo frame

    private void Start()
    {
        // �ʐ^�g���\���ɂ���
        photoFrame.SetActive(false);

        // RectTransform���擾
        photoFrameRect = photoFrame.GetComponent<RectTransform>();
        originalPosition = photoFrameRect.localPosition;

        // �|�[�Y�̕\�����J�n
        StartCoroutine(DisplayPose());
    }

    private IEnumerator DisplayPose()
    {
        yield return new WaitForSeconds(displayDelay);

        // ���ɐݒ肳��Ă���|�[�Y��photoDisplay�ɓK�p
        // ��FphotoDisplay.sprite = characterPose;

        // �V���b�^�[�����Đ�
        if (shutterSound != null)
        {
            AudioSource.PlayClipAtPoint(shutterSound, Camera.main.transform.position);
        }

        // �ʐ^�g��\��
        photoFrame.SetActive(true);

        // �����Ɉړ�����A�j���[�V�������J�n
        StartCoroutine(MoveToCenter());
    }

    private IEnumerator MoveToCenter()
    {
        Vector3 targetPosition = Vector3.zero;  // �����̍��W
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            photoFrameRect.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        photoFrameRect.localPosition = targetPosition;  // �ŏI�ʒu��ݒ�
    }
}
