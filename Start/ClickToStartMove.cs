using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;      // ’Ç‰Á
using TMPro;

public class ClickToStartMove : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    // Start is called before the first frame update
    void Start()
    {
        _text.DOFade(0.0f, 1.0f).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
