using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ResultView : MonoBehaviour
{
    [SerializeField]
    private GameObject _resultUIGroup;  //リザルト画面のUI
    [SerializeField]
    private TMP_Text _resultText;   //リザルト画面のテキスト
    [SerializeField]
    private Image _resultImage; //リザルト画面の画像
    [SerializeField]
    private Button _returnMainButton;   //ワールドマップへ戻るボタン
    [SerializeField]
    private AudioClip _clearBGM;    //クリアBGM
    [SerializeField]
    private AudioClip _failedBGM;   //ゲームオーバーBGM

    private float _waitTime = 1.0f; //リザルトを出す待ち時間

    public IObservable<Unit> OnClickReturnMainButton => _returnMainButton.OnClickAsObservable();

    public void Initialize()
    {
        _resultUIGroup.SetActive(false);
        _resultText.text = "";
    }

    /// <summary>
    /// リザルト表示
    /// </summary>
    /// <param name="success"></param>
    public async Task OpenAsync(bool success, CancellationToken cancellationToken = default)
    {
        //表示を待つ
        await UniTask.Delay(TimeSpan.FromSeconds(_waitTime), cancellationToken: cancellationToken);

        _resultUIGroup.SetActive(true);
        _returnMainButton.transform.DOPunchScale(Vector3.one * 0.1f, 1.5f, 1).SetLoops(-1, LoopType.Restart);

        if (success)
        {
            _resultText.text = "ゲームクリア！";
            SoundManager.Instance.PlayBGM(_clearBGM);
        }
        else
        {
            _resultText.text = "ゲームオーバー";
            SoundManager.Instance.PlayBGM(_failedBGM);
        }
    }

    public void SetResultImage(Sprite sprite)
    {
        _resultImage.sprite = sprite;
    }
}
