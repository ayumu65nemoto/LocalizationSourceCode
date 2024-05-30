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
        // �@ Image�I�u�W�F�N�g���u�ԁv��  
        _image.color = Color.magenta;
        // �A 2�b�ԏ�����ҋ@  
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        // �B Image�I�u�W�F�N�g���u�v��  
        _image.color = Color.cyan;

        _image.gameObject.transform.DOLocalMove(new Vector3(10f, 0, 0), 1f).SetEase(Ease.Linear);
    }
}
