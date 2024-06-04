using System.Collections.Generic;
using UnityEngine;

public class DestinationData
{
    private List<Vector2> _destinationList = new List<Vector2>();
    private List<string> _destinationStringList = new List<string>();

    public Vector2 CurrentPosition { get; private set; }    //現在の飛行機位置
    public Vector2 NextDestination { get; private set; }    //次の飛行機位置
    public string CurrentPositionName { get; private set; } //現在の国の名前
    public string NextDestinationName { get; private set; } //次の国の名前

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public DestinationData()
    {
        //目的地データ作成
        _destinationList = new()
        {
            new Vector2(6.51f, 0.18f),
            new Vector2(6.51f, 0.18f),  //日本
            new Vector2(-0.79f, 1.41f), //イギリス
            //new Vector2(-5.42f, 1f),    //アメリカ
            new Vector2(-0.2f, 1.22f),  //ドイツ
            new Vector2(0.42f, 0.29f)   //ギリシャ
        };

        //国名データ作成（上記目的地に合わせるように！！）
        _destinationStringList = new()
        {
            "",
            "日本",
            "イギリス",
            //"アメリカ",
            "ドイツ",
            "ギリシャ"
        };

        //初期値を設定
        CurrentPosition = _destinationList[1];
        NextDestination = _destinationList[2];
        CurrentPositionName = _destinationStringList[1];
        NextDestinationName = _destinationStringList[2];
    }

    /// <summary>
    /// 現在の飛行機位置を設定
    /// </summary>
    /// <param name="num">シーンタイプの番号</param>
    public void SetCurrentPosition(int num)
    {
        CurrentPosition = _destinationList[num];
        CurrentPositionName = _destinationStringList[num];
    }

    /// <summary>
    /// 次の飛行機位置を設定
    /// </summary>
    /// <param name="num">シーンタイプの番号</param>
    public void SetNextDestination(int num)
    {
        NextDestination = _destinationList[num];
        NextDestinationName = _destinationStringList[num];
    }
}
