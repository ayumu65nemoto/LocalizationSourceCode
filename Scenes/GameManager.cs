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
            //�V�[���J�ڂ��Ă��j������Ȃ��悤�ɂ���
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //��d�ŋN������Ȃ��悤�ɂ���
            Destroy(gameObject);
        }

        //�V�[���f�[�^�쐬
        SceneData = new SceneData();
        //�ړI�n�f�[�^�쐬
        DestinationData = new DestinationData();
        //�^�C�}�[�N���X�쐬
        GameTimer = new GameTimer();
    }

    /// <summary>
    /// ���̃~�j�Q�[���Ɉڂ鏀��
    /// </summary>
    public void SetNextScene()
    {
        SceneData.SetNextSceneType(SceneData.CurrentSceneType);
        DestinationData.SetCurrentPosition((int)SceneData.CurrentSceneType);
        DestinationData.SetNextDestination((int)SceneData.NextSceneType);
    }
}
