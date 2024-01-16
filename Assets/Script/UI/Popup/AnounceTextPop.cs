using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using uTools;

public class AnounceTextPop : UIPopupBase
{

    private Text _announce;
    private uTweenColor _colorTween;
    private uTweenPosition _positionTween;
    private uTweenScale _scaleTween;

    public uTweenColor ColorTween
    {
        get { return _colorTween; }
    }

    public uTweenPosition PositionTween
    {
        get { return _positionTween; }
    }

    public uTweenScale ScaleTween
    {
        get { return _scaleTween; }
    }


    void Awake ()
    {
        _announce = GetComponentInChildren<Text>();
        _colorTween = GetComponentInChildren<uTweenColor>(true);
        _positionTween = GetComponentInChildren<uTweenPosition>(true);
        _scaleTween = GetComponentInChildren<uTweenScale>(true);

        UnityEvent tweenEndEvent = new UnityEvent();
        tweenEndEvent.AddListener(()=> UIManager.i.RemovePopup<AnounceTextPop>());

        _colorTween.SetOnFinished(tweenEndEvent);
        _positionTween.SetOnFinished(tweenEndEvent);
        _scaleTween.SetOnFinished(tweenEndEvent);
    }

    public void SetUIData(string message)
    {
        _announce.text = message;
    }

}
