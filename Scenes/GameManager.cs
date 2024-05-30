using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SceneData SceneData { get; private set; }
    public DestinationData DestinationData { get; private set; }
    public GameTimer GameTimer { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            //シーン遷移しても破棄されないようにする
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //二重で起動されないようにする
            Destroy(gameObject);
        }

        //シーンデータ作成
        SceneData = new SceneData();
        //目的地データ作成
        DestinationData = new DestinationData();
        //タイマークラス作成
        GameTimer = new GameTimer();
    }

    /// <summary>
    /// 次のミニゲームに移る準備
    /// </summary>
    public void SetNextScene()
    {
        SceneData.SetNextSceneType(SceneData.CurrentSceneType);
        DestinationData.SetCurrentPosition((int)SceneData.CurrentSceneType);
        DestinationData.SetNextDestination((int)SceneData.NextSceneType);
    }
}
