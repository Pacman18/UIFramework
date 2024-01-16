using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class UINoticePopup : UIPopupBase
{



    private Text _title;
    private Text _information;

    public UnityAction Callback;

    void Awake()
    {
        IsModalBlack = true;        

        _title = GetText("Text_title");
        _information = GetText("Text_info");

        RegistAllButtonOnClickEvent();

        //SoundSystem.i.PlayShot("GameNotice");
    }
    

    public void SetUIData(string info , string title = "알림")
    {
        _title.text = title;
        _information.text = info;
    }

    protected override void OnButtonClick(string name)
    {
        base.OnButtonClick(name);

        if(name == GlobalUtil.BtnConfirm)
        {
            if (Callback != null)
                Callback.Invoke();

            UIManager.i.RemovePopup<UINoticePopup>();
        }

        if(name == GlobalUtil.BtnExit)
            UIManager.i.RemovePopup<UINoticePopup>();
    }

}
