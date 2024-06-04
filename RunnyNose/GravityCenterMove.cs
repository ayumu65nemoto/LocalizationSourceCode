using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCenterMove : MonoBehaviour
{
    private Rigidbody2D _rb; //自身のリジッドボディ

    private bool _sniffling; //鼻をすすっているかどうか
    public bool GetSniffling()
    {
        return _sniffling;
    }
    public void SetSniffling(bool value)
    {
        _sniffling = value;
    }

    [SerializeField] GameObject _runnyNoseManagerObj; //ゲームマネージャー
    private RunnyNoseManager _runnyNoseManager; //クラス

    private bool _isStart = false; //一斉スタート用の変数
    public void SetIsStart(bool value)
    {
        _isStart = value;
    }

    private SoundManager _soundManager;

    [SerializeField]
    private AudioClip _sniffleSE;    //鼻をすするSE

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); //Rigidbody2Dコンポーネントを取得

        _rb.isKinematic = true; //ゲームが始まるまで重力を無効化する

        SetSniffling(false);

        _runnyNoseManager = _runnyNoseManagerObj.GetComponent<RunnyNoseManager>(); //ゲームマネージャーのクラスを参照

        _soundManager = SoundManager.Instance;
    }

    void Update()
    {
        //ゲームが始まったら重力を有効化する
        if (_isStart)
        {
            _rb.isKinematic = false;
        }

        //ゲーム終了時（勝利or敗北）意外では鼻水の操作を可能にする
        if(!_runnyNoseManager.GetLosing() && !_runnyNoseManager.GetWinning())
        {
            RunnyNoseJump(); //鼻をすすった時の動作
        }
    }

    /// <summary>
    /// 鼻をすすった時の動作
    /// </summary>
    private void RunnyNoseJump()
    {
        //スペースキー操作
        if (Input.GetKeyDown(KeyCode.Space) && _isStart)
        {
            _soundManager.PlaySE(_sniffleSE, false);

            //一度加わっている力をリセット
            _rb.velocity = Vector3.zero;

            //上方向に瞬間的に力を加える
            _rb.AddForce(Vector3.up * 5, ForceMode2D.Impulse);

            //鼻水をすすっている状態に移行
            SetSniffling(true);
        }
    }
}
