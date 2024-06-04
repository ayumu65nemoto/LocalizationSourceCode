using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class ShutterManager : MonoBehaviour
{
    private float _spanToReleaseShutter = 0.5f; //�V���b�^�[��؂�X�p��
    private ScreenShot _screenShot; //�X�N���[���V���b�g�̃N���X

    [SerializeField] 
    private GameObject _flashObject; //�t���b�V�����o�̂��߂̂ł��������l�p�`

    [SerializeField] 
    private TextMeshProUGUI _timerText; //�V���b�^�[���؂���܂ł̎��ԕ\���e�L�X�g

    private float _time; //�V���b�^�[�Ԋu�Ɏg���^�C��
    private float _shutterSpan = 5.0f; //3�b�ŃV���b�^�[���؂���

    private bool _isTimeUp = false; //�V���b�^�[���؂�ꂽ��true�ɂȂ� 

    private int _NumberOfShots = 1; //�B�e�����
    private int _NumberOfShotsTaken = 0; //���݂̎B�e��

    [SerializeField]
    private GameObject _characterSelector; //�N���A����N���X�����I�u�W�F�N�g
    private Judgment _judgment; //�N���A����N���X

    private bool _shutterJudgment;

    private bool _isStart = false;
    public void SetIsStart(bool value)
    {
        _isStart = value;
    }

    [SerializeField]
    private ResultView _resultView;

    private SoundManager _soundManager; //�T�E���h�}�l�[�W���[

    [SerializeField]
    private AudioClip _shutterSE; //�V���b�^�[��SE

    [SerializeField]
    private AudioClip _hiCheeseSE; //�V���b�^�[��SE

    // Start is called before the first frame update
    void Start()
    {
        //_screenShot = transform.GetComponent<ScreenShot>(); //�X�N���[���V���b�g�̃N���X

        //_judgment = _characterSelector.GetComponent<Judgment>();

        //_time = _shutterSpan; //�^�C�}�[���Z�b�g

        _resultView.Initialize(); //���U���g��ʂ̏�����
        //���U���g��Ƀ��[���h�}�b�v�ɖ߂鏈���ݒ�
        GameManager.Instance.SetNextScene();
        _resultView.OnClickReturnMainButton.Subscribe(_ => GameManager.Instance.SceneData.BackToWorldMap());

        _soundManager = SoundManager.Instance;
    }

    public void StartProcess()
    {
        _screenShot = transform.GetComponent<ScreenShot>(); //�X�N���[���V���b�g�̃N���X

        _judgment = _characterSelector.GetComponent<Judgment>();

        _time = _shutterSpan; //�^�C�}�[���Z�b�g
    }

    // Update is called once per frame
    void Update()
    {
        if(_NumberOfShots > _NumberOfShotsTaken && _isStart)
        {
            if (_isTimeUp)
            {
                StartCoroutine(ReleaseShutter());
            }

            if (!_isTimeUp)
            {
                ShutterTimer();
            }
        }
    }

    /// <summary>
    /// �t���b�V�����o�ƁA�ۑ����ꂽ�ʐ^�̕\��
    /// </summary>
    /// <returns></returns>
    IEnumerator ReleaseShutter()
    {
        _isTimeUp = false;

        _screenShot.SaveScreenshot();

        //�͂��`�[�YSE
        _soundManager.PlaySE(_hiCheeseSE, false);

        yield return new WaitForSeconds(0.1f);

        //�V���b�^�[SE
        _soundManager.PlaySE(_shutterSE, false);

        _flashObject.SetActive(true);

        _shutterJudgment = _judgment.GetIsCrear(); //�V���b�^�[��؂�u�Ԃ̐��딻����B��

        yield return new WaitForSeconds(0.2f); //�����X�v���C�g����u�\�����ăt���b�V����

        _flashObject.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        _screenShot.ShowSSImage(); //�ۑ������ʐ^��\������֐����Ăяo��

        yield return new WaitForSeconds(1.0f);

        _judgment.JudgmentStamp(_shutterJudgment);

        yield return new WaitForSeconds(1.0f);

        _resultView.OpenAsync(_shutterJudgment);

        _NumberOfShotsTaken++;
    }

    /// <summary>
    /// �V���b�^�[���؂���܂ł̎c�莞�Ԃ�\������^�C�}�[
    /// </summary>
    private void ShutterTimer()
    {
        //_time = 3.0f
        if (0 <= _time)
        {
            _time -= Time.deltaTime;
            _timerText.text = _time.ToString("N0");
        }
        else if (0 >= _time)
        {
            _isTimeUp = true;

            _time = _shutterSpan; //�^�C�}�[���Z�b�g
        }
    }
}
