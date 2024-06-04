using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using System;

public class RunnyNoseManager : MonoBehaviour
{
    [SerializeField] GameObject _gravityCenter; //�@���̏d�S�I�u�W�F�N�g
    private GroundingChecker _groundingChecker; //�@���̐ڒn����N���X
    private GravityCenterMove _gravityCenterMove; //�@���������Ă��邩

    [SerializeField] GameObject _audience; //�ϋq�I�u�W�F�N�g
    private ImageSwitching _imageSwitching; //�����������Ă��邩

    private GameTimer _gameTimer; //�I���j�̃Q�[���}�l�[�W���[

    private Action _winAction; //�������̊֐�
    [SerializeField] TMP_Text _inGameTimerText; //�^�C�}�[�p�̃e�L�X�g
    [SerializeField] TMP_Text _startGameTimerText; //�Q�[���J�n���Ԃ̃e�L�X�g

    [SerializeField] ResultView _resultView; //ResultView�N���X

    private bool _losing = false; //���������𖈃t���[���Ă΂Ȃ��悤�ɂ��邽�߂Ɏg��
    public bool GetLosing()
    {
        return _losing;
    }

    private bool _winning = false; //���������𖈃t���[���Ă΂Ȃ��悤�ɂ��邽�߂Ɏg��
    public bool GetWinning()
    {
        return _winning;
    }

    private bool _isStart = false; //��ăX�^�[�g�p�̕ϐ�
    public void SetIsStart(bool value)
    {
        _isStart = value;
    }

    private bool _timerOn = false; //�^�C�}�[��1�񂾂��N�������邽��

    // Start is called before the first frame update
    void Start()
    {
        if (_isStart)
        {

        }
        GameStart(); //�Q�[�����n�߂鏈��

        //�e�N���X���Q��
        _groundingChecker = _gravityCenter.GetComponent<GroundingChecker>();
        _gravityCenterMove = _gravityCenter.GetComponent<GravityCenterMove>();
        _imageSwitching = _audience.GetComponent<ImageSwitching>();

        //�������̏����A���Ԍv���̏����A���擾
        _winAction += Win;
        _gameTimer = GameManager.Instance.GameTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isStart && !_timerOn)
        {
            //�^�C�}�[���쓮������ԂɈڍs�A���L�̏�����1�񂾂��s��
            _timerOn = true;

            //10�b�ς����珟�����̏��������s
            _gameTimer.TimerMethod(10.0f, _winAction, _inGameTimerText);
        }

        //�@�����n�ʂɒ���
        if (_groundingChecker.GetGrounding() == true)
        {
            Lose();
        }

        //�ϋq�������������Ă��鎞�ɕ@���������Ă��܂�
        if (_gravityCenterMove.GetSniffling() == true && _imageSwitching.GetLooking() == true)
        {
            Lose();
        }
    }

    /// <summary>
    /// �Q�[�����n�߂�
    /// </summary>
    private void GameStart()
    {
        Debug.Log("�Q�[���J�n�I");
        _resultView.Initialize();
        //���U���g��Ƀ��[���h�}�b�v�ɖ߂鏈���ݒ�
        GameManager.Instance.SetNextScene();
        _resultView.OnClickReturnMainButton.Subscribe(_ => GameManager.Instance.SceneData.BackToWorldMap());
    }

    /// <summary>
    /// �Q�[���N���A
    /// </summary>
    private void Win()
    {
        if (!_winning && !_losing)
        {
            Debug.Log("�����I");

            _winning = true;
            _resultView.OpenAsync(true);

            GameManager.Instance.GameTimer.Dispose();
        }       
    }

    /// <summary>
    /// �Q�[���I�[�o�[
    /// </summary>
    private void Lose()
    {
        if (!_losing && !_winning)
        {
            Debug.Log("�����I");

            _imageSwitching.SetAngryFace();

            _losing = true;

            _resultView.OpenAsync(false);

            GameManager.Instance.GameTimer.Dispose();
        }
    }
}
