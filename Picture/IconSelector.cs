using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IconSelector : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer[] _icons; // �C���X�y�N�^�[�ŃA�C�R�������蓖�Ă�

    [SerializeField] 
    Image[] _poses; // �C���X�y�N�^�[�ŉ摜�����蓖�Ă�

    [SerializeField]
    SpriteRenderer[] _backgrounds; // �C���X�y�N�^�[�Ŕw�i�����蓖�Ă�

    private Vector3 _rightIconPos = new Vector3(254, -121, 0); //�A�C�R����z�u����ʒu�@�E
    private Vector3 _leftIconPos = new Vector3(-254, -121, 0); //�A�C�R����z�u����ʒu�@��
    private SpriteRenderer _rightIcon; //�E���̃A�C�R���̃X�v���C�g
    private SpriteRenderer _leftIcon; //�����̃A�C�R���̃X�v���C�g
    private int _rightIconIndex; //�E���̃A�C�R���̔ԍ�������
    private int _leftIconIndex; //�����̃A�C�R���̔ԍ�������

    private int _correctPose; //�����̃A�C�R���̔ԍ�������
    public int GetCorrectPose()
    {
        return _correctPose;
    }

    private int _selectedPose; //�I�𒆂̃A�C�R���̔ԍ�������
    public int GetSelectedPose()
    {
        return _selectedPose;
    }

    private bool _canSelect; //����\�ɂ���

    private int _selectedCountry;   //�I�����ꂽ�w�i�i���j�̔ԍ�
    [SerializeField]
    private ResultView _resultView;
    [SerializeField]
    private Sprite[] _sprites;

    //�A�C�R�������˂�Tween
    private Tween _rightYoyo;
    private Tween _leftYoyo;

    private SoundManager _soundManager; //�T�E���h�}�l�[�W���[

    [SerializeField]
    private AudioClip _selectSE; //�V���b�^�[��SE

    void Start()
    {
        _canSelect = false;

        _soundManager = SoundManager.Instance;
    }

    public void StartProcess()
    {
        _selectedCountry = Random.Range(0, _backgrounds.Length); //�w�i�摜�̐��̒����烉���_���Ȓl���o��
        SetBackGround(_selectedCountry); //�I�����ꂽ�w�i�摜

        for (int i = 0; i < _poses.Length; i++)
        {
            _poses[i].gameObject.SetActive(false); //��x���ׂẴ|�[�Y�摜���\��        
        }

        SetIconsByCountry(_selectedCountry); //�����ƂɎw�肳��Ă���2�̃A�C�R����\��

        _rightIcon.color = Color.white; //�ŏ��͈ꗥ�ŁA�E���̉摜��I�ԁi���邭�\���j�悤�ɂ���
        _leftIcon.color = Color.gray; //�����̉摜�͈Â��\��

        SetPoses(_rightIconIndex); //�E���̃A�C�R���ɑΉ����Ă���|�[�Y��\��
        _selectedPose = _rightIconIndex; //�E���̃A�C�R����I�����Ă���

        _rightYoyo = _rightIcon.transform.DOJump(_rightIcon.transform.position, 1f, 1, 0.5f).SetLoops(-1, LoopType.Yoyo);
        _leftYoyo = _leftIcon.transform.DOJump(_leftIcon.transform.position, 1f, 1, 0.5f).SetLoops(-1, LoopType.Yoyo);
        //_rightYoyo.Pause();
        _leftYoyo.Pause();

        _canSelect = true;

        //���U���g��ʂ̉摜������
        _resultView.SetResultImage(_sprites[_selectedCountry]);
    }

    void Update()
    {
        if (_canSelect)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            _soundManager.PlaySE(_selectSE, false);

            _rightIcon.color = Color.white; // �I�����ꂽ�A�C�R���𖾂邭����
            _leftIcon.color = Color.gray;

            //�I�����ꂽ�A�C�R�����҂��҂�񒵂˂����A��I���A�C�R����DOTween���~����
            _leftYoyo.Pause();
            _rightYoyo.Play();

            SetPoses(_rightIconIndex);
            _selectedPose = _rightIconIndex;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _soundManager.PlaySE(_selectSE, false);

            _leftIcon.color = Color.white; // �I�����ꂽ�A�C�R���𖾂邭����
            _rightIcon.color = Color.gray;

            //�I�����ꂽ�A�C�R�����҂��҂�񒵂˂����A��I���A�C�R����DOTween���~����
            _rightYoyo.Pause();
            _leftYoyo.Play();

            SetPoses(_leftIconIndex);
            _selectedPose = _leftIconIndex;
        }
    }

    private void SetIconsByCountry(int selectedIndex)
    {
        //�A�����J�̏ꍇ
        if(selectedIndex == 0)
        {
            SetIcon(0, 3);
        }
        //�M���V���̏ꍇ
        else if (selectedIndex == 1)
        {
            SetIcon(2, 0);
        }
    }

    private void SetIcon(int correctAnswer, int wrongAnswer)
    {
        for (int i = 0; i < _icons.Length; i++)
        {
            _icons[i].gameObject.SetActive(false); //��x���ׂẴA�C�R���摜���\��        
        }

        _correctPose = correctAnswer;

        int random = Random.Range(0, 2); //�����_�� 0~1

        //���E�ŁA�����̃A�C�R���ƊԈႢ�̃A�C�R���������_���ɓ���ւ���
        if(random == 0)
        {
            _icons[correctAnswer].gameObject.SetActive(true);
            _rightIcon = _icons[correctAnswer];
            _rightIconIndex = correctAnswer;
            _icons[correctAnswer].gameObject.transform.localPosition = _rightIconPos;

            _icons[wrongAnswer].gameObject.SetActive(true);
            _leftIcon = _icons[wrongAnswer];
            _leftIconIndex = wrongAnswer;
            _icons[wrongAnswer].gameObject.transform.localPosition = _leftIconPos;
        }
        else if (random == 1)
        {
            _icons[wrongAnswer].gameObject.SetActive(true);
            _rightIcon = _icons[wrongAnswer];
            _rightIconIndex = wrongAnswer;
            _icons[wrongAnswer].gameObject.transform.localPosition = _rightIconPos;

            _icons[correctAnswer].gameObject.SetActive(true);
            _leftIcon = _icons[correctAnswer];
            _leftIconIndex = correctAnswer;
            _icons[correctAnswer].gameObject.transform.localPosition = _leftIconPos;
        }
    }

    private void SetPoses(int selectedIndex)
    {
        for (int i = 0; i < _poses.Length; i++)
        {
            if (i == selectedIndex)
            {
                _poses[i].gameObject.SetActive(true); // �I�����ꂽ�摜��\��

                //�X�P�[�����傫���Ȃ�DOTween
                _poses[i].gameObject.transform.localScale = new Vector3(0, 0, 0);
                _poses[i].gameObject.transform.DOScale(new Vector3(1.94f, 1.94f, 1.94f), 0.1f);
            }
            else
            {
                _poses[i].gameObject.SetActive(false); // ���̉摜���\��
            }
        }
    }

    private void SetBackGround(int selectedIndex)
    {
        for (int i = 0; i < _backgrounds.Length; i++)
        {
            if (i == selectedIndex)
            {
                _backgrounds[i].gameObject.SetActive(true); // �I�����ꂽ�摜��\��
            }
            else
            {
                _backgrounds[i].gameObject.SetActive(false); // ���̉摜���\��
            }
        }
    }
}
