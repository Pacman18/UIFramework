using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;

namespace UIContent
{
    public class UITabController : UIBase
    {

        private Button[] _buttons;
        private Text[] _btnText;
        //private int _currentIndex = 0;    

        void Awake()
        {
            _buttons = RegistAllButtonOnClickEvent();
            _btnText = GetComponentsInChildren<Text>();       

            //Initialize(2); // Sample Code
        }

        /// <summary>
        /// 버튼 세팅이 끝나면 최초 어디부터 시작할껀지 시작점코드부터 진행 ( 시작탭 인덱스를 넣어주면된다 )
        /// Action은 해당 구현 함수를 넣어줘야한다 ( 사운드 중첩때문에 이렇게 함 )
        /// </summary>
        public void Initialize(int index , UnityAction action)
        {
            CheckButtonIndex(index);
            if(action != null)
                action.Invoke();
            //_buttons[index].onClick.Invoke();           
        }

        public void AddListener(int index, UnityAction callback)
        {
            _buttons[index].onClick.AddListener(callback);
        }

        public void RemoveListener(int index, UnityAction callback)
        {
            _buttons[index].onClick.RemoveListener(callback);
        }

        // 단순히 이미지만 바꾸는 함수다  이벤트는 이미 등록해서 자동적으로 버튼 누르면 실행됨
        public void CheckButtonIndex(int index)
        {
            //_currentIndex = index;

            for(int i =0; i< _buttons.Length; i++)
            {
                _buttons[i].interactable = true;
                _btnText[i].color = Color.white;
            }           
            
            _buttons[index].interactable = false;
            _btnText[index].color = Color.gray;
        }

        protected override void OnButtonClick(string name)
        {
            base.OnButtonClick(name);
            //SoundSystem.i.PlayShot(48);

            for (int i = 0; i < _buttons.Length; i++)
            {
                if (_buttons[i].name == name)
                {
                    CheckButtonIndex(i);
                }
            }
        }


    }

}

