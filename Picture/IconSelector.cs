using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IconSelector : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer[] _icons; // インスペクターでアイコンを割り当てる

    [SerializeField] 
    Image[] _poses; // インスペクターで画像を割り当てる

    [SerializeField]
    SpriteRenderer[] _backgrounds; // インスペクターで背景を割り当てる

    private Vector3 _rightIconPos = new Vector3(254, -121, 0); //アイコンを配置する位置　右
    private Vector3 _leftIconPos = new Vector3(-254, -121, 0); //アイコンを配置する位置　左
    private SpriteRenderer _rightIcon; //右側のアイコンのスプライト
    private SpriteRenderer _leftIcon; //左側のアイコンのスプライト
    private int _rightIconIndex; //右側のアイコンの番号が入る
    private int _leftIconIndex; //左側のアイコンの番号が入る

    private int _correctPose; //正解のアイコンの番号が入る
    public int GetCorrectPose()
    {
        return _correctPose;
    }

    private int _selectedPose; //選択中のアイコンの番号が入る
    public int GetSelectedPose()
    {
        return _selectedPose;
    }

    private bool _canSelect; //操作可能にする

    private int _selectedCountry;   //選択された背景（国）の番号
    [SerializeField]
    private ResultView _resultView;
    [SerializeField]
    private Sprite[] _sprites;

    //アイコンが跳ねるTween
    private Tween _rightYoyo;
    private Tween _leftYoyo;

    private SoundManager _soundManager; //サウンドマネージャー

    [SerializeField]
    private AudioClip _selectSE; //シャッター音SE

    void Start()
    {
        _canSelect = false;

        _soundManager = SoundManager.Instance;
    }

    public void StartProcess()
    {
        _selectedCountry = Random.Range(0, _backgrounds.Length); //背景画像の数の中からランダムな値を出す
        SetBackGround(_selectedCountry); //選択された背景画像

        for (int i = 0; i < _poses.Length; i++)
        {
            _poses[i].gameObject.SetActive(false); //一度すべてのポーズ画像を非表示        
        }

        SetIconsByCountry(_selectedCountry); //国ごとに指定されている2つのアイコンを表示

        _rightIcon.color = Color.white; //最初は一律で、右側の画像を選ぶ（明るく表示）ようにする
        _leftIcon.color = Color.gray; //左側の画像は暗く表示

        SetPoses(_rightIconIndex); //右側のアイコンに対応しているポーズを表示
        _selectedPose = _rightIconIndex; //右側のアイコンを選択している

        _rightYoyo = _rightIcon.transform.DOJump(_rightIcon.transform.position, 1f, 1, 0.5f).SetLoops(-1, LoopType.Yoyo);
        _leftYoyo = _leftIcon.transform.DOJump(_leftIcon.transform.position, 1f, 1, 0.5f).SetLoops(-1, LoopType.Yoyo);
        //_rightYoyo.Pause();
        _leftYoyo.Pause();

        _canSelect = true;

        //リザルト画面の画像を決定
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

            _rightIcon.color = Color.white; // 選択されたアイコンを明るくする
            _leftIcon.color = Color.gray;

            //選択されたアイコンをぴょんぴょん跳ねさせ、非選択アイコンのDOTweenを停止する
            _leftYoyo.Pause();
            _rightYoyo.Play();

            SetPoses(_rightIconIndex);
            _selectedPose = _rightIconIndex;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _soundManager.PlaySE(_selectSE, false);

            _leftIcon.color = Color.white; // 選択されたアイコンを明るくする
            _rightIcon.color = Color.gray;

            //選択されたアイコンをぴょんぴょん跳ねさせ、非選択アイコンのDOTweenを停止する
            _rightYoyo.Pause();
            _leftYoyo.Play();

            SetPoses(_leftIconIndex);
            _selectedPose = _leftIconIndex;
        }
    }

    private void SetIconsByCountry(int selectedIndex)
    {
        //アメリカの場合
        if(selectedIndex == 0)
        {
            SetIcon(0, 3);
        }
        //ギリシャの場合
        else if (selectedIndex == 1)
        {
            SetIcon(2, 0);
        }
    }

    private void SetIcon(int correctAnswer, int wrongAnswer)
    {
        for (int i = 0; i < _icons.Length; i++)
        {
            _icons[i].gameObject.SetActive(false); //一度すべてのアイコン画像を非表示        
        }

        _correctPose = correctAnswer;

        int random = Random.Range(0, 2); //ランダム 0~1

        //左右で、正解のアイコンと間違いのアイコンをランダムに入れ替える
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
                _poses[i].gameObject.SetActive(true); // 選択された画像を表示

                //スケールが大きくなるDOTween
                _poses[i].gameObject.transform.localScale = new Vector3(0, 0, 0);
                _poses[i].gameObject.transform.DOScale(new Vector3(1.94f, 1.94f, 1.94f), 0.1f);
            }
            else
            {
                _poses[i].gameObject.SetActive(false); // 他の画像を非表示
            }
        }
    }

    private void SetBackGround(int selectedIndex)
    {
        for (int i = 0; i < _backgrounds.Length; i++)
        {
            if (i == selectedIndex)
            {
                _backgrounds[i].gameObject.SetActive(true); // 選択された画像を表示
            }
            else
            {
                _backgrounds[i].gameObject.SetActive(false); // 他の画像を非表示
            }
        }
    }
}
