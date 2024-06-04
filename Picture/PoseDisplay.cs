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
        // 写真枠を非表示にする
        photoFrame.SetActive(false);

        // RectTransformを取得
        photoFrameRect = photoFrame.GetComponent<RectTransform>();
        originalPosition = photoFrameRect.localPosition;

        // ポーズの表示を開始
        StartCoroutine(DisplayPose());
    }

    private IEnumerator DisplayPose()
    {
        yield return new WaitForSeconds(displayDelay);

        // 既に設定されているポーズをphotoDisplayに適用
        // 例：photoDisplay.sprite = characterPose;

        // シャッター音を再生
        if (shutterSound != null)
        {
            AudioSource.PlayClipAtPoint(shutterSound, Camera.main.transform.position);
        }

        // 写真枠を表示
        photoFrame.SetActive(true);

        // 中央に移動するアニメーションを開始
        StartCoroutine(MoveToCenter());
    }

    private IEnumerator MoveToCenter()
    {
        Vector3 targetPosition = Vector3.zero;  // 中央の座標
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            photoFrameRect.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        photoFrameRect.localPosition = targetPosition;  // 最終位置を設定
    }
}
