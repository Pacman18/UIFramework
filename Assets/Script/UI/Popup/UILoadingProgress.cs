using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UIContent
{
    public class UILoadingProgress : UIPopupBase
    {
        //private RectTransform _loadImg;
        private const string ImageName = "Image";
        private TMPro.TextMeshProUGUI _countText;

        void Awake()
        {
            _countText = GetTMPText("Text_Count");
            IsModalBlack = true;
        }

        public override void SetUIData(UIData data)
        {
            base.SetUIData(data);

            var uidata = data as UILoadingProgressData;

            _countText.text = uidata.WaitingCount.ToString();
        }
    }

    public class UILoadingProgressData : UIData
    {
        public int WaitingCount;

        public void AddCount()
        {
            WaitingCount++;
        }

        public void ReduceCount()
        {
            WaitingCount--;

            if (WaitingCount < 0)
                WaitingCount = 0;
        }

        public override void OpenInitData()
        {
            WaitingCount = 1;
        }
    }
}

