using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class UIFadePanel : UIPopupBase
{

    private Image _fadeImage;

    private float _fadeTime = 1.0f;

    private bool _isFadeing = false;
    

    [HideInInspector]
    public UnityAction FadeInCallbackMethod = null;

    [HideInInspector]
    public UnityAction FadeOutCallbackMethod = null;


    void Awake()
    {
        _fadeImage = GetImage("Img_Black");
    }


    public void StartFadeOut(float time, UnityAction callback = null , bool destory = false )
    {
        FadeOutCallbackMethod = callback;

        if(!_isFadeing)
        {
            StartCoroutine(fadeOut(destory, time));
        }        
    }

    public void StartFadeIn(float time, UnityAction callback = null, bool destory = true)
    {
        FadeInCallbackMethod = callback;

        if (!_isFadeing)
        {            
            StartCoroutine(fadeIn(destory,time));
        }       
    }


    // 페이드 아웃 & In 이후 콜백 메서드 실행 
    public void FadeOutInDestory(UnityAction callback = null, float time = 1.0f)
    {
        FadeOutCallbackMethod = rigtFadeout;
        FadeInCallbackMethod = callback;

        _relayTime = time;

        if (!_isFadeing)
        {
            StartCoroutine(fadeOut(false ,time));
        }
    }

    private float _relayTime;

    private void rigtFadeout()
    {
        StartCoroutine(fadeIn(true , _relayTime));
    }


    public IEnumerator fadeOut(bool destory, float endTime)
    {
        float accTime = 0;
        float percentage = 0;
        _fadeTime = endTime;

        _isFadeing = true;

        Color color = new Color(0,0,0,0);
        _fadeImage.color = color; 

        while (accTime < _fadeTime)
        {
            accTime += Time.smoothDeltaTime;

            percentage = accTime / _fadeTime;

            color = _fadeImage.color;

            color.a = percentage;

            _fadeImage.color = color;

            yield return null;
        }

        _fadeImage.color = new Color(0,0,0,1);

        _isFadeing = false;


        if (FadeOutCallbackMethod != null)
        {
            FadeOutCallbackMethod.Invoke();
            FadeOutCallbackMethod = null;
        }        

        if (destory)
            UIManager.i.RemovePopup<UIFadePanel>();
    }


    public IEnumerator fadeIn(bool destory,  float endTime)
    {
        float accTime = 0;
        float percentage = 0;
        _fadeTime = endTime;

        _isFadeing = true;

        Color color = new Color(0, 0, 0, 1);
        _fadeImage.color = color;

        while (accTime < _fadeTime)
        {
            accTime += Time.smoothDeltaTime;

            percentage = 1 - ( accTime / _fadeTime );

            color = _fadeImage.color;

            color.a = percentage;

            _fadeImage.color = color;

            yield return null;
        }

        _fadeImage.color = new Color(0, 0, 0, 0);

        _isFadeing = false;

        if (FadeInCallbackMethod != null)
        {
            FadeInCallbackMethod.Invoke();
            FadeInCallbackMethod = null;
        }        

        if (destory)
            UIManager.i.RemovePopup<UIFadePanel>();
    }







}
