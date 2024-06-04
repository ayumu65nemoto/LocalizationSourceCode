using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ControlMethod : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector2(80, 80);
        transform.DOScale(100, 1).SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
