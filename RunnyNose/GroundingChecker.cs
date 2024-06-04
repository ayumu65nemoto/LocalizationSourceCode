using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundingChecker : MonoBehaviour
{
    [SerializeField] 
    GameObject _runnyNoseGravityCenter; //•@…‚ÌdS

    [SerializeField] 
    GameObject _splashRunnyNose; //”ò‚ÑU‚Á‚½•@…

    private string _groundTag = "Ground"; //Groundƒ^ƒO

    private bool _grounding; //Ú’n‚µ‚Ä‚é‚©‚Ç‚¤‚©
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

            Instantiate(_splashRunnyNose, transform.position + new Vector3(0.0f, 0.5f), Quaternion.identity);

            Debug.Log("•@…‚ª’n–Ê‚ÉG‚ê‚Ü‚µ‚½");
        }
    }
}
