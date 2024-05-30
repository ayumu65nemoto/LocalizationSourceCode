using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CysharpTest : MonoBehaviour
{
    [SerializeField] private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        var clickStream = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0));

        clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
            .Where(xs => xs.Count >= 2)
            .Subscribe(_ => RedToBlue());
    }

    private async UniTask RedToBlue()
    {
        // ① Imageオブジェクトを「赤」へ  
        _image.color = Color.magenta;
        // ② 2秒間処理を待機  
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        // ③ Imageオブジェクトを「青」へ  
        _image.color = Color.cyan;

        _image.gameObject.transform.DOLocalMove(new Vector3(10f, 0, 0), 1f).SetEase(Ease.Linear);
    }
}
