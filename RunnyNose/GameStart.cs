using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField]
    private GameObject _gravityCenterMoveObj;
    private GravityCenterMove _gravityCenterMove;

    [SerializeField]
    private GameObject _audienceObj;
    private ImageSwitching _imageSwitching;

    [SerializeField]
    private GameObject _runnyNoseManagerObj;
    private RunnyNoseManager _runnyNoseManager;

    [SerializeField]
    private StartUIManager _startUIPanel;

    [SerializeField]
    private GameObject _inGameTimerText;

    private bool _alreadyStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        _gravityCenterMove = _gravityCenterMoveObj.GetComponent<GravityCenterMove>();
        _imageSwitching = _audienceObj.GetComponent<ImageSwitching>();
        _runnyNoseManager = _runnyNoseManagerObj.GetComponent<RunnyNoseManager>();

        _startUIPanel.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_alreadyStarted && _startUIPanel.IsFinishIntro)
        {
            _startUIPanel.gameObject.SetActive(false);
            _inGameTimerText.gameObject.SetActive(true);
            SetIsStartAll();

            _alreadyStarted = true;
        }
    }

    private void SetIsStartAll()
    {
        _gravityCenterMove.SetIsStart(true);
        _imageSwitching.SetIsStart(true);
        _runnyNoseManager.SetIsStart(true);
    }
}
