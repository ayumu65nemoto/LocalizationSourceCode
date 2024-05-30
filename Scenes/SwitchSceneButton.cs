using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSceneButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    public IObservable<Unit> OnClickStartButton => _button.OnClickAsObservable();
}
