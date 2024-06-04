using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class ShutterManager : MonoBehaviour
{
    private float _spanToReleaseShutter = 0.5f; //シャッターを切るスパン
    private ScreenShot _screenShot; //スクリーンショットのクラス

    [SerializeField] 
    private GameObject _flashObject; //フラッシュ演出のためのでかい白い四角形

    [SerializeField] 
    private TextMeshProUGUI _timerText; //シャッターが切られるまでの時間表示テキスト

    private float _time; //シャッター間隔に使うタイム
    private float _shutterSpan = 5.0f; //3秒でシャッターが切られる

    private bool _isTimeUp = false; //シャッターが切られたらtrueになる 

    private int _NumberOfShots = 1; //撮影する回数
    private int _NumberOfShotsTaken = 0; //現在の撮影回数

    [SerializeField]
    private GameObject _characterSelector; //クリア判定クラスを持つオブジェクト
    private Judgment _judgment; //クリア判定クラス

    private bool _shutterJudgment;

    private bool _isStart = false;
    public void SetIsStart(bool value)
    {
        _isStart = value;
    }

    [SerializeField]
    private ResultView _resultView;

    private SoundManager _soundManager; //サウンドマネージャー

    [SerializeField]
    private AudioClip _shutterSE; //シャッター音SE

    [SerializeField]
    private AudioClip _hiCheeseSE; //シャッター音SE

    // Start is called before the first frame update
    void Start()
    {
        //_screenShot = transform.GetComponent<ScreenShot>(); //スクリーンショットのクラス

        //_judgment = _characterSelector.GetComponent<Judgment>();

        //_time = _shutterSpan; //タイマーリセット

        _resultView.Initialize(); //リザルト画面の初期化
        //リザルト後にワールドマップに戻る処理設定
        GameManager.Instance.SetNextScene();
        _resultView.OnClickReturnMainButton.Subscribe(_ => GameManager.Instance.SceneData.BackToWorldMap());

        _soundManager = SoundManager.Instance;
    }

    public void StartProcess()
    {
        _screenShot = transform.GetComponent<ScreenShot>(); //スクリーンショットのクラス

        _judgment = _characterSelector.GetComponent<Judgment>();

        _time = _shutterSpan; //タイマーリセット
    }

    // Update is called once per frame
    void Update()
    {
        if(_NumberOfShots > _NumberOfShotsTaken && _isStart)
        {
            if (_isTimeUp)
            {
                StartCoroutine(ReleaseShutter());
            }

            if (!_isTimeUp)
            {
                ShutterTimer();
            }
        }
    }

    /// <summary>
    /// フラッシュ演出と、保存された写真の表示
    /// </summary>
    /// <returns></returns>
    IEnumerator ReleaseShutter()
    {
        _isTimeUp = false;

        _screenShot.SaveScreenshot();

        //はいチーズSE
        _soundManager.PlaySE(_hiCheeseSE, false);

        yield return new WaitForSeconds(0.1f);

        //シャッターSE
        _soundManager.PlaySE(_shutterSE, false);

        _flashObject.SetActive(true);

        _shutterJudgment = _judgment.GetIsCrear(); //シャッターを切る瞬間の正誤判定を撮る

        yield return new WaitForSeconds(0.2f); //白いスプライトを一瞬表示してフラッシュ風

        _flashObject.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        _screenShot.ShowSSImage(); //保存した写真を表示する関数を呼び出す

        yield return new WaitForSeconds(1.0f);

        _judgment.JudgmentStamp(_shutterJudgment);

        yield return new WaitForSeconds(1.0f);

        _resultView.OpenAsync(_shutterJudgment);

        _NumberOfShotsTaken++;
    }

    /// <summary>
    /// シャッターが切られるまでの残り時間を表示するタイマー
    /// </summary>
    private void ShutterTimer()
    {
        //_time = 3.0f
        if (0 <= _time)
        {
            _time -= Time.deltaTime;
            _timerText.text = _time.ToString("N0");
        }
        else if (0 >= _time)
        {
            _isTimeUp = true;

            _time = _shutterSpan; //タイマーリセット
        }
    }
}
