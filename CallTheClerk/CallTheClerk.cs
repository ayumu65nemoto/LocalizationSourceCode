using UniRx;
using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class CallTheClerk : MonoBehaviour
{
    private GameManager _gameManager;
    private SoundManager _soundManager;
    private SendGlance _sendGlance;
    private MouseHoverHighlighter _mouseHoverHighlighter;
    private float _gameTime = 10f;  //ゲームの制限時間
    private float _startTime = 0f;  //ゲーム開始までの時間
    private IDisposable _disposable;
    private List<GameObject> _disposePool = new List<GameObject>(); //オブジェクト破棄用のプール

    [SerializeField]
    private ResultView _resultView; //結果画面
    [SerializeField]
    private CallClerk _callClerk;
    [SerializeField]
    private Clerk _clerkPrefab;    //店員のPrefab
    [SerializeField]
    private TMP_Text _startGameTimerText;   //ゲーム開始までの時間を表示するテキスト
    [SerializeField]
    private TMP_Text _inGameTimerText;  //ゲーム中のタイマーを表示するテキスト
    [SerializeField]
    private TargetCursor _cursorPrefab;  //ターゲットカーソル
    [SerializeField]
    private StartUIManager _introGroup; //ゲームスタート時の説明パネル
    [SerializeField]
    private AudioClip _bgm; //BGM
    [SerializeField]
    private AudioClip _buttonSE;    //ボタンのSE

    private void Awake()
    {
        _resultView.Initialize();
        //_startGameTimerText.gameObject.SetActive(true);
        _inGameTimerText.gameObject.SetActive(false);
        _introGroup.gameObject.SetActive(true);

        //BGM再生
        _soundManager = SoundManager.Instance;
        SoundManager.Instance.PlayBGM(_bgm);

        _disposable = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0)) // 左クリックを監視
            .Subscribe(_ =>
            {
                if (!_introGroup.IsFinishIntro) return;

                //説明パネル削除
                _introGroup.gameObject.SetActive(false);
                //タイマー設定
                _gameManager = GameManager.Instance;
                _gameManager.GameTimer.TimerMethod(_startTime, Initialize, _startGameTimerText);

                _disposable.Dispose();
            });
    }

    private void Initialize()
    {
        //各初期化処理
        Clerk clerk = Instantiate(_clerkPrefab);
        _disposePool.Add(clerk.gameObject);
        clerk.Initialize();
        HighlightableObject highlightableObject = clerk.gameObject.GetComponent<HighlightableObject>();
        highlightableObject.Initialize();
        _mouseHoverHighlighter = new MouseHoverHighlighter();
        _mouseHoverHighlighter.SetHighlight();

        _sendGlance = new SendGlance(_resultView, clerk, _callClerk);
        _sendGlance.Initialize();

        TargetCursor targetCursor = Instantiate(_cursorPrefab);
        _disposePool.Add(targetCursor.gameObject);
        targetCursor.SetCursor();

        _startGameTimerText.gameObject.SetActive(false);
        _inGameTimerText.gameObject.SetActive(true);

        GameTimer gameTimer = GameManager.Instance.GameTimer;

        //リザルト画面表示処理設定
        clerk.ResultSubject
            .Skip(1)
            .Subscribe(x =>
            {
                _resultView.OpenAsync(x);
                //時間切れなどで結果が出た際に、改めてクリックの処理はさせたくないので
                _sendGlance.Dispose();
                //タイマーは破棄
                gameTimer.Dispose();
                //カーソルを元に戻しておく
                Destroy(targetCursor.gameObject);
                _mouseHoverHighlighter.Dispose();
            });
        //リザルト後にワールドマップに戻る処理設定
        _gameManager.SetNextScene();
        _resultView.OnClickReturnMainButton.Subscribe(_ =>
        {
            _soundManager.PlaySE(_buttonSE, false);
            _gameManager.SceneData.BackToWorldMap();
        });
        //タイマー設定
        gameTimer.TimerMethod(_gameTime, () => _resultView.OpenAsync(false), _inGameTimerText);
    }

    private void OnDestroy()
    {
        foreach(var obj in _disposePool)
        {
            Destroy(obj);
        }

        _disposable?.Dispose();
        _sendGlance?.Dispose();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.StopSE();
    }
}
