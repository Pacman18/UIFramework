using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIContent
{
    public class UITestPage : UIPopupBase
    {
        private Button _leftButton;
        private Button _rightButton;
        private TMPro.TextMeshProUGUI _titleText;

        void Awake()
        {
            RegistAllButtonOnClickEvent();
            _titleText = GetTMPText("Text_Title");
        }        

        public override void SetUIData(UIData data)
        {
            var uidata = data as UITestPageData;

            _titleText.text = uidata.TitleText;
        }

        protected override void OnButtonClick(string name)
        {
            base.OnButtonClick(name);

            switch(name)
            {
                case "Btn_1":
                    UIManager.i.CreateStackPopUp<UITestStackPage>();
                    break;
                case "Btn_2":
                    var popup = UIManager.i.CreatePopupUI<UINoticePopup>();
                    popup.SetUIData("이것은 팝업");
                    break;
                case "Btn_3":   
                    var uidata = new UITestPageData();
                    uidata.TitleText = "제목 업데이트";
                    UIManager.i.RefreshUI(uidata);
                    break;
                case "Btn_Exit":
                    UIManager.i.RemoveCurrentPageUI();
                    break;
            }
        }        
    }

    public class UITestPageData : UIData
    {
        public string TitleText;

        public override void OpenInitData()
        {
            TitleText = "초기화 페이지 제목 ";
        }
    }

}


