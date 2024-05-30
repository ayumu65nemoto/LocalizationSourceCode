using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RunnyNoseLanding : MonoBehaviour
{
    [SerializeField] GameObject _runnyNoseImg; //鼻水画像
    [SerializeField] GameObject _runnyNoseGravityCenter; //鼻水の重心

    private Vector3 _imageScale; //鼻水画像の大きさ

    private Vector3 _firstGravityCenterPosition; //最初の重心の位置
    private Vector3 _currentGravityCenterPosition; //現在の重心の位置

    [SerializeField] GameObject _gameManager; //ゲームマネージャー
    private RunnyNoseManager _runnyNoseManager; //クラス

    // Start is called before the first frame update
    void Start()
    {
        _imageScale = _runnyNoseImg.transform.localScale; //鼻水のサイズ

        _firstGravityCenterPosition = _runnyNoseGravityCenter.transform.position; //最初の重心の位置を保存

        _runnyNoseManager = _gameManager.GetComponent<RunnyNoseManager>(); //ゲームマネージャーのクラスを参照
    }

    // Update is called once per frame
    void Update()
    {
        ExpansionRunnyNose(); //伸びていく鼻水を表現

        if(_imageScale.y < 0)
        {
            _imageScale.y = 0;
        }

        if (_runnyNoseManager.GetWinning())
        {
            //重心の位置で鼻水のスケールを合わせる
            _runnyNoseImg.transform.localScale = new Vector3(_imageScale.x, 0.4f, _imageScale.z);

            transform.DORotate(new Vector3(0, 0, 360), 2, RotateMode.WorldAxisAdd);

        }
        else
        {
            //重心の位置で鼻水のスケールを合わせる
            _runnyNoseImg.transform.localScale = new Vector3(_imageScale.x, _imageScale.y / 15.0f, _imageScale.z);
        }
    }

    /// <summary>
    /// 重心と鼻水サイズの同期
    /// 伸びていく鼻水を表現
    /// </summary>
    private void ExpansionRunnyNose()
    {
        //重心の初期位置による鼻水のサイズ調整
        if (_firstGravityCenterPosition.y < 0)
        {
            _imageScale.y = -_runnyNoseGravityCenter.transform.position.y - _firstGravityCenterPosition.y;
        }
        else if (_firstGravityCenterPosition.y > 0)
        {
            _imageScale.y = -_runnyNoseGravityCenter.transform.position.y + _firstGravityCenterPosition.y;
        }
        else if (_firstGravityCenterPosition.y == 0)
        {
            _imageScale.y = -_runnyNoseGravityCenter.transform.position.y;
        }
    }
}
