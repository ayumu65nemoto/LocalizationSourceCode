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

    [SerializeField] GameObject _gameManager; //ゲームマネージャー
    private RunnyNoseManager _runnyNoseManager; //クラス

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); //Rigidbody2Dコンポーネントを取得

        SetSniffling(false);

        _runnyNoseManager = _gameManager.GetComponent<RunnyNoseManager>(); //ゲームマネージャーのクラスを参照
    }

    void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //一度加わっている力をリセット
            _rb.velocity = Vector3.zero;

            //上方向に瞬間的に力を加える
            _rb.AddForce(Vector3.up * 5, ForceMode2D.Impulse);

            //鼻水をすすっている状態に移行
            SetSniffling(true);
        }
    }
}
