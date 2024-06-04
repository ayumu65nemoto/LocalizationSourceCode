using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AirplaneMove : MonoBehaviour
{
    [SerializeField] private GameObject[] _positions = new GameObject[5];

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] path = new Vector3[_positions.Length];
        for (int i = 0; i < _positions.Length; i++)
        {
            path[i] = _positions[i].transform.position;
        }

        //(通る点の座標配列, スタートからゴールまでの時間, 直線補完か曲線補完か)
        transform.DOLocalPath(path, 2.0f, PathType.CatmullRom)
                .SetEase(Ease.Linear)
                .SetLookAt(0.01f, Vector3.up, Vector3.forward)
                .SetOptions(false, AxisConstraint.Z /*AxisConstraint.X*/ /*| AxisConstraint.Y*/);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
