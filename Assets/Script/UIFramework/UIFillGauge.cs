using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;


namespace UIContent
{
    /// <summary>
    ///  정수만 들어와서 쓸수 있도록 한 그래프 FillAmount 클래스 
    /// </summary>
    public class UIFillGauge : UIBase
    {
        // 그래프가 올라가는 속도 및 내려가는 속도 
        public float PlusSpeed = 1.0f;
        public float MinusSpeed = 1.0f;
        public float SubGraphicWaitTime = 0.8f;
        public float SubSpeed = 50f;

        private UnityAction _aniEndFunc = null;
        private bool _textHide = false;
        private bool _expTextNo = false;

        private Coroutine _coroutine;

        public bool OnlyValueText
        {
            set { _expTextNo = value; }
        }

        // Text 수정 값을 넣는다 
        public string TextValue
        {
            set
            {
                if(_textValue)
                    _textValue.text = value;
            }
        }


        // 맥스를 지정시 현재값이 맥스로 저장됨 
        public int MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                Value = value;
            }
        }


        //  Max 값이 지정이 안되었을시 0 ~ 1 사이의 값을 넣는다 
        public float Value
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;

                _currentPercentage = _currentValue / _maxValue;

                if (_currentPercentage > 1)
                    _currentPercentage = 1;


                if (_fillSprite == null)
                    Init();

                _fillSprite.fillAmount = _currentPercentage;

                _textValue.text = Mathf.RoundToInt(_currentValue).ToString();
            
            }
        }

        /// <summary>
        /// PrepareComponent 구조때문에 어쩔수없이 사용할때 쓴다
        /// </summary>
        public void Init()
        {
            if (_fillSprite == null)
            {
                _fillSprite = GetImage("Img_Fill");
                _expImage = GetImage("Img_Exp");
                _textValue = GetText("Text_Value");
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

                _expImage.fillAmount = _expPercentage;

                _expPercentage *= 100;

                /*if (_expTextNo == false && _textHide == false && _textValue)
                    _textValue.text = string.Format("{0:F1}", _expPercentage + _addText);*/
            }

        }

        [HideInInspector]
        public bool IsPercent = true;

        protected int _maxValue = 1;
        protected float _expValue;
        protected float _expPercentage;
        protected float _currentValue;
        protected float _currentPercentage;        

        private Image _fillSprite;
        public Text _textValue;
        protected Image _expImage;    

        protected string _addText;
        protected int _aniloopCount;


        void Awake()
        {
            _fillSprite = GetImage("Img_Fill");
            _expImage = GetImage("Img_Exp");
            _textValue = GetText("Text_Value");        
        }

        public void EndText(string text)
        {
            _addText = text;
        }

        public void HideText()
        {
            _textHide = true;

            if(_textValue)
                _textValue.enabled = false;
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

            if(_textValue)
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
            }

        }

        private IEnumerator sliderIncreaseAnimation(int end, float time, UnityAction finish = null)
        {

            float add = end - _currentValue;
            float offset = (add / time); // 시간을 기준으로한  분배 ( Timedelta)        

            //Debug.Log("Slide Value : add , " + add + " , offset : " + offset + ", current : " + _currentValue);

            Value = _currentValue;

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
            //ExpValue = 0;
            if(gameObject.activeSelf == false)
                ShowBaseUI();

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(sliderAnimation(dest, time, loopCount, finish, upcallBack));
        }


        public virtual void DecreaseGraphic(int dest, float time, bool subAni = false, UnityAction finish = null)
        {
            //ExpValue = 0;
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            if (subAni)
            {
                if (gameObject.activeSelf == false)
                    return;

                _aniEndFunc = finish;
                _coroutine = StartCoroutine(sliderDecreaseAnimation(dest, time, OnAniComplete));
            }
            else
            {
                if (gameObject.activeSelf == false)
                    return;

                _coroutine = StartCoroutine(sliderDecreaseAnimation(dest, time, finish));
            }
        }


        private void OnAniComplete()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            if(gameObject.activeSelf)
                _coroutine = StartCoroutine(SubDecreaseGraph());
        }




        private IEnumerator sliderAnimation(int end, float time, int loopCount, UnityAction finish = null, UnityAction upCallback = null)
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

        public override void HideBaseUI()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            base.HideBaseUI();            
        }


        void OnDisable()
        {
            if (_fillSprite == null)
            {
                _fillSprite = GetImage("Img_Fill");
                _expImage = GetImage("Img_Exp");
                _textValue = GetText("Text_Value");
            }
        }


        /*void OnGUI()
        {
            if (GUI.Button(new Rect(100, 0, 100, 100), "Init"))
            {
                IsPercent = true;
                MaxValue = 100;
                Value = 0;
                EndText("%");

            }        

            if (GUI.Button(new Rect(100, 200, 100, 100), "LoopAni"))
            {   
                IncreaseGraphic(5, 3, null, Test, 3);
            }

            if (GUI.Button(new Rect(200, 200, 100, 100), "NexLevel"))
            {
                IncreaseGraphic(10, 2, null, Test, 3);
            }

            if (GUI.Button(new Rect(300, 200, 100, 100), "ExpUp"))
            {
                ExpValue = 10;
                
            }
        }

        int index = 10;

        void Test()
        {
            MaxValue = index * 10;
            index++;

            print(MaxValue);
        }*/
    }
}


