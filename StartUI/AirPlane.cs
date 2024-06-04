using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AirPlane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMove(new Vector2(562, 153), 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
