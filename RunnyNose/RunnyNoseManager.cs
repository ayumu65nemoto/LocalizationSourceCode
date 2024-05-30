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

    // Start is called before the first frame update
    void Start()
    {
        GameStart();

        _groundingChecker = _gravityCenter.GetComponent<GroundingChecker>();
        _gravityCenterMove = _gravityCenter.GetComponent<GravityCenterMove>();

        _imageSwitching = _audience.GetComponent<ImageSwitching>();

        _winAction += Win;

        _gameTimer = GameManager.Instance.GameTimer;
        _gameTimer.TimerMethod(10.0f, _winAction, _inGameTimerText);
    }

    // Update is called once per frame
    void Update()
    {
        //鼻水が地面に着く
        if(_groundingChecker.GetGrounding() == true)
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
        _resultView.Initialize(); //リザルト画面の初期化
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
            _resultView.Open(true);
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

            _losing = true;
            _resultView.Open(false);
        }
    }
}
