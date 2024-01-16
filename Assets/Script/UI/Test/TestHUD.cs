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

        // 3D ���� �����
        private void OnButtonClick_CreateObject()
        {
            _dummy_1 = Instantiate(Resources.Load<GameObject>("Prefab/Test/TestUICapsule_1"));
            _dummy_2 = Instantiate(Resources.Load<GameObject>("Prefab/Test/TestUICapsule_2"));
        }

        // 3D ������Ʈ��  UI ���̱� 
        private void OnButtonClick_AttatchUI()
        {
            if(_dummy_1 == null)
            {
                UIManager.i.Toast("���̸� ���� ���弼��.");
                return;
            }

            var gameUI = UIManager.i.CreateGameUI<TestInGameUI>();
            gameUI.SetTarget(_dummy_1.transform);
            gameUI = UIManager.i.CreateGameUI<TestInGameUI>();
            gameUI.SetTarget(_dummy_2.transform);
        }

        // ��ȭâ  ���̾�α� �˾����̾ ���� 
        private void OnButtonClick_OpenDialog()
        {
            var  ui = UIManager.i.CreatePopupUI<DialogBox>();
            ui.SetUIData(0, "���� ���̾� �α� �ڽ�~~~~ ");
            ui.ShowText();

            // ���̺�� �����ؼ� ���
            //ui.ShowByTableIndex(1, 5);
        }

        // ������ �˾� ���� 
        private void OnButtonClick_OpenPage()
        {
            UIManager.i.CreatePagePopUp<UITestPage>();
        }

    }
}
