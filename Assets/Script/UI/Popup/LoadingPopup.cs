using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIContent
{

    public class LoadingPopup : UIPopupBase
    {
        private Image _backImage;
        private Text _progress;

        private const string percent = "%";
        private const string imageNamte = "Img_Back";
        private const string textNamte = "Text_Progress";
        private const string zero = "0";

        void Awake ()
        {
            _backImage = GetImage(imageNamte);
            _progress = GetText(textNamte);
        }

        void Start()
        {
            _progress.text = GlobalUtil.AddString(zero, percent);
        }
        
        public void SetUIData(int progress)
        {
            _progress.text = GlobalUtil.AddString(progress.ToString() , percent);
        }

        public override void ResetUIUpdate(UIData data)
        {
            SetUIData((data as LoadingPopupData).Number);
        }



    }

}
