using UnityEngine;
using UniRx;
using System;
using DG.Tweening;

public class Clerk : MonoBehaviour
{
    private Vector2 _pos;   //�X���̈ʒu
    private float _clerkSize = 0.3f;   //�X���̃T�C�Y
    private int _num = 1;   //�X�����ǂ���Ɍ����Ă��邩
    private float _moveSpeed = 4.0f;    //�X���̈ړ����x
    private float _rightBorder;   //��ʉE�[
    private float _leftBorder;   //��ʍ��[
    private bool _isPaused = false; //�X������~���Ă��邩
    private IDisposable _clerkMoveSubscription; //�X���̓�����ێ�
    private IDisposable _pauseSubscription; //���݂̃^�C�}�[��ێ�
    private float _markAnimationScale = 0.3f;  //�A�j���[�V�����Ɏg���X�P�[���l
    private float _animationTime = 2.0f;    //�A�j���[�V��������
    private float _feintProbability = 0.5f; //�t�F�C���g�̊m��
    private float _stopTiming = 2f; //�X�����~�܂�^�C�~���O�̍ő�l
    private SoundManager _soundManager; //SoundManager

    [SerializeField]
    private SpriteRenderer _renderer;   //�X����SpriteRenderer
    [SerializeField]
    private Sprite _yoko;   //�������Ă���Sprite
    [SerializeField]
    private Sprite _front;  //���ʂ������Ă���Sprite
    [SerializeField]
    private Sprite _preFront;   //���ʂ������\������Sprite
    [SerializeField]
    private GameObject _noticeMark; //�C�Â��}�[�N
    [SerializeField]
    private GameObject _angerMark;  //�{��}�[�N
    [SerializeField]
    private AudioClip _walkSE;  //�X��������SE
    [SerializeField]
    private AudioClip _noticeSE;    //������ɋC�Â���SE
    [SerializeField]
    private AudioClip _angerSE; //�{���SE

    public bool IsFront { get; private set; } = false; //�X�������ʂ������Ă��邩
    public ReactiveProperty<bool> ResultSubject { get; private set; } = new ReactiveProperty<bool>();

    // Start is called before the first frame update
    public void Initialize()
    {
        //��ʒ[�����߂�
        var clerkSize = GetComponent<SpriteRenderer>().bounds.size.x;
        _rightBorder = Camera.main.ViewportToWorldPoint(Vector2.one).x - clerkSize;
        _leftBorder = Camera.main.ViewportToWorldPoint(Vector2.zero).x + clerkSize;
        _soundManager = SoundManager.Instance;
        //SE�Đ�
        _soundManager.PlaySE(_walkSE, true);

        //�I�u�W�F�N�g�̈ړ����Ǘ�����Observable
        _clerkMoveSubscription = Observable.EveryUpdate()
            .Where(_ => !_isPaused) //��~���͏������s��Ȃ�
            .Subscribe(_ =>
            {
                _pos = transform.position;

                //�i�|�C���g�j�}�C�i�X�������邱�Ƃŋt�����Ɉړ�����B
                transform.Translate(transform.right * Time.deltaTime * _moveSpeed * _num);

                //��ʒ[�̕��Ɉړ������ۂɌ�����ς���
                if (_pos.x > _rightBorder)
                {
                    _num = -1;
                    this.gameObject.transform.localScale = new Vector3(_clerkSize * _num, _clerkSize, _clerkSize);
                }
                if (_pos.x < _leftBorder)
                {
                    _num = 1;
                    this.gameObject.transform.localScale = new Vector3(_clerkSize * _num, _clerkSize, _clerkSize);
                }
            })
            .AddTo(this);

        // �����_���ȃ^�C�~���O�Œ�~���s��Observable
        RandomPauseClerk();
    }

    ///<summary>
    ///�����_���ɓX�����~�߂�
    ///</summary>
    private void RandomPauseClerk()
    {
        _pauseSubscription?.Dispose(); // �O�̃^�C�}�[��j��

        //�����_���Ȏ��_�ł����������
        _pauseSubscription = Observable.Interval(TimeSpan.FromSeconds(UnityEngine.Random.Range(1f, _stopTiming)))
            .Subscribe(_ =>
            {
                //SE�Đ���~
                _soundManager.StopSE();

                //������������O��
                _isPaused = true;
                _renderer.sprite = _preFront;

                //������������O���łP�b�A�m���ł�����������ĂQ�b�Î~
                var prePauseTimer = Observable.Timer(TimeSpan.FromSeconds(1))
                    .Subscribe(_ =>
                    {
                        //�������U��������Ƃ�
                        if (UnityEngine.Random.Range(0f, 1f) < _feintProbability)
                        {
                            //�����������
                            IsFront = true;
                            _renderer.sprite = _front;

                            //�Q�b��Ɍ��̜p�j�ɖ߂�
                            var pauseTimer = Observable.Timer(TimeSpan.FromSeconds(2)) // ���[�J���ϐ��Ɉꎞ�I�ɕێ�
                            .Subscribe(__ =>
                            {
                                _isPaused = false;
                                IsFront = false;
                                _renderer.sprite = _yoko;
                                RandomPauseClerk(); // �ċA�I�ɌĂяo��
                                //SE�Đ�
                                _soundManager.PlaySE(_walkSE, true);
                            });

                            _pauseSubscription?.Dispose(); // �O�̃^�C�}�[��j��
                            _pauseSubscription = pauseTimer; // �V�����^�C�}�[����
                        }
                        //�t�F�C���g�������Ƃ�
                        else
                        {
                            //�t�F�C���g�̊m����������
                            _feintProbability += 0.2f;
                            _isPaused = false;
                            IsFront = false;
                            _renderer.sprite = _yoko;
                            RandomPauseClerk(); // �ċA�I�ɌĂяo��
                            //SE�Đ�
                            _soundManager.PlaySE(_walkSE, true);
                        }
                    });
                
                _pauseSubscription?.Dispose(); // �O�̃^�C�}�[��j��
                _pauseSubscription = prePauseTimer; // �V�����^�C�}�[����
            })
            .AddTo(this);
    }

    /// <summary>
    /// �X�������A�N�V���������郁�\�b�h
    /// </summary>
    /// <param name="success"></param>
    public void Reaction(bool success)
    {
        //�������~�߂�
        _clerkMoveSubscription?.Dispose();
        _pauseSubscription?.Dispose();

        //�Q�[���ɐ��������Ƃ�
        if (success)
        {
            //SE�Đ�
            _soundManager.PlaySE(_noticeSE, false);
            //�C�Â��}�[�N�̃A�j���[�V����
            _noticeMark.SetActive(true);
            _noticeMark.transform
                .DOPunchScale
                (
                    new Vector2(_markAnimationScale, _markAnimationScale),
                    _animationTime
                )
                .OnComplete(() => AnimationEnd(success));
        }
        //�Q�[���Ɏ��s�����Ƃ�
        else
        {
            _soundManager.PlaySE(_angerSE, false);
            _angerMark.SetActive(true);
            _angerMark.transform
                .DOPunchScale
                (
                    new Vector2(_markAnimationScale, _markAnimationScale),
                    _animationTime
                )
                .OnComplete(() => AnimationEnd(success));
        }

        //���ʂ���������
        _renderer.sprite = _front;
    }

    private void AnimationEnd(bool success)
    {
        ResultSubject.SetValueAndForceNotify(success);
    }

    public void Dispose()
    {
        _clerkMoveSubscription?.Dispose();
        _pauseSubscription?.Dispose();
    }

    private void OnDestroy()
    {
        // �I�u�W�F�N�g�j������Subscription������
        Dispose();
        ResultSubject.Dispose();
    }
}
