using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _shutterManagerObj;
    private ShutterManager _shutterManager;

    [SerializeField]
    private GameObject _judgmentObj;
    private Judgment _judgment;

    [SerializeField]
    private GameObject _iconSelectorObj;
    private IconSelector _iconSelector;

    [SerializeField]
    private StartUIManager _startUIPanel;

    private bool _alreadyStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        _shutterManager = _shutterManagerObj.GetComponent<ShutterManager>();
        _judgment = _judgmentObj.GetComponent<Judgment>();
        _iconSelector = _iconSelectorObj.GetComponent<IconSelector>();

        _startUIPanel.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            && !_alreadyStarted
            && _startUIPanel.IsFinishIntro)
        {
            _startUIPanel.gameObject.SetActive(false);

            ConcurrentStartProcess();

            _alreadyStarted = true;
        }
    }

    private void ConcurrentStartProcess()
    {
        _judgment.SetIsStart(true);
        _shutterManager.SetIsStart(true);

        _iconSelector.StartProcess();
        _shutterManager.StartProcess();
        _judgment.StartProcess();        
    }
}
