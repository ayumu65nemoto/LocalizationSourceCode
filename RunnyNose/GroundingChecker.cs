using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundingChecker : MonoBehaviour
{
    [SerializeField] GameObject _runnyNoseGravityCenter; //鼻水の重心

    private string _groundTag = "Ground"; //Groundタグ

    private bool _grounding; //接地してるかどうか
    public bool GetGrounding()
    {
        return _grounding;
    }
    private void SetGrounding(bool value)
    {
        _grounding = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGrounding(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == _groundTag)
        {
            SetGrounding(true);
            Debug.Log("鼻水が地面に触れました");
        }
    }
}
