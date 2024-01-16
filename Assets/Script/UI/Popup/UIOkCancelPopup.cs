using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;


namespace UIContent
{
    public class UIOkCancelPopup : UIPopupBase
    {

        private UnityAction _okCallback;
        private UnityAction _noCallback;
        private Text _okText;
        private Text _cancelText;
        private Text _infoText;
        private Text _titleText;

        void Awake()
        {
            IsModalBlack = true;

            _okText = GetText("Text_Ok");
            _cancelText = GetText("Text_Cancel");
            _infoText = GetText("Text_Info");
            _titleText = GetText("Text_Title");

            RegistAllButtonOnClickEvent();
        }

        public void SetUIData(string title, string infoText, UnityAction yes = null, UnityAction no = null)
        {
            _infoText.text = infoText;
            _titleText.text = title;
            _okCallback = yes;
            _noCallback = no;
        }

        public void ForceClose()
        {
            if (_noCallback != null)
            {
                _noCallback.Invoke();
                _okCallback = null;
                _noCallback = null;
            }

            UIManager.i.RemovePopup<UIOkCancelPopup>();
        }


        protected override void OnButtonClick(string name)
        {
            base.OnButtonClick(name);

            if (name == "Btn_Ok")
            {
                if (_okCallback != null)
                {
                    _okCallback.Invoke();
                    _okCallback = null;
                    _noCallback = null;
                }


                UIManager.i.RemovePopup<UIOkCancelPopup>();
            }

            if (name == "Btn_Cancel")
            {
                if (_noCallback != null)
                {
                    _noCallback.Invoke();
                    _okCallback = null;
                    _noCallback = null;
                }

                UIManager.i.RemovePopup<UIOkCancelPopup>();
            }
        }
    }
}
