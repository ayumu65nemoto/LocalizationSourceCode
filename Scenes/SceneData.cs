using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneData
{
	private Dictionary<SceneType, string> _sceneTypeDict;   //シーンタイプとシーン名の紐づけ

	public enum SceneType   //シーンのタイプ
	{
		Start = 0,	//スタート画面
		WorldMap = 1,	//ワールドマップ
		CallTheClerk = 2,   //店員を呼べ
		RunnyNose = 3,	//鼻水ゲーム
	}
	public SceneType CurrentSceneType { get; private set; } //現在のシーンタイプ
	public SceneType NextSceneType { get; private set; }	//次のシーンタイプ

	/// <summary>
	/// コンストラクタ
	/// シーンタイプのDictionaryにstring紐づけ
	/// </summary>
	public SceneData()
    {
		_sceneTypeDict = new()
		{
			[SceneType.Start] = "Start",
			[SceneType.WorldMap] = "WorldMap",
			[SceneType.CallTheClerk] = "CallTheClerk",
			[SceneType.RunnyNose] = "RunnyNose"
		};

		//初期値設定
		CurrentSceneType = SceneType.Start;
    }

	/// <summary>
	/// シーンの読み込み
	/// </summary>
	/// <param name="sceneType">遷移したいシーンタイプ</param>
	public void LoadScene(SceneType sceneType)
    {
		//現在のシーンタイプを更新
		CurrentSceneType = sceneType;
		//シーンをロード
		SceneManager.LoadScene(_sceneTypeDict[sceneType]);
    }

	/// <summary>
	/// 次のシーンタイプをセットする
	/// </summary>
	/// <param name="current">現在のシーンタイプ</param>
	public void SetNextSceneType(SceneType current)
	{
		// SceneTypeの全ての値を配列に取得
		SceneType[] values = (SceneType[])Enum.GetValues(typeof(SceneType));

		// 現在の値のインデックスを取得
		int currentIndex = Array.IndexOf(values, current);

		// 次のインデックスを計算
		int nextIndex = currentIndex + 1;

		// 次のインデックスが配列の範囲内かどうかを確認
		if (nextIndex < values.Length)
		{
			NextSceneType =  values[nextIndex];
		}
        else
        {
			NextSceneType = SceneType.WorldMap;
        }
	}

	/// <summary>
	/// ワールドマップに戻る処理
	/// </summary>
	public void BackToWorldMap()
    {
		LoadScene(SceneType.WorldMap);
    }
}
