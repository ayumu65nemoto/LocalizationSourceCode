using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoyFaceSwitching : MonoBehaviour
{
    [SerializeField] Sprite _nomalFace; //通常時の顔画像
    [SerializeField] Sprite _blowFace; //鼻水をすする時の顔画像

    [SerializeField] Sprite _winFace; //勝利時の顔画像
    [SerializeField] Sprite _loseFace; //敗北時の顔画像

    private SpriteRenderer _currentSprite; //現在表示しているスプライト

    [SerializeField] GameObject _runnyNoseGravityCenter; //鼻水の重心
    private GravityCenterMove _gravityCenterMove; //クラス

    [SerializeField] GameObject _runnyNoseManagerObj; //ゲームマネージャー
    private RunnyNoseManager _runnyNoseManager; //クラス

    private bool _isStart = false; //一斉スタート用の変数
    public void SetIsStart(bool value)
    {
        _isStart = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        //SpriteRendererコンポーネントを取得
        _currentSprite = transform.GetComponent<SpriteRenderer>();
        //重心のクラスを参照
        _gravityCenterMove = _runnyNoseGravityCenter.GetComponent<GravityCenterMove>();
        //ゲームマネージャーのクラスを参照
        _runnyNoseManager = _runnyNoseManagerObj.GetComponent<RunnyNoseManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //鼻水をすすっている状態になったら
        if (_gravityCenterMove.GetSniffling() == true)
        {
            //鼻水をすすっている顔のスプライトに切り替える
            StartCoroutine("FaceSwitching");    
        }

        ToggleToResultFace();
    }

    private void ToggleToResultFace()
    {
        //勝利時、敗北時に、それぞれの顔画像に切り替える
        if (_runnyNoseManager.GetWinning() == true)
        {
            _currentSprite.sprite = _winFace;
        }
        else if (_runnyNoseManager.GetLosing() == true)
        {
            _currentSprite.sprite = _loseFace;
        }
    }

    /// <summary>
    /// スプライト切り替え
    /// </summary>
    /// <returns></returns>
    IEnumerator FaceSwitching()
    {
        //ゲーム継続中なら
        if (!_runnyNoseManager.GetWinning() && !_runnyNoseManager.GetLosing())
        {
            //鼻をかんでいる顔のスプライトに切り替える
            _currentSprite.sprite = _blowFace;

            //0.2秒停止
            yield return new WaitForSeconds(0.1f);

            //元のスプライトに戻す
            _currentSprite.sprite = _nomalFace;

            //鼻をかんでいない状態に戻す
            _gravityCenterMove.SetSniffling(false);
        }
    }
}
