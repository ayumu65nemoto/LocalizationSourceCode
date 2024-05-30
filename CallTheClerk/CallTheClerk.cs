using UniRx;
using UnityEngine;
using TMPro;

public class CallTheClerk : MonoBehaviour
{
    private GameManager _gameManager;
    private SendGlance _sendGlance;
    private MouseHoverHighlighter _mouseHoverHighlighter;
    private float _gameTime = 10f;
    private float _startTime = 3f;

    [SerializeField]
    private ResultView _resultView;
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

    private void Awake()
    {
        _resultView.Initialize();
        _startGameTimerText.gameObject.SetActive(true);
        _inGameTimerText.gameObject.SetActive(false);
        //タイマー設定
        _gameManager = GameManager.Instance;
        _gameManager.GameTimer.TimerMethod(_startTime, Initialize, _startGameTimerText);
    }

    private void Initialize()
    {
        //各初期化処理
        Clerk clerk = Instantiate(_clerkPrefab);
        clerk.Initialize();
        HighlightableObject highlightableObject = clerk.gameObject.GetComponent<HighlightableObject>();
        highlightableObject.Initialize();
        _mouseHoverHighlighter = new MouseHoverHighlighter();
        _mouseHoverHighlighter.SetHighlight();

        _sendGlance = new SendGlance(_resultView, clerk, _callClerk);
        _sendGlance.Initialize();

        TargetCursor targetCursor = Instantiate(_cursorPrefab);
        targetCursor.SetCursor();

        _startGameTimerText.gameObject.SetActive(false);
        _inGameTimerText.gameObject.SetActive(true);

        GameTimer gameTimer = GameManager.Instance.GameTimer;

        //リザルト画面表示処理設定
        clerk.ResultSubject
            .Skip(1)
            .Subscribe(x =>
            {
                _resultView.Open(x);
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
        _resultView.OnClickReturnMainButton.Subscribe(_ => _gameManager.SceneData.BackToWorldMap());
        //タイマー設定
        gameTimer.TimerMethod(_gameTime, () => _resultView.Open(false), _inGameTimerText);
    }

    private void OnDestroy()
    {
        _sendGlance.Dispose();
    }
}
