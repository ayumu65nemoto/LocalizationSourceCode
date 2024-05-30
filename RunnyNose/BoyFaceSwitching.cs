using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoyFaceSwitching : MonoBehaviour
{
    [SerializeField] Sprite _nomalFace; //正面の顔画像
    [SerializeField] Sprite _blowFace; //ななめの顔画像

    [SerializeField] Sprite _winFace; //勝利時の顔画像
    [SerializeField] Sprite _loseFace; //敗北時の顔画像

    private SpriteRenderer _currentSprite; //現在表示しているスプライト

    [SerializeField] GameObject _runnyNoseGravityCenter; //鼻水の重心
    private GravityCenterMove _gravityCenterMove; //クラス

    [SerializeField] GameObject _gameManager; //ゲームマネージャー
    private RunnyNoseManager _runnyNoseManager; //クラス

    // Start is called before the first frame update
    void Start()
    {
        // SpriteRendererコンポーネントを取得
        _currentSprite = GetComponent<SpriteRenderer>();

        _gravityCenterMove = _runnyNoseGravityCenter.GetComponent<GravityCenterMove>(); //重心のクラスを参照

        _runnyNoseManager = _gameManager.GetComponent<RunnyNoseManager>(); //ゲームマネージャーのクラスを参照
    }

    // Update is called once per frame
    void Update()
    {
        //スペースが押されたら
        if (_gravityCenterMove.GetSniffling() == true)
        {
            if (!_runnyNoseManager.GetWinning() && !_runnyNoseManager.GetLosing())
            {
                //鼻をかんでいる顔のスプライトに切り替える
                StartCoroutine("FaceSwitching");
            }
        }

        if(_runnyNoseManager.GetWinning() == true)
        {
            _currentSprite.sprite = _winFace;
        }
        else if (_runnyNoseManager.GetLosing() == true)
        {
            _currentSprite.sprite = _loseFace;
        }

        //transform.DOShakePosition(1f, 3f, 1, 1, false, false);
    }

    /// <summary>
    /// スプライト切り替え
    /// </summary>
    /// <returns></returns>
    IEnumerator FaceSwitching()
    {
        //鼻をかんでいる顔のスプライトに切り替える
        _currentSprite.sprite = _blowFace;

        //0.2秒停止
        yield return new WaitForSeconds(0.2f);

        //元のスプライトに戻す
        _currentSprite.sprite = _nomalFace;

        //鼻をかんでいない状態に戻す
        _gravityCenterMove.SetSniffling(false);
    }
}
