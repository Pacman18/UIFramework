using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using uTools;
using UIContent;

public class UIPopupBase : UIBase
{

    //Popup ID 정의 
    // 88 : 배틀 팝업 
    public enum PopIndex 
    {
        BATTLE_YESNO = 10,
        TRADE_CANCEL = 198,// 서버 진행이전 
        TRADE_CONFIRM_CANCEL = 199, // 서버 진행이후
    };


    [HideInInspector]
    public string Name = "";

    public enum PopupType
    {
        PAGE,
        TOP_BAR,
        SUB_STACK,
        POP_UP,
        FADE,
        WORLD_LAYER,
        FRONT
    }

    public static Color DarkBackColor = new Color(0,0,0,0.8f);

    private PopupType _type;

    public PopupType PopType
    {
        set { _type = value; }
        get { return _type; }
    }
    


    #region Modal & tween Option    
    

    private GameObject _modalGo = null;

    public bool IsModal
    {
        get { return _modal; }

        set
        {
            _modal = value;

            if(value)
                CreateModal();            
        }
    }

    public bool IsModalBlack
    {
        get { return _modal; }

        set
        {
            _modal = value;

            if (value)
                CreateModal();

            SetDarkBack();
        }
    }


    protected bool _modal = false;
    protected RectTransform tweenTrans;
    private float _tweenSpeed = 0.25f;

    private Color _modalColor = new Color(0, 0, 0, 0.0f);
    public UnityAction TweenCallback;
    
    protected Image modalImage;

    // modal 창 make  
    // Todo :: 3D 레이케스팅이 막히는지 봐야한다  2D 오브젝트는 현재 다 막은상태임 
    // Modal 은 Default 로 만들어 놓고 꺼둔다 
    private void CreateModal()
    {
        if (_modalGo != null)
            return;

        _modalGo = new GameObject();
        _modalGo.layer = this.gameObject.layer; 

        _modalGo.name = "ModalImage";        

        _modalGo.transform.parent = transform;
        _modalGo.transform.SetSiblingIndex(0);       

        modalImage = _modalGo.AddComponent<Image>();        
        //modalImage.sprite = UISpriteRepository.i.GetCommonSprite("White");

        RectTransform trans = modalImage.transform as RectTransform;

        trans.sizeDelta = new Vector2(Screen.width, Screen.height);
        trans.localPosition = Vector3.zero;
        trans.localScale = Vector3.one;
        trans.anchorMin = Vector2.zero;
        trans.anchorMax = Vector2.one;

        modalImage.type = Image.Type.Simple;
        modalImage.color = _modalColor;        
    }

    public void SmoothModalShowEffect(float time = 0.3f)
    {
        IsModal = true;
        StartCoroutine(SmoothAlphaModalColor(time));
    }

    public void SmoothModalHideEffect(float time)
    {
        StartCoroutine(SmoothAlphaModalColor(time, true));
    }

    // 스무스 모달 
    private IEnumerator SmoothAlphaModalColor(float smoothTime, bool hide = false)
    {
        float _accTime = 0;

        Color aZeroColor = _modalColor;
        aZeroColor.a = hide ? 1 : 0;
        modalImage.color = aZeroColor;

        float lerpValue = 0;

        while (_accTime < smoothTime)
        {
            _accTime += Time.deltaTime;

            if (_accTime > smoothTime)
                _accTime = smoothTime;

            if (hide)
                lerpValue = (smoothTime - _accTime / smoothTime);
            else
                lerpValue = _accTime / smoothTime;

            aZeroColor.a = lerpValue;
            modalImage.color = aZeroColor;

            yield return null;
        }

        if (hide)
        {
            Color zeroAlpha = _modalColor;
            zeroAlpha.a = 0;
            modalImage.color = zeroAlpha;
        }
        else
            modalImage.color = _modalColor;

    }

    // Modal 색깔 변경 
    protected void ChangeModalColor(Color color)
    {
        if (modalImage)
            modalImage.color = _modalColor = color;
        else
            _modalColor = color;
    }

    public UIData GetData()
    {
        return UIManager.i.GetUIData(this);
    }

    protected void SetDarkBack()
    {
        modalImage.color = _modalColor = DarkBackColor;
    }


    public void ClosedTweenScale()
    {
        TweenCallback = null;

        uTweenScale tween = uTweenScale.Begin(tweenTrans.gameObject, tweenTrans.localScale,  Vector3.zero, _tweenSpeed);
        UnityEvent callback = new UnityEvent();
        callback.AddListener(ClosedFinishedTweenDoCallMethod);
        tween.SetOnFinished(callback);
        tween.method = EaseType.easeInExpo;
    }

    public void ClosedTweenScale(UnityAction callback)
    {
        TweenCallback = callback;

        if (tweenTrans)
        {
            uTweenScale tween = uTweenScale.Begin(tweenTrans.gameObject, tweenTrans.localScale, Vector3.zero, _tweenSpeed);
            UnityEvent callmethod = new UnityEvent();
            callmethod.AddListener(ClosedFinishedTweenDoCallMethod);
            tween.SetOnFinished(callmethod);
        }
        else
            Debug.Log("not found child name tween transform ");
    }


    public void OpenTweenScale()
    {
        TweenCallback = null;

        if (tweenTrans)
        {
            tweenTrans.localScale = Vector3.zero;
            uTweenScale tween = uTweenScale.Begin(tweenTrans.gameObject, Vector3.zero, Vector3.one, _tweenSpeed);
            tween.method = EaseType.easeOutExpo;
        }
        else
            Debug.Log("not found child name tween transform ");
    }


    public void ShowTweenScale(UnityAction callback, float delay = 0)
    {
        TweenCallback = callback;

        if (tweenTrans)
        {
            tweenTrans.localScale = Vector3.zero;
            uTweenScale tween = uTweenScale.Begin(tweenTrans.gameObject, Vector3.zero, Vector3.one, _tweenSpeed);
            tween.delay = delay;
            tween.method = EaseType.easeOutExpo;
            tween.RemoveAllFinished();

            UnityEvent callmethod = new UnityEvent();
            callmethod.AddListener(OpenFinishedTweenDoCallMethod);

            tween.SetOnFinished(callmethod);
        }
        else
            Debug.Log("not found child name tween transform ");
    }

    public uTweenScale ShowTweenScale(Vector3 from, Vector3 to, float time, EaseType method, UnityAction callback)
    {
        TweenCallback = callback;

        if (tweenTrans)
        {
            tweenTrans.localScale = from;
            uTweenScale tween = uTweenScale.Begin(tweenTrans.gameObject, from, to, time);
            tween.method = method;

            UnityEvent callmethod = new UnityEvent();
            callmethod.AddListener(OpenFinishedTweenDoCallMethod);
            tween.SetOnFinished(callmethod);
            return tween;
        }
        else
            Debug.Log("not found child name tween transform ");

        return null;
    }

    protected void OpenFinishedTweenDoCallMethod()
    {
        if (TweenCallback != null)
            TweenCallback.Invoke();

        TweenCallback = null;
    }

    // 트윈이 끝나고 나면 콜백을 실행시키고 본인은 삭제한다
    protected void ClosedFinishedTweenDoCallMethod()
    {
        if (TweenCallback != null)
            TweenCallback.Invoke();

        UIManager.i.RemoveTopPopup();
    }

    public override void Close()
    {
        UIManager.i.RemovePopup(this);
    }

    #endregion


    /// <summary>
    /// Stack팝업 함수로 생성된 팝업이 위로 쌓일때 현재 팝업이 아래로 깔리면서 해줘야하는 함수 
    /// </summary>
    public virtual void BelowStateDoSomething()
    {
        
    }

    /// <summary>
    /// 1. UIData 가 UIManager를 통해서 업데이트가 되면 자동적으로 해당 UI(Active일시)를 업데이트 시켜준다
    /// 2. 스택으로 쌓인 팝업의 최상위가 지워질시 차상위 팝업의 리셋을 UI 매니저에서 불러준다
    /// </summary>
    public virtual void ResetUIUpdate(UIData data)
    {
        ShowBaseUI();
        SetUIData(data);
    }

    


}
