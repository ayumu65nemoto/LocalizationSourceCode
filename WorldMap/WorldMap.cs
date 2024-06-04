using UnityEngine;

public class WorldMap : MonoBehaviour
{
    private float _moveTime = 3.0f; //�ړ�����

    [SerializeField]
    private PlaneMove _planePrefab; //��s�@�v���n�u
    [SerializeField]
    private AudioClip _bgm; //BGM
    [SerializeField]
    private TextScroll _textScroll;

    private async void Start()
    {
        //BGM�Đ�
        SoundManager.Instance.PlayBGM(_bgm);

        //�ړI�n�e�L�X�g���X�N���[��
        _textScroll.Initialize();

        var gameManager = GameManager.Instance;
        var destinationData = gameManager.DestinationData;
        var sceneData = gameManager.SceneData;

        //��s�@�𐶐�
        var plane = Instantiate(_planePrefab, destinationData.CurrentPosition, Quaternion.identity);

        //�ړ������J�n
        await plane.MapMove(destinationData.CurrentPosition, destinationData.NextDestination, _moveTime);

        //�V�[���̑J��
        if (sceneData.NextSceneType == SceneData.SceneType.WorldMap) sceneData.SetNextSceneType(SceneData.SceneType.WorldMap);
        sceneData.LoadScene(sceneData.NextSceneType);
    }
}
