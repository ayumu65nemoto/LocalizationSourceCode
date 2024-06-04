using UnityEngine;

public class WorldMap : MonoBehaviour
{
    private float _moveTime = 3.0f; //移動時間

    [SerializeField]
    private PlaneMove _planePrefab; //飛行機プレハブ
    [SerializeField]
    private AudioClip _bgm; //BGM
    [SerializeField]
    private TextScroll _textScroll;

    private async void Start()
    {
        //BGM再生
        SoundManager.Instance.PlayBGM(_bgm);

        //目的地テキストをスクロール
        _textScroll.Initialize();

        var gameManager = GameManager.Instance;
        var destinationData = gameManager.DestinationData;
        var sceneData = gameManager.SceneData;

        //飛行機を生成
        var plane = Instantiate(_planePrefab, destinationData.CurrentPosition, Quaternion.identity);

        //移動処理開始
        await plane.MapMove(destinationData.CurrentPosition, destinationData.NextDestination, _moveTime);

        //シーンの遷移
        if (sceneData.NextSceneType == SceneData.SceneType.WorldMap) sceneData.SetNextSceneType(SceneData.SceneType.WorldMap);
        sceneData.LoadScene(sceneData.NextSceneType);
    }
}
