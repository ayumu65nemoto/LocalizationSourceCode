using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using System;

public class RunnyNoseManager : MonoBehaviour
{
    [SerializeField] GameObject _gravityCenter; //鼻水の重心オブジェクト
    private GroundingChecker _groundingChecker; //鼻水の接地判定クラス
    private GravityCenterMove _gravityCenterMove; //鼻をすすっているか

    [SerializeField] GameObject _audience; //観客オブジェクト
    private ImageSwitching _imageSwitching; //こっちを見ているか

    private GameTimer _gameTimer; //オンニのゲームマネージャー

    private Action _winAction; //勝利時の関数
    [SerializeField] TMP_Text _inGameTimerText; //タイマー用のテキスト
    [SerializeField] TMP_Text _startGameTimerText; //ゲーム開始時間のテキスト

    [SerializeField] ResultView _resultView; //ResultViewクラス

    private bool _losing = false; //負け処理を毎フレーム呼ばないようにするために使う
    public bool GetLosing()
    {
        return _losing;
    }

    private bool _winning = false; //勝ち処理を毎フレーム呼ばないようにするために使う
    public bool GetWinning()
    {
        return _winning;
    }

    private bool _isStart = false; //一斉スタート用の変数
    public void SetIsStart(bool value)
    {
        _isStart = value;
    }

    private bool _timerOn = false; //タイマーを1回だけ起動させるため

    // Start is called before the first frame update
    void Start()
    {
        if (_isStart)
        {

        }
        GameStart(); //ゲームを始める処理

        //各クラスを参照
        _groundingChecker = _gravityCenter.GetComponent<GroundingChecker>();
        _gravityCenterMove = _gravityCenter.GetComponent<GravityCenterMove>();
        _imageSwitching = _audience.GetComponent<ImageSwitching>();

        //勝利時の処理、時間計測の処理、を取得
        _winAction += Win;
        _gameTimer = GameManager.Instance.GameTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isStart && !_timerOn)
        {
            //タイマーが作動した状態に移行、下記の処理を1回だけ行う
            _timerOn = true;

            //10秒耐えたら勝利時の処理を実行
            _gameTimer.TimerMethod(10.0f, _winAction, _inGameTimerText);
        }

        //鼻水が地面に着く
        if (_groundingChecker.GetGrounding() == true)
        {
            Lose();
        }

        //観客がこっちを見ている時に鼻をすすってしまう
        if (_gravityCenterMove.GetSniffling() == true && _imageSwitching.GetLooking() == true)
        {
            Lose();
        }
    }

    /// <summary>
    /// ゲームを始める
    /// </summary>
    private void GameStart()
    {
        Debug.Log("ゲーム開始！");
        _resultView.Initialize();
        //リザルト後にワールドマップに戻る処理設定
        GameManager.Instance.SetNextScene();
        _resultView.OnClickReturnMainButton.Subscribe(_ => GameManager.Instance.SceneData.BackToWorldMap());
    }

    /// <summary>
    /// ゲームクリア
    /// </summary>
    private void Win()
    {
        if (!_winning && !_losing)
        {
            Debug.Log("勝ち！");

            _winning = true;
            _resultView.OpenAsync(true);

            GameManager.Instance.GameTimer.Dispose();
        }       
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    private void Lose()
    {
        if (!_losing && !_winning)
        {
            Debug.Log("負け！");

            _imageSwitching.SetAngryFace();

            _losing = true;

            _resultView.OpenAsync(false);

            GameManager.Instance.GameTimer.Dispose();
        }
    }
}
