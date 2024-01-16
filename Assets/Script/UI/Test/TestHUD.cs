using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIContent
{
    public class TestHUD : UIBase
    {
        private GameObject _dummy_1 = null;
        private GameObject _dummy_2 = null;

        private void Awake()
        {
            RegistAllButtonOnClickEvent();
        }

        protected override void OnButtonClick(string name)
        {
            base.OnButtonClick(name);

            switch(name)
            {
                // 
                case "Button_1":
                    OnButtonClick_CreateObject();
                    break;
                case "Button_2":
                    OnButtonClick_AttatchUI();
                    break;
                case "Button_3":
                    OnButtonClick_OpenDialog();
                    break;
                case "Button_4":
                    OnButtonClick_OpenPage();
                    break;
            }
        }

        // 3D 더미 만들기
        private void OnButtonClick_CreateObject()
        {
            _dummy_1 = Instantiate(Resources.Load<GameObject>("Prefab/Test/TestUICapsule_1"));
            _dummy_2 = Instantiate(Resources.Load<GameObject>("Prefab/Test/TestUICapsule_2"));
        }

        // 3D 오브젝트에  UI 붙이기 
        private void OnButtonClick_AttatchUI()
        {
            if(_dummy_1 == null)
            {
                UIManager.i.Toast("더미를 먼저 만드세요.");
                return;
            }

            var gameUI = UIManager.i.CreateGameUI<TestInGameUI>();
            gameUI.SetTarget(_dummy_1.transform);
            gameUI = UIManager.i.CreateGameUI<TestInGameUI>();
            gameUI.SetTarget(_dummy_2.transform);
        }

        // 대화창  다이얼로그 팝업레이어에 열기 
        private void OnButtonClick_OpenDialog()
        {
            var  ui = UIManager.i.CreatePopupUI<DialogBox>();
            ui.SetUIData(0, "데모 다이얼 로그 박스~~~~ ");
            ui.ShowText();

            // 테이블과 연동해서 사용
            //ui.ShowByTableIndex(1, 5);
        }

        // 페이지 팝업 생성 
        private void OnButtonClick_OpenPage()
        {
            UIManager.i.CreatePagePopUp<UITestPage>();
        }

    }
}
