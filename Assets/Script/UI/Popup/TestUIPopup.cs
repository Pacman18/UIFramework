using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIContent
{

    public class TestUIPopup : UIPopupBase
    {
        UIFillGauge[] _gauges;

        void Awake()
        {
            RegistAllButtonOnClickEvent();

            _gauges = GetComponentsInChildren<UIFillGauge>();

            _gauges[0].MaxValue = 100;
            _gauges[1].MaxValue = 100;
            _gauges[1].ExpValue = 100;
        }        

        public override void SetUIData(UIData data)
        {
         
            
        }

        private void OnLevelUp()
        {
            UIManager.i.Toast("레벨업!!");
        }

        private void OnLevelUpComplete()
        {
            var ui = UIManager.i.CreatePopupUI<UINoticePopup>();
            ui.SetUIData("레벨업 완료");
        }

        protected override void OnButtonClick(string name)
        {
            switch(name)
            {
                case "Btn_Level":
                    _gauges[0].IncreaseGraphic(30, 5, OnLevelUpComplete, OnLevelUp, 5);
                    break;
                case "Btn_Action":
                    _gauges[1].DecreaseGraphic(50, 1, true);
                    break;
                case "Btn_Root":
                    break;
                case "Btn_GrandFather":
                    RedDotProxy.Instance.SetCount( RedDotProxy.RedDotKey.GrandFather, 0);
                    break;
                case "Btn_Father":
                    RedDotProxy.Instance.SetCount(RedDotProxy.RedDotKey.Father, 0);
                    break;
                case "Btn_Brother":
                    RedDotProxy.Instance.SetCount(RedDotProxy.RedDotKey.Brother, 0);
                    break;
                case "Btn_GrandMother":
                    RedDotProxy.Instance.SetCount(RedDotProxy.RedDotKey.GrandMother, 0);
                    break;
                case "Btn_Mother":
                    RedDotProxy.Instance.SetCount(RedDotProxy.RedDotKey.Mother, 0);
                    break;
                case "Btn_RedRan": // 레드닷 랜덤 생성
                    RandomRedCount();
                    break;
                case "Btn_Exit":
                    UIManager.i.RemovePopup(this);
                    break;
            }
        }

        private void RandomRedCount()
        {
            int randomCount = Random.Range(0, 30);
            RedDotProxy.Instance.SetCount( RedDotProxy.RedDotKey.Brother, randomCount, RedDotProxy.RedDotKey.GrandFather);

            randomCount = Random.Range(0, 30);
            RedDotProxy.Instance.SetCount(RedDotProxy.RedDotKey.Father, randomCount, RedDotProxy.RedDotKey.GrandFather);

            randomCount = Random.Range(0, 30);
            RedDotProxy.Instance.SetCount(RedDotProxy.RedDotKey.GrandFather, randomCount, RedDotProxy.RedDotKey.Family);

            randomCount = Random.Range(0, 30);
            RedDotProxy.Instance.SetCount(RedDotProxy.RedDotKey.Mother, randomCount, RedDotProxy.RedDotKey.GrandMother);

            randomCount = Random.Range(0, 30);
            RedDotProxy.Instance.SetCount(RedDotProxy.RedDotKey.GrandMother, randomCount, RedDotProxy.RedDotKey.Family);

        }

    }

    public class TestUIPopupData : UIData
    {
        public override void OpenInitData()
        {

        }
    }
}


