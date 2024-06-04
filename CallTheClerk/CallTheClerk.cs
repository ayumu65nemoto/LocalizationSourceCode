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
    private float _gameTime = 10f;  //�Q�[���̐�������
    private float _startTime = 0f;  //�Q�[���J�n�܂ł̎���
    private IDisposable _disposable;
    private List<GameObject> _disposePool = new List<GameObject>(); //�I�u�W�F�N�g�j���p�̃v�[��

    [SerializeField]
    private ResultView _resultView; //���ʉ��
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
    [SerializeField]
    private StartUIManager _introGroup; //�Q�[���X�^�[�g���̐����p�l��
    [SerializeField]
    private AudioClip _bgm; //BGM
    [SerializeField]
    private AudioClip _buttonSE;    //�{�^����SE

    private void Awake()
    {
        _resultView.Initialize();
        //_startGameTimerText.gameObject.SetActive(true);
        _inGameTimerText.gameObject.SetActive(false);
        _introGroup.gameObject.SetActive(true);

        //BGM�Đ�
        _soundManager = SoundManager.Instance;
        SoundManager.Instance.PlayBGM(_bgm);

        _disposable = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0)) // ���N���b�N���Ď�
            .Subscribe(_ =>
            {
                if (!_introGroup.IsFinishIntro) return;

                //�����p�l���폜
                _introGroup.gameObject.SetActive(false);
                //�^�C�}�[�ݒ�
                _gameManager = GameManager.Instance;
                _gameManager.GameTimer.TimerMethod(_startTime, Initialize, _startGameTimerText);

                _disposable.Dispose();
            });
    }

    private void Initialize()
    {
        //�e����������
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

        //���U���g��ʕ\�������ݒ�
        clerk.ResultSubject
            .Skip(1)
            .Subscribe(x =>
            {
                _resultView.OpenAsync(x);
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
        _resultView.OnClickReturnMainButton.Subscribe(_ =>
        {
            _soundManager.PlaySE(_buttonSE, false);
            _gameManager.SceneData.BackToWorldMap();
        });
        //�^�C�}�[�ݒ�
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
