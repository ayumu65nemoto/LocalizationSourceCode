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

    private float _switchingSpan = 3.0f; //振り向くスパン、現状3.0で固定
    private float _currentTime = 0f; //タイマー

    private int _status = 1; //0=こっちを見てる、1=後ろを見てる、2=びっくり顔

    private bool _isLooking; //こっちを見ているかどうか
    public bool GetLooking()
    {
        return _isLooking;
    }
    private void SetLooking(bool value)
    {
        _isLooking = value;
    }

    void Start()
    {
        //SpriteRendererコンポーネントを取得
        _currentImage = GetComponent<SpriteRenderer>();

        //こっちを見ていない
        SetLooking(false);
    }

    void Update()
    {
        //後ろを向いている時
        if(_status == 1)
        {
            //タイマー起動
            _currentTime += Time.deltaTime;
        }      

        //_switchingSpanを満たしたら　且つ　後ろを向いている時
        if (_currentTime > _switchingSpan && _status == 1)
        {
            //前を向かせる
            StartCoroutine("FaceForward");

            //タイマーリセット
            _currentTime = 0f;
        }
    }

    /// <summary>
    /// 前を向く
    /// </summary>
    /// <returns></returns>
    IEnumerator FaceForward()
    {
        //画像切り替え
        _currentImage.sprite = _nanameSprite;

        //
        yield return new WaitForSeconds(1f);

        //画像切り替え
        _currentImage.sprite = _syoumenSprite;

        //前を向いている状態
        _status = 0;

        //こっちを見ている
        SetLooking(true);

        //後ろを向かせる
        StartCoroutine("BackToBackward");
    }

    /// <summary>
    /// 後ろを向く
    /// </summary>
    /// <returns></returns>
    IEnumerator BackToBackward()
    {
        // 秒停止
        yield return new WaitForSeconds(0.5f);

        //画像切り替え
        _currentImage.sprite = _naname2Sprite;

        //こっちを見ていない
        SetLooking(false);

        yield return new WaitForSeconds(0.2f);

        //画像切り替え
        _currentImage.sprite = _uragawaSprite;

        //後ろを向いている状態
        _status = 1;
    }
}
