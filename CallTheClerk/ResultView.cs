using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ResultView : MonoBehaviour
{
    [SerializeField]
    private GameObject _resultUIGroup;  //リザルト画面のUI
    [SerializeField]
    private TMP_Text _resultText;   //リザルト画面のテキスト
    [SerializeField]
    private Button _returnMainButton;

    public IObservable<Unit> OnClickReturnMainButton => _returnMainButton.OnClickAsObservable();

    public void Initialize()
    {
        _resultUIGroup.SetActive(false);
        _resultText.text = "";
    }

    /// <summary>
    /// リザルト表示
    /// </summary>
    /// <param name="success"></param>
    public void Open(bool success)
    {
        _resultUIGroup.SetActive(true);

        if (success)
        {
            _resultText.text = "ゲームクリア！";
        }
        else
        {
            _resultText.text = "ゲームオーバー";
        }
    }
}
