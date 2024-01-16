using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

namespace UIContent
{
    public class UISlideGauge : UIBase
    {

        // 그래프가 올라가는 속도 및 내려가는 속도 
        public float PlusSpeed = 1.0f;
        public float MinusSpeed = 1.0f;
        public float SubGraphicWaitTime = 0.8f;
        public float SubSpeed = 50f;

        private bool _textHide = false;
        private UnityAction _aniEndFunc = null;

        // Text 수정 값을 넣는다 
        public string TextValue
        {
            set { _textValue.text = value; }
        }

        public void HideText()
        {
            _textHide = true;
            _textValue.enabled = false;
        }

        // 맥스를 지정시 현재값이 맥스로 저장됨 
        public int MaxValue
        {
            get { return _maxValue; }
            set
            {
                //float percent = _slider.value / _slider.maxValue;
                
                _maxValue = value;

                if(_slider == null)
                {
                    _slider = GetComponentInChildren<Slider>();
                    _textValue = GetText("scrollValue");

                    if (_slider)
                    {
                        RectTransform expArea = _slider.transform.Find("ExpArea") as RectTransform;

                        if(expArea)
                            _expMaxDeltaSize = expArea.rect.size;
                    }

                    _expImage = GetImage("expFill");

                    if (_expImage)
                        _expGauge = _expImage.GetComponent<RectTransform>();
                }


                _slider.maxValue = value;

                Value = value;//Mathf.CeilToInt(percent * _maxValue);

            }
        }


        //  Max 값이 지정이 안되었을시 0 ~ 1 사이의 값을 넣는다 
        public float Value
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;

                if(_slider)
                    _slider.value = value;

                if (IsPercent)
                {
                    _currentPercentage = _currentValue / _maxValue;

                    if (_currentPercentage > 1)
                        _currentPercentage = 1;

                    _currentPercentage *= 100;

                    if(_textHide == false)
                    {
                        _textValue.enabled = true;
                        _textValue.text = string.Format("{0:F1}", _currentPercentage) + _addText;
                    }
                }
                else
                {
                    if (_textHide == false)
                    {
                        _textValue.enabled = true;
                        _textValue.text = string.Format("{0}", (int)_currentValue) + _addText;
                    }
                }
            }
        }

        public virtual float ExpValue
        {
            set
            {
                _expValue = value;
                _expPercentage = _expValue / _maxValue;

                if (_expPercentage > 1)
                    _expPercentage = 1;

                if(_expGauge)
                    _expGauge.sizeDelta = new Vector2(_expMaxDeltaSize.x * _expPercentage, 0);

                _expPercentage *= 100;

                if(_textHide == false)
                    _textValue.text = string.Format("{0:F1}", _expPercentage)+ _addText;
            }

        }

        [HideInInspector]
        public bool IsPercent = false;

        protected int _maxValue = 1;
        protected float _expValue;
        protected float _expPercentage;
        protected float _currentValue;
        protected float _currentPercentage;
        
        [HideInInspector]
        public Text _textValue;

        protected Slider _slider;
        protected Image _expImage;
        protected Vector2 _expMaxDeltaSize;

        protected RectTransform _expGauge;

        protected string _addText;
        protected int _aniloopCount;

        public virtual void Awake()
        {
            _slider = GetComponentInChildren<Slider>();
            _textValue = GetText("scrollValue");

            if (_slider)
            {
                RectTransform expArea = _slider.transform.Find("ExpArea") as RectTransform;
                if(expArea)
                    _expMaxDeltaSize = expArea.rect.size;
            }

            _expImage = GetImage("expFill");

            if (_expImage)
                _expGauge = _expImage.GetComponent<RectTransform>();
        }

        public void AddOnChangeValue(UnityAction<float> callbackEvent)
        {
            Slider.SliderEvent callEvent = new Slider.SliderEvent();
            callEvent.AddListener(callbackEvent);
            _slider.onValueChanged = callEvent;
        }

        public void EndText(string text)
        {
            _addText = text;
        }

        public void ShowHideExpGauge(bool onOff)
        {
            _expImage.enabled = onOff;
        }

        public void Reset()
        {
            MaxValue = 1;
            Value = 0;
            ExpValue = 0;

            if(_textHide == false)
                _textValue.text = "0";
        }

        private IEnumerator sliderDecreaseAnimation(int end, float time, UnityAction finish = null)
        {   

            float add = _currentValue - end;
            float offset = (add / time); // 시간을 기준으로한  분배 ( Timedelta)        

            Value = _currentValue;        

            while (_currentValue > end)
            {
                _currentValue -= (offset * Time.deltaTime * MinusSpeed);

                Value = _currentValue;

                yield return null;
            }

            Value = end;

            if (finish != null)
            {
                finish.Invoke();
                finish = null;
            }
        }

        protected IEnumerator SubDecreaseGraph()
        {
            if (_expValue < _currentValue)
                ExpValue = _currentValue;

            yield return new WaitForSeconds(SubGraphicWaitTime);

            while (_expValue > _currentValue)
            {
                _expValue -= SubSpeed * Time.deltaTime;
                ExpValue = _expValue;


                if (_expValue <= _currentValue)
                    _expValue = _currentValue;

                yield return null;
            }

            if (_aniEndFunc != null)
            {
                _aniEndFunc.Invoke();
                //_aniEndFunc = null;
            }
        }

        private IEnumerator sliderIncreaseAnimation(int end, float time, UnityAction finish = null)
        {

            float add = end - _currentValue;
            float offset = (add / time); // 시간을 기준으로한  분배 ( Timedelta)        

            Value = _currentValue;

            //print("add : " + add + " time : " + time + " offset :" + offset );

            while (_currentValue < end)
            {
                _currentValue += (offset * Time.deltaTime * PlusSpeed);

                Value = _currentValue;

                yield return null;
            }

            Value = end;

            if (finish != null)
            {
                finish.Invoke();
                finish = null;
            }
        }

        /// <summary>
        /// 애니메이션 슬라이드 시작점
        /// </summary>
        /// <param name="dest">최종 Exp</param>
        /// <param name="loopCount">애니메이션 레벨업 카운트</param>
        /// <param name="finish">최종 애니메이션 종료시 콜백</param>
        /// <param name="upcallBack"> 1업 마다시 콜백</param>
        /// <param name="time">총 애니메이션 시간</param>
        public void IncreaseGraphic(int dest, int loopCount = 0, UnityAction finish = null, UnityAction upcallBack = null, float time = 1.0f)
        {
            StopAllCoroutines();        
            StartCoroutine(sliderAnimation(dest, time, loopCount, finish, upcallBack));
        }


        public virtual void DecreaseGraphic(int dest , float time , bool subAni = false, UnityAction finish = null)
        {


            if (_currentValue == dest)
                return;        

            StopAllCoroutines();
            if (subAni)
            {
                _aniEndFunc = finish;
                StartCoroutine(sliderDecreaseAnimation(dest, time, () => StartCoroutine(SubDecreaseGraph())));
            }
            else
                StartCoroutine(sliderDecreaseAnimation(dest, time, finish));
        }




        private IEnumerator sliderAnimation(int end, float time,int loopCount, UnityAction finish = null, UnityAction upCallback = null)
        {
            float divideTime;

            if (loopCount == 0)
                divideTime = time;
            else
            {
                divideTime = time / (float)(loopCount);  // 순환횟수만큼 나누어주어야 총합 시간이 내가 원한시간이 된다 
                upCallback += () => { Value = 0; };
            }        

            // 단순 증가 애니메이션 
            if (loopCount == 0)
            {
                yield return StartCoroutine(sliderIncreaseAnimation(end, divideTime, finish));
                yield break;
            }        

            // Loop 애니메이션
            while (loopCount > 0) 
            {                        
                loopCount--;
                yield return StartCoroutine(sliderIncreaseAnimation(_maxValue, divideTime, upCallback));                                    
            }

            // 최종 애니메이션
            yield return StartCoroutine(sliderIncreaseAnimation(end, divideTime, finish));
        }

        private void OnDisable()
        {
            StopAllCoroutines();        
        }
        /*
        void OnGUI()
        {
            if (GUI.Button(new Rect(100, 0, 100, 100), "Init"))
            {
                IsPercent = true;
                MaxValue = 20;
                Value = 1;
                EndText("%");

            }        

            if (GUI.Button(new Rect(100, 200, 100, 100), "LoopAni"))
            {   
                IncreaseGraphic(4, 3, null, Test, 3);
            }

            if (GUI.Button(new Rect(200, 200, 100, 100), "NexLevel"))
            {
                IncreaseGraphic(10, 2, null, Test, 3);
            }
        }

        int index = 10;

        void Test()
        {
            MaxValue = index * 10;
            index++;

            print(MaxValue);
        }
        */
    }
}

