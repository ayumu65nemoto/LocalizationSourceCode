using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Judgment : MonoBehaviour
{
    private IconSelector _iconSelector; //�A�C�R���\���A�I���̃N���X

    [SerializeField]
    private GameObject _winStamp;

    [SerializeField]
    private GameObject _loseStamp;

    private Vector2 _stampPosision = new Vector2(2.22f, -0.7f);

    private bool _isCrear = false;
    public bool GetIsCrear()
    {
        return _isCrear;
    }

    private bool _isStart = false;
    public void SetIsStart(bool value)
    {
        _isStart = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        //_iconSelector = transform.GetComponent<IconSelector>();

        //_winStamp.SetActive(false);
        //_loseStamp.SetActive(false);
    }

    public void StartProcess()
    {
        _iconSelector = transform.GetComponent<IconSelector>();

        _winStamp.SetActive(false);
        _loseStamp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isStart)
        {
            if (_iconSelector.GetCorrectPose() == _iconSelector.GetSelectedPose())
            {
                _isCrear = true;
            }
            else
            {
                _isCrear = false;
            }
        }
    }

    public void JudgmentStamp(bool shutterJudgment)
    {
        if (shutterJudgment)
        {
            _winStamp.SetActive(true);

            //�Q�[����ʂ̐^�񒆂ɔz�u
            _winStamp.transform.position = new Vector2(0, 0);
            _winStamp.transform.localScale = new Vector2(2, 2);

            Vector2 resultScale = new Vector2(0.7f, 0.7f);

            //2�b�Ԃ����āAresultScale�̑傫���ɂ���
            _winStamp.transform.DOScale(resultScale, 0.25f);
        }
        else
        {
            _loseStamp.SetActive(true);

            //�Q�[����ʂ̐^�񒆂ɔz�u
            _loseStamp.transform.position = new Vector3(2.95f, -10.25f, 0 )/*_stampPosision*/;
            _loseStamp.transform.localScale = new Vector2(4, 4);

            Vector2 resultScale = new Vector2(2, 2);

            //2�b�Ԃ����āAresultScale�̑傫���ɂ���
            _loseStamp.transform.DOScale(resultScale, 0.25f);
        }
    }
}
