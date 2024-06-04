using UnityEngine;
using UniRx;
using System;
using DG.Tweening;

public class Clerk : MonoBehaviour
{
    private Vector2 _pos;   //店員の位置
    private float _clerkSize = 0.3f;   //店員のサイズ
    private int _num = 1;   //店員がどちらに向いているか
    private float _moveSpeed = 4.0f;    //店員の移動速度
    private float _rightBorder;   //画面右端
    private float _leftBorder;   //画面左端
    private bool _isPaused = false; //店員が停止しているか
    private IDisposable _clerkMoveSubscription; //店員の動きを保持
    private IDisposable _pauseSubscription; //現在のタイマーを保持
    private float _markAnimationScale = 0.3f;  //アニメーションに使うスケール値
    private float _animationTime = 2.0f;    //アニメーション時間
    private float _feintProbability = 0.5f; //フェイントの確率
    private float _stopTiming = 2f; //店員が止まるタイミングの最大値
    private SoundManager _soundManager; //SoundManager

    [SerializeField]
    private SpriteRenderer _renderer;   //店員のSpriteRenderer
    [SerializeField]
    private Sprite _yoko;   //横を向ているSprite
    [SerializeField]
    private Sprite _front;  //正面を向いているSprite
    [SerializeField]
    private Sprite _preFront;   //正面を向く予備動作Sprite
    [SerializeField]
    private GameObject _noticeMark; //気づきマーク
    [SerializeField]
    private GameObject _angerMark;  //怒りマーク
    [SerializeField]
    private AudioClip _walkSE;  //店員が歩くSE
    [SerializeField]
    private AudioClip _noticeSE;    //こちらに気づいたSE
    [SerializeField]
    private AudioClip _angerSE; //怒りのSE

    public bool IsFront { get; private set; } = false; //店員が正面を向いているか
    public ReactiveProperty<bool> ResultSubject { get; private set; } = new ReactiveProperty<bool>();

    // Start is called before the first frame update
    public void Initialize()
    {
        //画面端を求める
        var clerkSize = GetComponent<SpriteRenderer>().bounds.size.x;
        _rightBorder = Camera.main.ViewportToWorldPoint(Vector2.one).x - clerkSize;
        _leftBorder = Camera.main.ViewportToWorldPoint(Vector2.zero).x + clerkSize;
        _soundManager = SoundManager.Instance;
        //SE再生
        _soundManager.PlaySE(_walkSE, true);

        //オブジェクトの移動を管理するObservable
        _clerkMoveSubscription = Observable.EveryUpdate()
            .Where(_ => !_isPaused) //停止中は処理を行わない
            .Subscribe(_ =>
            {
                _pos = transform.position;

                //（ポイント）マイナスをかけることで逆方向に移動する。
                transform.Translate(transform.right * Time.deltaTime * _moveSpeed * _num);

                //画面端の方に移動した際に向きを変える
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

        // ランダムなタイミングで停止を行うObservable
        RandomPauseClerk();
    }

    ///<summary>
    ///ランダムに店員を止める
    ///</summary>
    private void RandomPauseClerk()
    {
        _pauseSubscription?.Dispose(); // 前のタイマーを破棄

        //ランダムな時点でこちらを向く
        _pauseSubscription = Observable.Interval(TimeSpan.FromSeconds(UnityEngine.Random.Range(1f, _stopTiming)))
            .Subscribe(_ =>
            {
                //SE再生停止
                _soundManager.StopSE();

                //こちらを向く前兆
                _isPaused = true;
                _renderer.sprite = _preFront;

                //こちらを向く前兆で１秒、確率でこちらを向いて２秒静止
                var prePauseTimer = Observable.Timer(TimeSpan.FromSeconds(1))
                    .Subscribe(_ =>
                    {
                        //こちらを振り向いたとき
                        if (UnityEngine.Random.Range(0f, 1f) < _feintProbability)
                        {
                            //こちらを向く
                            IsFront = true;
                            _renderer.sprite = _front;

                            //２秒後に元の徘徊に戻る
                            var pauseTimer = Observable.Timer(TimeSpan.FromSeconds(2)) // ローカル変数に一時的に保持
                            .Subscribe(__ =>
                            {
                                _isPaused = false;
                                IsFront = false;
                                _renderer.sprite = _yoko;
                                RandomPauseClerk(); // 再帰的に呼び出す
                                //SE再生
                                _soundManager.PlaySE(_walkSE, true);
                            });

                            _pauseSubscription?.Dispose(); // 前のタイマーを破棄
                            _pauseSubscription = pauseTimer; // 新しいタイマーを代入
                        }
                        //フェイントだったとき
                        else
                        {
                            //フェイントの確率を下げる
                            _feintProbability += 0.2f;
                            _isPaused = false;
                            IsFront = false;
                            _renderer.sprite = _yoko;
                            RandomPauseClerk(); // 再帰的に呼び出す
                            //SE再生
                            _soundManager.PlaySE(_walkSE, true);
                        }
                    });
                
                _pauseSubscription?.Dispose(); // 前のタイマーを破棄
                _pauseSubscription = prePauseTimer; // 新しいタイマーを代入
            })
            .AddTo(this);
    }

    /// <summary>
    /// 店員がリアクションをするメソッド
    /// </summary>
    /// <param name="success"></param>
    public void Reaction(bool success)
    {
        //動きを止める
        _clerkMoveSubscription?.Dispose();
        _pauseSubscription?.Dispose();

        //ゲームに成功したとき
        if (success)
        {
            //SE再生
            _soundManager.PlaySE(_noticeSE, false);
            //気づきマークのアニメーション
            _noticeMark.SetActive(true);
            _noticeMark.transform
                .DOPunchScale
                (
                    new Vector2(_markAnimationScale, _markAnimationScale),
                    _animationTime
                )
                .OnComplete(() => AnimationEnd(success));
        }
        //ゲームに失敗したとき
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

        //正面を向かせる
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
        // オブジェクト破棄時にSubscriptionを解除
        Dispose();
        ResultSubject.Dispose();
    }
}
