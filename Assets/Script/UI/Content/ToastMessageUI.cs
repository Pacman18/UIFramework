using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using uTools;

namespace UIContent
{
    public class ToastMessageUI : UIBase
    {
        private Text _message;
        private uTweenAlpha _alpha;

        void Awake ()
        {
            _message = GetComponentInChildren<Text>();
            _alpha = GetComponentInChildren<uTweenAlpha>(true);

            UnityEvent endEvent2 = new UnityEvent();
            endEvent2.AddListener(() =>
            {                
                UIManager.i.RemoveFrontUI(this);            
            });
            _alpha.SetOnFinished(endEvent2);

            //SoundSystem.i.PlayShot("GameUIErError");
        }

        public void SetUIData(ToastUIData data)
        {
            _message.text = data.Message;
        }	
    }

    public class ToastUIData
    {
        public string Message;
        public Object ExtraData;

    }
}
