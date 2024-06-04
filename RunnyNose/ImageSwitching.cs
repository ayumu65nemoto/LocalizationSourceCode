using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitching : MonoBehaviour
{
    [SerializeField] Sprite _syoumenSprite; //正面
    [SerializeField] Sprite _nanameSprite; //正面を向く際の差分

    [SerializeField] Sprite _uragawaSprite; //裏側
    [SerializeField] Sprite _naname2Sprite; //後ろを向く際の差分

    [SerializeField] Sprite _bikkuriSprite; //びっくり

    private SpriteRenderer _currentImage;

    private float _switchingSpan = 5.0f; //振り向くスパン、現状3.0で固定
    private float _currentTime = 0f; //タイマー

    private bool _isLooking; //こっちを見ているかどうか
    public bool GetLooking()
    {
        return _isLooking;
    }
    private void SetLooking(bool value)
    {
        _isLooking = value;
    }

    private bool _isStart = false; //一斉スタート用の変数
    public void SetIsStart(bool value)
    {
        _isStart = value;
    }

    Coroutine _someCoroutine; //コルーチン停止用の箱

    private SoundManager _soundManager;

    [SerializeField]
    private AudioClip _noticeSE;    //鼻をすするSE

    [SerializeField]
    private AudioClip _angrySE;    //鼻をすするSE

    void Start()
    {
        //SpriteRendererコンポーネントを取得
        _currentImage = GetComponent<SpriteRenderer>();
        //最初は後ろを向かせる
        _currentImage.sprite = _uragawaSprite;

        //こっちを見ていない
        SetLooking(false);

        _soundManager = SoundManager.Instance;
    }

    void Update()
    {
        if (_isStart)
        {
            TurnAroundPerTime();
        }
    }

    private void TurnAroundPerTime()
    {
        //後ろを向いている時
        if (!_isLooking)
        {
            //タイマー起動
            _currentTime += Time.deltaTime;

            //_switchingSpanを満たしたら
            if (_currentTime > _switchingSpan)
            {
                //前を向かせる
                _someCoroutine = StartCoroutine(FaceForward());

                //タイマーリセット
                _currentTime = 0f;
            }
        }
    }

    /// <summary>
    /// 前を向く
    /// </summary>
    /// <returns></returns>
    IEnumerator FaceForward()
    {
        //こっちに気づくSE
        _soundManager.PlaySE(_noticeSE, false);

        //画像切り替え
        _currentImage.sprite = _nanameSprite;

        //
        yield return new WaitForSeconds(0.2f);

        //画像切り替え
        _currentImage.sprite = _syoumenSprite;

        //こっちを見ている
        SetLooking(true);

        // 秒停止
        yield return new WaitForSeconds(0.5f);

        //画像切り替え
        _currentImage.sprite = _naname2Sprite;

        //こっちを見ていない
        SetLooking(false);

        yield return new WaitForSeconds(0.2f);

        if (_isStart)
        {
            //画像切り替え
            _currentImage.sprite = _uragawaSprite;
        }
        else if (!_isStart)
        {
            _currentImage.sprite = _bikkuriSprite; //怒りのオンニスプライト
        }
    }

    /// <summary>
    /// ゲーム失敗時に呼ばれる、観客の怒り演出処理
    /// </summary>
    public void SetAngryFace()
    {
        _isStart = false; //振り向くまでの時間計測を停止

        //StopCoroutine(_someCoroutine); //振り向きを停止

        _soundManager.PlaySE(_angrySE, false); //高森の怒り音声

        _currentImage.sprite = _bikkuriSprite; //怒りのオンニスプライト
    }
}
