using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class CallClerk : MonoBehaviour
{
    private Vector2 _rotateValue = Vector2.zero;
    private Vector2 _imageSize = new Vector2(0.3f, 0.3f);

    [SerializeField]
    private GameObject _riseArm;    //腕を上げる画像
    [SerializeField]
    private GameObject _speak;  //吹き出し
    [SerializeField]
    private AudioClip _callSE;  //店員を呼ぶSE

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        _riseArm.SetActive(false);
        _speak.SetActive(false);
    }

    /// <summary>
    /// 腕を上げる処理
    /// </summary>
    public async UniTask RiseArm()
    {
        _riseArm.SetActive(true);
        Tween tween =  _riseArm.transform.DORotate(_rotateValue, 1.0f);
        // アニメーションの完了を待機
        await tween.AsyncWaitForCompletion();
    }

    /// <summary>
    /// 声を上げる処理
    /// </summary>
    public async UniTask Speak()
    {
        SoundManager.Instance.PlaySE(_callSE, false);
        _speak.SetActive(true);
        Tween tween = _speak.transform.DOScale(_imageSize, 1.0f);
        // アニメーションの完了を待機
        await tween.AsyncWaitForCompletion();
    }
}
