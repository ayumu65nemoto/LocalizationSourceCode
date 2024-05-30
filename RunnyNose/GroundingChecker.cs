using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundingChecker : MonoBehaviour
{
    [SerializeField] GameObject _runnyNoseGravityCenter; //�@���̏d�S

    private string _groundTag = "Ground"; //Ground�^�O

    private bool _grounding; //�ڒn���Ă邩�ǂ���
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
            Debug.Log("�@�����n�ʂɐG��܂���");
        }
    }
}
