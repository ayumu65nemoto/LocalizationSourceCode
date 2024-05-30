using System.Collections.Generic;
using UnityEngine;

public class DestinationData
{
    private List<Vector2> _destinationList = new List<Vector2>();

    public Vector2 CurrentPosition { get; private set; }    //現在の飛行機位置
    public Vector2 NextDestination { get; private set; }    //次の飛行機位置

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public DestinationData()
    {
        //目的地データ作成
        _destinationList = new()
        {
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(6.51f, 0.18f),
            new Vector2(4.2f, 2.5f)
        };

        //初期値を設定
        CurrentPosition = _destinationList[1];
        NextDestination = _destinationList[2];
    }

    /// <summary>
    /// 現在の飛行機位置を設定
    /// </summary>
    /// <param name="num">シーンタイプの番号</param>
    public void SetCurrentPosition(int num)
    {
        CurrentPosition = _destinationList[num];
    }

    /// <summary>
    /// 次の飛行機位置を設定
    /// </summary>
    /// <param name="num">シーンタイプの番号</param>
    public void SetNextDestination(int num)
    {
        NextDestination = _destinationList[num];
    }
}
