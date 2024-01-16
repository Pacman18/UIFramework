using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIContent;




namespace UIContent
{
    public enum UISTATE { None }

}
public abstract class UIData
{
    public UISTATE State;

    
    //Popup 등 사용하는 UI가 새로 만들어질때 반드시 한번 호출되는 함수 즉 UI초기화 세팅이라고 보면된다. 
    // 기존에 UI를 열었다 닫았어도 다시 새로 열때 호출되므로 리셋용으로 사용될 함수다.
    // 그러니까 인벤토리나 다른데서 값을 가져와서 기본세팅하는 용도로 사용할 필요성이 있을듯. 
    abstract public void OpenInitData();

}
