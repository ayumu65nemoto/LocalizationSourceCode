using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCaptureEffect : MonoBehaviour
{
    [SerializeField] private Image _photoDisplay;  //選択したポーズを表示するUI画像
    [SerializeField] private GameObject _photoFrame;  //フォトフレームを表示するUI GameObject
    [SerializeField] private Image _shutterEffect;  //シャッター効果用UI画像
    [SerializeField] private float _displayDelay = 7.0f;  //ポーズを表示するまでの遅延時間
    [SerializeField] private AudioClip _shutterSound;  //ポーズ表示時に再生するシャッター音
    [SerializeField] private float _moveDuration = 1.0f;  //中央に移動する時間

    private RectTransform _photoFrameRect;  //フォトフレームのRectTransform
    private Vector3 _originalPosition;  //フォトフレームの元の位置
    private Vector3 _originalScale;  //フォトフレームの元のスケール

    private void Start()
    {
        // 写真枠とシャッターエフェクトを非表示にする
        _photoFrame.SetActive(false);
        _shutterEffect.gameObject.SetActive(false);

        // RectTransformを取得
        _photoFrameRect = _photoFrame.GetComponent<RectTransform>();
        _originalPosition = _photoFrameRect.localPosition;
        _originalScale = _photoFrameRect.localScale;

        // ポーズの表示を開始
        StartCoroutine(DisplayPose());
    }

    private IEnumerator DisplayPose()
    {
        yield return new WaitForSeconds(_displayDelay);

        // 既に設定されているポーズをphotoDisplayに適用
        // 例：photoDisplay.sprite = characterPose;

        // シャッター音を再生
        if (_shutterSound != null)
        {
            AudioSource.PlayClipAtPoint(_shutterSound, Camera.main.transform.position);
        }

        // シャッターエフェクトを表示
        StartCoroutine(ShutterAnimation());

        // 写真枠を表示
        _photoFrame.SetActive(true);

        // 中央に移動するアニメーションを開始
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
        Vector3 targetPosition = Vector3.zero;  // 中央の座標
        Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);  // 拡大縮小する場合の目標スケール
        float elapsedTime = 0f;

        while (elapsedTime < _moveDuration)
        {
            _photoFrameRect.localPosition = Vector3.Lerp(_originalPosition, targetPosition, elapsedTime / _moveDuration);
            _photoFrameRect.localScale = Vector3.Lerp(_originalScale, targetScale, elapsedTime / _moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _photoFrameRect.localPosition = targetPosition;  // 最終位置を設定
        _photoFrameRect.localScale = targetScale;  // 最終スケールを設定
    }
}
