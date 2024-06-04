using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _regionName;
    [SerializeField] private GameObject _title;
    [SerializeField] private GameObject _subTitle;
    [SerializeField] private GameObject _controlMethod;
    [SerializeField] private GameObject _controlDescription;
    [SerializeField] private GameObject _airPlane;

    private float _startWaitTime = 2.0f;
    public bool IsFinishIntro { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        _regionName.SetActive(false);
        _title.SetActive(false);
        _subTitle.SetActive(false);
        _controlMethod.SetActive(false);
        _controlDescription.SetActive(false);
        _airPlane.SetActive(false);

        IsFinishIntro = false;

        StartCoroutine(StartUIMove());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator StartUIMove()
    {
        yield return new WaitForSeconds(0.5f);

        //タイトル
        _title.SetActive(true);
        _title.transform.localScale = new Vector2(3, 3);
        float titleDoTime = 0.5f;
        _title.transform.DOScale(new Vector2(1.0f, 1.0f), titleDoTime);

        yield return new WaitForSeconds(titleDoTime);

        //サブタイトル
        _subTitle.SetActive(true);
        _subTitle.transform.localScale = new Vector2(0.9f, 0.9f);
        float subTitleDoTime = 0.5f;
        _subTitle.transform.DOScale(new Vector2(1.0f, 1.0f), subTitleDoTime);

        yield return new WaitForSeconds(subTitleDoTime);

        //操作説明ボタン
        _controlMethod.SetActive(true);
        _controlMethod.transform.localScale = new Vector2(0.9f, 0.9f);
        float controlMethodDoTime = 0.5f;
        _controlMethod.transform.DOScale(new Vector2(1.0f, 1.0f), controlMethodDoTime);
        //操作説明テキスト
        _controlDescription.SetActive(true);
        _controlDescription.transform.localScale = new Vector2(0.9f, 0.9f);
        float controlDescriptionDoTime = 0.5f;
        _controlDescription.transform.DOScale(new Vector2(1.0f, 1.0f), controlDescriptionDoTime);

        _airPlane.SetActive(true);
        _regionName.SetActive(true);

        yield return new WaitForSeconds(_startWaitTime);
        IsFinishIntro = true;
    }
}
