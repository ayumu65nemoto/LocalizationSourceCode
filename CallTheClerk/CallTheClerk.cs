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
    private Clerk _clerkPrefab;    //�X����Prefab
    [SerializeField]
    private TMP_Text _startGameTimerText;   //�Q�[���J�n�܂ł̎��Ԃ�\������e�L�X�g
    [SerializeField]
    private TMP_Text _inGameTimerText;  //�Q�[�����̃^�C�}�[��\������e�L�X�g
    [SerializeField]
    private TargetCursor _cursorPrefab;  //�^�[�Q�b�g�J�[�\��

    private void Awake()
    {
        _resultView.Initialize();
        _startGameTimerText.gameObject.SetActive(true);
        _inGameTimerText.gameObject.SetActive(false);
        //�^�C�}�[�ݒ�
        _gameManager = GameManager.Instance;
        _gameManager.GameTimer.TimerMethod(_startTime, Initialize, _startGameTimerText);
    }

    private void Initialize()
    {
        //�e����������
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

        //���U���g��ʕ\�������ݒ�
        clerk.ResultSubject
            .Skip(1)
            .Subscribe(x =>
            {
                _resultView.Open(x);
                //���Ԑ؂�ȂǂŌ��ʂ��o���ۂɁA���߂ăN���b�N�̏����͂��������Ȃ��̂�
                _sendGlance.Dispose();
                //�^�C�}�[�͔j��
                gameTimer.Dispose();
                //�J�[�\�������ɖ߂��Ă���
                Destroy(targetCursor.gameObject);
                _mouseHoverHighlighter.Dispose();
            });
        //���U���g��Ƀ��[���h�}�b�v�ɖ߂鏈���ݒ�
        _gameManager.SetNextScene();
        _resultView.OnClickReturnMainButton.Subscribe(_ => _gameManager.SceneData.BackToWorldMap());
        //�^�C�}�[�ݒ�
        gameTimer.TimerMethod(_gameTime, () => _resultView.Open(false), _inGameTimerText);
    }

    private void OnDestroy()
    {
        _sendGlance.Dispose();
    }
}
