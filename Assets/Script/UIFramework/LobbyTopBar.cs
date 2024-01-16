using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using uTools;

namespace UIContent
{

    public class LobbyTopBar : UIPopupBase 
    {
        private Text _coin;
        private Text _cash;
        private Text _flower;
        private UITabController _tapController;


        void Awake()
        {        
            _coin = GetText(GlobalUtil.TextMoney);
            _cash = GetText(GlobalUtil.TextCash);
            _flower = GetText(GlobalUtil.TextFlower);

            _tapController = GetComponentInChildren<UITabController>();
        }

        void Start()
        {

            _tapController.AddListener(0, ChangeMyDeck);
            _tapController.AddListener(1, ChangeShop);
            _tapController.AddListener(2, ChangeTrade);
            _tapController.AddListener(3, ChangeMainLobby);


            _tapController.Initialize(3, ChangeMainLobby);
        }

        private void ChangeMainLobby()
        {
            UIManager.i.PrevPopup();
            //UIManager.i.CreateStackPopUp<MainLobbyPopup>();
        }

        private void ChangeShop()
        {
            //UIManager.i.PopupNotice("준비중입니다.");
        }

        private void ChangeTrade()
        {
            //UIManager.i.PopupNotice("준비중입니다.");
        }

        private void ChangeMyDeck()
        {
            UIManager.i.PrevPopup();
            //UIManager.i.CreateStackPopUp<LobbyAlbumPopup>();
        }

    }

}
