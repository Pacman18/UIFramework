using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UIContent
{ 
    public class UITestStackPage : UIPopupBase
    {
        private TMPro.TextMeshProUGUI _titleText;


        void Awake()
        {
            RegistAllButtonOnClickEvent();
            _titleText = GetText("Text_Title", true);
        }

       
        public override void SetUIData(UIData data)
        {
            var uidata = data as UITestStackPageData;

            _titleText.text = uidata.TitleText;
        }

        private void OnLeftBtnClick()
        {
            var uistatckdata = new UITestStackPageData();
            uistatckdata.TitleText = "���� ���� ������Ʈ";
            UIManager.i.RefreshUI(uistatckdata);
        }

        private void OnRightBtnClick()
        {
            var uidata = new UITestPageData();
            uidata.TitleText = "���� ���� �˾� ������ ������Ʈ";
            UIManager.i.SetUIData(uidata);
        }

        private void OnEndBtnClick()
        {
            UIManager.i.RemoveCurrentTopStackPopup();
        }

        private void OnBtnClick_One()
        {
            UIManager.i.FadeOutIn();
        }
        private void OnBtnClick_Two()
        {
            UIManager.i.RemoveCurrentPageUI();
        }
        private void OnBtnClick_Three()
        {
            UIManager.i.Toast("�̰��� �佺Ʈ �޼�����");
        }

        private void OnBtnClick_Four()
        {
            UIManager.i.CreatePopupUI<TestUIPopup>();
        }

        protected override void OnButtonClick(string name)
        {
            base.OnButtonClick(name);

            switch (name)
            {
                case "Btn_1":
                    OnBtnClick_One();
                    break;
                case "Btn_2":
                    OnBtnClick_Two();
                    break;
                case "Btn_3":
                    OnBtnClick_Three();
                    break;
                case "Btn_4":
                    OnBtnClick_Four();
                    break;
                case "Btn_Left":
                    OnLeftBtnClick();
                    break;
                case "Btn_Right":
                    OnRightBtnClick();
                    break;
                case "Btn_End":
                    OnEndBtnClick();
                    break;
            }
        }
    }

    public class UITestStackPageData : UIData
    {
        public string TitleText;

        public override void OpenInitData()
        {
            TitleText = "�ʱ�ȭ ���� ���� ";
        }
    }
}
