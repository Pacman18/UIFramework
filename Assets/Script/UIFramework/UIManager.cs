using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UIContent;
using System;

public class UIManager : MonoBehaviour
{
    public Camera PopupCamera;

    private UIDataManager _datamanager = new UIDataManager();


    public UIPopupBase CurrentPageBase;

    private const string _hudPath = "Prefab/UI/HUD/";
    private const string _UINamespace = "UIContent.";
    private const string _GameUIPath = "Prefab/UI/GameUI/";
    private const string STACK = "_Stack";
    private const string STACK_PREFAB = "STACK";

    private static bool _applicationQuit = false;

    private bool _waitingToast = false;
    private Queue<ToastUIData> _toastQueueUI = new Queue<ToastUIData>();

    /// <summary>
    /// UI Depth 관리를 위한 컨테이너  , Key 값은 BasePopup , Value 값은 Base 위에 올라가는 중,소 Popup 
    /// </summary>
    private Dictionary<UIPopupBase, List<UIPopupBase>> _pagePopupList = new Dictionary<UIPopupBase, List<UIPopupBase>>();
    private Dictionary<string, RectTransform> _stackCanvasList = new Dictionary<string, RectTransform>();
    private List<UIPopupBase> _popupList = new List<UIPopupBase>();   // 전체를 가리는 팝업단 관리 ( 판매완료 , 업그레이드 완료 정보등 )

    // 3D Uniq UI 인덱스 관리용 
    private int _uniqIndex = 0;

    // GameUI Uniq UI 관리용 인덱스
    private int _gameUniqIndex = 0;

    // 3D 오브젝트 UI 관리  
    private List<UIBase> _ownerUIList = new List<UIBase>();

    // InGame 안의 UI 관리 ( 3D 오브젝트 관련 아님 )
    private List<UIBase> _inGameUIList = new List<UIBase>();

    private List<UIBase> _blockWorldList = new List<UIBase>(); // Touch를 막을 UI들

    private List<UIPopupBase> _poolList = new List<UIPopupBase>();

    // 풀리스트 가져오기
    public T GetPoolUI<T>() where T : UIPopupBase
    {
        T ui = _poolList.Find((c) => c is T) as T;

        if(ui != null)
        {
            ui.gameObject.SetActive(true);
            _poolList.Remove(ui);
        }

        return ui;
    }
    
    // 풀리스트에 저장
    public void AddPagePool(UIPopupBase popup) 
    {
        if (_poolList.Contains(popup) == false)
        {
            popup.gameObject.SetActive(false);
            popup.transform.SetParent(Pool, false);            

            _poolList.Add(popup);
        }

        if(_poolList.Count > 5 )
        {
            var remo = _poolList[0].gameObject;
            _poolList.RemoveAt(0);
            Destroy(remo);
        }
    }

    // 풀안에있는것 지워버린다
    public void DestroyPool()
    {
        for(int i =0; i < _poolList.Count; i++)
        {
            var temp = _poolList[0];
            _poolList.RemoveAt(0);
            Destroy(temp.gameObject);
        }
    }

    public int PopCount
    {
        get { return _popupList.Count;}
    }

    public int StatckCount
    {
        get
        {
            if (CurrentPageBase != null)
                return _pagePopupList[CurrentPageBase].Count;
            else
                return 0;
        }
    }

    public bool IsAnyPopup
    {
        get
        {
            if(CurrentPageBase != null)
                return true;

            if (_popupList.Count > 0)
                return true;

            if (_blockWorldList.Count > 0)
                return true;

            return false;
        }
    }

    public bool ISBlock
    {
        get { return _blockWorldList.Count > 0; }
    }


    public Transform WORLD_LAYER
    {
        get { return WorldUI; }
    }

    // HUD 생길때는 따로 해줘야한다
    public void EventOff_WorldLayer(bool onOff)
    {
        WorldUI.gameObject.SetActive(onOff);
    }

    private Transform Pool;

    private Transform WorldUI;
    private Transform PageUI;
    private Transform PopUpUI;
    private Transform FrontUI;
    private Transform HudUI;

    private TouchPanel _touchPanel;

    // 터치 패드 관련 
    public TouchPanel TouchPad
    {
        get
        {
            if (_touchPanel == null)
            {
                _touchPanel = GetComponentInChildren<TouchPanel>();
            }                

            return _touchPanel;
        }
    }

    private HUD _hudManager;

    public HUD Hud
    {
        get { return _hudManager; }
    }

    public RectTransform CanvasRect
    {
        get { return _canvasRect; }
    }

    private RectTransform _canvasRect;

    
    public Canvas UICanvas
    {
        get
        {
            if(_uicanvas == null)
                _uicanvas = GetComponent<Canvas>();

            return _uicanvas;
        }
    }

    private Canvas _uicanvas;    

    public CanvasScaler UICanvasScaler
    {
        get { return _uiScaler; }
    }
    private CanvasScaler _uiScaler;

    private static UIManager _instance;

    public static UIManager i
    {
        get
        {
            if (_applicationQuit)
                return null;

            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }

            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();                

                UIManager managerScript = Instantiate(Resources.Load<GameObject>("Prefab/UI/UIManager")).GetComponent<UIManager>();
                _instance = managerScript;

                managerScript.CreateEventSystem();
            }

            return _instance;
        }
    }



    void Awake()
    {
        _applicationQuit = false;

        _uicanvas = GetComponent<Canvas>();
        _uiScaler = GetComponent<CanvasScaler>();

        _canvasRect = _uicanvas.GetComponent<RectTransform>();       

        CreateEventSystem();

        this.name = this.name.Replace("(Clone)", "");

        WorldUI = transform.Find("WORLD_LAYER");
        PageUI = transform.Find("PAGE");
        PopUpUI = transform.Find("POPUP");
        FrontUI = transform.Find("FRONT");
        Pool = transform.Find("POOL");

        DontDestroyOnLoad(gameObject);
    }



    /// <summary>
    /// 이벤트시스템이 없는 씬이 있는경우 입력값을 가져올수 없다 
    /// UI 매니져가 DontDestory인경우 씬이 바뀔때마다 확인해줘야한다 
    /// 향후 UI매니져로 변경시  이벤트시스템 확인후 만들어주는 부분이 필요 혹은 Test씬에서 작동용
    /// </summary>
    public void CreateEventSystem()
    {
        EventSystem evtstm = FindObjectOfType<EventSystem>();

        if (evtstm != null)
            return;

        GameObject go = new GameObject("EventSystem");
        evtstm = go.AddComponent<EventSystem>();
        go.AddComponent<StandaloneInputModule>();
        go.transform.SetParent(transform);        
    }

    public void CreateHUD()
    {
        if (_hudManager != null)
            return;

        HUD hudManager = Instantiate(Resources.Load<GameObject>("Prefab/UI/HUD"),transform).GetComponent<HUD>();
        _hudManager = hudManager;

        _hudManager.transform.SetSiblingIndex(2);
        _hudManager.transform.localPosition = _hudManager.transform.localPosition + Vector3.forward * 800;

        HudUI = _hudManager.transform;
    }

    public void RemoveHUD()
    {
        if(_hudManager)
        {
            _hudManager.Clear();
            Destroy(_hudManager.gameObject);
        }

        _hudManager = null;
    }

    public T CreateHUDUI<T>() where T : UIBase
    {
        return CreateHUDUI<T>(typeof(T).ToString());
    }

    public T CreateHUDUI<T>(string resName) where T : UIBase
    {
        int index = resName.IndexOf(".");
        string realPath = resName;

        if(index > -1)
        {
            realPath = resName.Substring(index + 1);
        }

        string path = GlobalUtil.AddString(_hudPath, realPath);

        //Debug.Log(path);
        GameObject res = Resources.Load<GameObject>(path);        
        
        GameObject obj = Instantiate(res);

        if(_hudManager == null)
            CreateHUD();

        obj.transform.SetParent(HudUI, false);
        T callUI = obj.GetComponent<T>();        

        if(callUI)
            _hudManager.AddHUDUI(callUI);

        return callUI;
    }

    public T CreateHUDUI<T>(int tableIndex) where T : UIBase
    {
        GameObject res = Resources.Load<GameObject>("");
        GameObject obj = Instantiate(res);

        if (_hudManager == null)
            CreateHUD();

        obj.transform.SetParent(HudUI, false);
        T callUI = obj.GetComponent<T>();

        if (callUI)
            _hudManager.AddHUDUI(callUI);

        return callUI;
    }

    public void RemoveHUDUI<T>() where T : UIBase
    {
        List<UIBase> HudList = _hudManager.HUDList;

        UIBase targetUI = HudList.Find((x) => x is T);

        if (targetUI)
        {
            _hudManager.RemoveHUDUI(targetUI);
            
        }
    }

    public void RemoveHUDUI(UIBase targetUI)
    {
        if (targetUI == null)
            return;

        List<UIBase> HudList = _hudManager.HUDList;

        _hudManager.RemoveHUDUI(targetUI);
    }

    public T CreateWorldUI<T>(Transform target, float offset = 0) where T : UIBase
    {
        GameObject res = Resources.Load<GameObject>(GlobalUtil.AddString("Prefab/UI/3D_UI/", typeof(T).ToString()));
        
        GameObject obj = Instantiate(res);

        obj.transform.SetParent(WorldUI , false);
        T callUI = obj.GetComponent<T>();

        _uniqIndex++;

        // 3D 오브젝트 관리용 Id 를 넣어준다
        callUI.Id = _uniqIndex;        

        if(callUI.Block)
        {
            _blockWorldList.Add(callUI);
            CheckPossibleWorldTouch();
        }

        _ownerUIList.Add(callUI);

        return callUI;
    }

    public void RemoveWorldGameUI(UIBase uiCs)
    {
        if (uiCs == null)
            return;

        UIBase targetUI = _ownerUIList.Find((x) => x.Id == uiCs.Id);

        if (targetUI)
        {

            _ownerUIList.Remove(targetUI);

            if (targetUI.Block)
                _blockWorldList.Remove(targetUI);

            Destroy(targetUI.gameObject);

            CheckPossibleWorldTouch();
        }
    }

    public bool RemoveWorldGameUI<T>() where T : UIBase
    {
        UIBase targetUI = _ownerUIList.Find((x) => x is T) as T;
        
        if (targetUI == null)
            return false;
        else
            _ownerUIList.Remove(targetUI);

        if (targetUI.Block)
            _blockWorldList.Remove(targetUI);

        Destroy(targetUI.gameObject);

        CheckPossibleWorldTouch();

        return true;
    }

    public T GetUIResource<T>() where T : UIBase
    {
        var compText = typeof(T).ToString().Replace(_UINamespace, string.Empty);

        return Resources.Load<T>(GlobalUtil.AddString(_GameUIPath, compText));        
    }


    public T CreateGameUI<T>() where T : UIBase
    {
        var compText = typeof(T).ToString().Replace(_UINamespace, string.Empty);

        GameObject res = Resources.Load<GameObject>(GlobalUtil.AddString(_GameUIPath, compText));

        GameObject obj = Instantiate(res);

        obj.transform.SetParent(WorldUI, false);
        T callUI = obj.GetComponent<T>();

        _inGameUIList.Add(callUI);

        _gameUniqIndex++;

        callUI.Id = _gameUniqIndex;

        if (callUI.Block)
        {
            _blockWorldList.Add(callUI);
            CheckPossibleWorldTouch();
        }

        return callUI;
    }

     public T CreateFrontUI<T>() where T : UIBase
    {
        var compText = typeof(T).ToString().Replace(_UINamespace, string.Empty);

        GameObject res = Resources.Load<GameObject>(GlobalUtil.AddString(_GameUIPath, compText));

        GameObject obj = Instantiate(res);        
        
        obj.transform.SetParent(FrontUI, false);
        T callUI = obj.GetComponent<T>();

        return callUI;
    }

    public T GetUIData<T>() where T : UIData
    {   
        return _datamanager.GetUIData<T>();
    }

    public UIData GetUIData(UIPopupBase popup)
    {
        return _datamanager.GetData(popup.GetType());
    }


    public T GetGameUI<T>() where T : UIBase
    {
        return _inGameUIList.Find((x) => x is T) as T;
    }

    public T GetWorldUI<T>(int index) where T : UIBase
    {
        return _ownerUIList.Find((x) => x.Id == index) as T;
    }

    public T GetUIDWorldUI<T>(long UID) where T : UIBase
    {
        return _ownerUIList.Find((x) => x.UID == UID) as T;
    }

    public T GetHUDUI<T>() where T : UIBase
    {
        if(_hudManager == null)
            _hudManager = FindObjectOfType<HUD>();  

        return _hudManager.GetHUDUI<T>();
    }

    public bool RemoveGameUI<T>()  where T : UIBase
    {
        UIBase targetUI = _inGameUIList.Find((x) => x is T );

        if (targetUI == null)
            return false;
        else
            _inGameUIList.Remove(targetUI);

        if (targetUI.Block)
            _blockWorldList.Remove(targetUI);

        Destroy(targetUI.gameObject);

        CheckPossibleWorldTouch();

        return true;
    }

    public void RemoveGameUI(UIBase ui)
    {
        UIBase targetUI = _inGameUIList.Find((x) => x.Id == ui.Id);

        if (targetUI != null)
        {
            _inGameUIList.Remove(targetUI);

            if (targetUI.Block)
                _blockWorldList.Remove(targetUI);

            if(_waitingToast)
                _waitingToast = targetUI is ToastMessageUI;

            Destroy(targetUI.gameObject);
        }

        CheckPossibleWorldTouch();
    }

    public void RemoveFrontUI(UIBase ui)
    {
        if(_waitingToast)
            _waitingToast = !(ui is ToastMessageUI);

        Destroy(ui.gameObject);
    }

    /// <summary>
    /// 가장 바닥에 깔리는 페이지 팝업 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data">Null 을 넣고하면 UIData의 InitData가 호출된 값으로 세팅될것 , Data를 넣으면 SetUIData 함수가 호출된느 부분으로 세팅됨 </param>
    /// <param name="deletePrev">이전 페이지가 있는것을 지울것인지</param>
    /// <returns></returns>
    public T CreatePagePopUp<T>(UIData data = null, bool deletePrev = true) where T : UIPopupBase
    {
        return CreatePopupUI<T>( data, UIPopupBase.PopupType.PAGE , deletePrev);
    }

    public T CreatePagePopUp<T>(string folder, UIData data = null, bool deletePrev = true) where T : UIPopupBase
    {
        return CreatePopupUI<T>(folder, data, UIPopupBase.PopupType.PAGE, deletePrev);
    }

    /// <summary>
    /// Page 위에 올라갈 스택 팝업
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="preHide">이전 페이지나 이전 스택팝업을 Hide시킬지 여부</param>
    /// <returns></returns>
    public T CreateStackPopUp<T>(UIData data = null, bool preHide = false) where T : UIPopupBase
    {
        return CreatePopupUI<T>( data, UIPopupBase.PopupType.SUB_STACK, preHide);
    }

    public T CreateStackPopUp<T>(string folder, UIData data = null, bool preHide = true) where T : UIPopupBase
    {
        return CreatePopupUI<T>(folder, data, UIPopupBase.PopupType.SUB_STACK, preHide);
    }

    public T CreateTopBar<T>(UIPopupBase.PopupType type = UIPopupBase.PopupType.TOP_BAR, bool preOption = false) where T : UIPopupBase
    {
        return CreatePopupUI<T>(null, type, false);
    }

    /// <summary>
    /// 타입 이름가지고 팝업 생성함수 
    /// </summary>
    public T CreatePopupUI<T>(UIData data = null, UIPopupBase.PopupType type = UIPopupBase.PopupType.POP_UP,  bool preOption = false) where T : UIPopupBase
    {
        return CreatePopupUI<T>(typeof(T).ToString(), data, type, preOption);
    }

    private T CreatePopupUI<T>(string folder, UIData data = null, UIPopupBase.PopupType type = UIPopupBase.PopupType.POP_UP, bool preOption = false) where T : UIPopupBase
    {
        T bundlePopup = null;

        bundlePopup = GetPoolUI<T>();

        if(bundlePopup == null)
        {
            string popname = typeof(T).ToString();
            int startIndex = popname.IndexOf('.');

            string loadpopname;
            if (startIndex < 0)
                loadpopname = popname;
            else
                loadpopname = popname.Substring(startIndex + 1 );            

            GameObject res = Resources.Load<GameObject>(GlobalUtil.AddString("Prefab/UI/Popup/", loadpopname));

            if (res == null)
            {
                Debug.Log("Look for Other Resources Path : " + loadpopname);
                return null;
            }

            GameObject obj = Instantiate(res);

            bundlePopup = obj.GetComponent<T>();
            bundlePopup.name = loadpopname;
        }

        if (bundlePopup == null)
            Debug.Log("Plz Check UIPrefab Attatch Script UIPopupBase");

        selectParentBelongtoUI(bundlePopup, type, preOption);

        if(data == null)
        {
            data = _datamanager.GetData(typeof(T));

            if(data == null)
               data = _datamanager.AddData<T>();
            else            
            {
                data.OpenInitData();
                _datamanager.UpdateData<T>(data);            
            }
        }
        else
        {
            data.OpenInitData();
            _datamanager.UpdateData<T>(data);            
        }

        bundlePopup.SetUIData(data);

        return bundlePopup as T;
    }


    /// <summary>
    /// PopUp 하이어라키에 있는것중 삭제
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void RemovePopup<T>() where T :UIPopupBase
    {
        T popup = _popupList.Find( (pop)=> pop is T) as T;

        if (popup == null)
        {
            print("Pop is Null" + typeof(T));
            return;
        }            

        _popupList.Remove(popup);        

        Destroy(popup.gameObject);
    }

    /// <summary>
    /// 팝업 Id 특정된것만 지운다
    /// </summary>
    /// <param name="popup"></param>
    public void RemovePopup(UIPopupBase popup)
    {
        UIPopupBase find = _popupList.Find((pop) => pop.Id == popup.Id);

        if (popup == null)
        {
            print("Pop is Null" + popup.Id);
            return;
        }

        _popupList.Remove(popup);

        Destroy(popup.gameObject);
    }


    /// <summary>
    ///  팝업 하이어라키 부분의 최상위를 삭제해준다 
    /// </summary>
    /// <returns> 팝업을 지우면 true </returns>
    public bool RemoveTopPopup()
    {
        int count = _popupList.Count;

        if(count > 0)
        {
            UIPopupBase popup = _popupList[count - 1];

            _popupList.Remove(popup);

            Destroy(popup.gameObject);

            // 팝업이 있으면 다음 팝업을 업데이트 시킨다
            if(_popupList.Count > 0)
            {
                popup = _popupList[_popupList.Count - 1];

                var data = _datamanager.GetData(popup.GetType());

                popup.ResetUIUpdate(data);
            }
            else
            {
                // 스택팝업까지만 reUpdate를 진행하자 페이지는 잠시 보류
                if (CurrentPageBase == null)
                    return false;

                List<UIPopupBase> stackList = _pagePopupList[CurrentPageBase];

                if(stackList.Count > 0)
                {
                    popup = stackList[stackList.Count - 1];
                    var data = _datamanager.GetData(popup.GetType());
                    popup.ResetUIUpdate(data);
                }
                else
                {
                    var data = _datamanager.GetData(popup.GetType());
                    CurrentPageBase.ResetUIUpdate(data);
                }
            }

            return true;
        }

        return false;
    }

    // 기본적으로 쌓인 Popup 리스트를 모두 지운다 
    public void ClearPopupList()
    {
        UIPopupBase ui;
        
        var removeList = _popupList.FindAll((x)=> x.PopType != UIPopupBase.PopupType.FADE);
        _popupList.RemoveAll((x) => x.PopType != UIPopupBase.PopupType.FADE);
        
        for (int i =0; i < removeList.Count; i++)
        {
            ui = removeList[i];
            Destroy(ui.gameObject);            
        }

        //QQQ 디버그 확인용 추후 삭제
        for (int i = 0; i < _popupList.Count; i++)
            Debug.Log(_popupList[i].gameObject.name);

        removeList.Clear();
        removeList = null;
    }

    #region Function PopUp

    // Fade In 이후 Out을 바로 실행 
    public void FadeOutIn(UnityAction callback = null, float time = 1.0f)
    {
        UIFadePanel popup;

        popup = FrontUI.GetComponentInChildren<UIFadePanel>();

        if (popup == null)
        {
            popup = CreatePopupUI<UIFadePanel>(  null, UIPopupBase.PopupType.FADE, false);
            popup.FadeOutInDestory(callback,time);
        }
        else
        {
            popup.FadeOutInDestory(callback, time);
        }
    }

    public void StartFadeIn(UnityAction callback = null, float time = 1.0f)
    {
        UIFadePanel popup;
        
        popup = GetUIComponent<UIFadePanel>();

        if(popup == null)
        {
            popup = CreatePopupUI<UIFadePanel>( null, UIPopupBase.PopupType.FADE, false);
            popup.StartFadeIn(time, callback, true);
        }
        else
        {
            popup.StartFadeIn(time, callback, true);
        }
    }

    public void StartFadeOut(UnityAction callback = null, float time = 1.0f)
    {
        UIFadePanel popup;

        popup = GetUIComponent<UIFadePanel>();

        if (popup == null)
        {
            popup = CreatePopupUI<UIFadePanel>(null, UIPopupBase.PopupType.FADE, false);
            popup.StartFadeOut(time, callback, false);
        }
        else
        {
            popup.StartFadeOut(time, callback, false);
        }
    }


    #endregion

    #region Find Component

    /// <summary>
    /// PagePop UI를 찾는 함수 
    /// </summary>
    /// <typeparam name="T"> UIPopup Base 를 상속받은 Class 타입</typeparam>
    /// <returns></returns>
    public T GetPagePopupUI<T>() where T : UIPopupBase
    {
        Dictionary<UIPopupBase, List<UIPopupBase>>.Enumerator iterator = _pagePopupList.GetEnumerator();

        while (iterator.MoveNext())
        {
            List<UIPopupBase> stklist = iterator.Current.Value;

            if (stklist.Find((ui) => ui is T))
                return iterator.Current.Key as T;

            if (iterator.Current.Key is T)
                return iterator.Current.Key as T;
        }

        return null;
    }


    /// <summary>
    /// Page UI 위에 쌓인 스택 팝업을 찾는 함수 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetStakPopComponent<T>() where T : UIPopupBase
    {
        foreach(var pageContain in _pagePopupList)
        {
            List<UIPopupBase> stklist = pageContain.Value;

            if(stklist.Count > 0)
                return stklist.Find((ui) => ui is T) as T;
        }               

        return null;
    }

    /// <summary>
    /// 페이지x 스택x 위에 쌓인 진짜 Popup 리스트에서 찾는다 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetPopComponent<T>() where T : UIPopupBase
    {
        T popui = _popupList.Find((popup) => popup is T) as T;

        if (popui)
            return popui;
        else
            return null;
    } 

    private UIPopupBase GetPopup(Type type)
    {
        UIPopupBase popup = null;

        popup = _popupList.Find(x => x.GetType() == type);

        if (popup != null)
            return popup;

        foreach(var pagePop in _pagePopupList.Keys)
        {
            if (pagePop.GetType() == type)
                return pagePop;
            else
            {
                foreach(var stackPop in _pagePopupList[pagePop])
                {
                    if (stackPop.GetType() == type)
                        return stackPop;
                }
            }
        }

        return null;
    }

    // 전체를 뒤져서 찾아내는 함수 
    private T GetUIComponent<T>() where T : UIPopupBase
    {
        T popup = null;

        popup = GetPagePopupUI<T>();

        if (popup)
            return popup;

        popup = GetStakPopComponent<T>();

        if (popup)
            return popup;

        popup = GetPopComponent<T>();

        if (popup)
            return popup;

        return popup;
    }

    // UIData만 업데이트한다. 
    public void SetUIData(UIData data)
    {
        _datamanager.UpdateData(data);
    }  

    /// 기존 UIData를가지고 리프레쉬해준다. 
    public void ReFreshUpdate<T>() where T : UIPopupBase
    {
        T popup = GetUIComponent<T>();

        if (popup != null)
        {
            var uidata = _datamanager.GetData(popup.GetType());
            popup.ResetUIUpdate(uidata);
        }
    }

      // UIData를 설정하고 팝업이 있으면 업데이트 한다
    public void RefreshUI(UIData data)
    {
        _datamanager.UpdateData(data);

        string dataName = data.ToString();
        string popupName = dataName.Replace("Data","");

        Type popupType = Type.GetType(popupName);

        var popup = GetPopup(popupType);

        if (popup != null)
            popup.ResetUIUpdate(data);
        else
            Debug.Log("Popup is Null : " + popupType + ", popup Name : " + popupName);
    }    

    #endregion

    private void addStackPopup(UIPopupBase popup, bool hidePrev)
    {
        int lastCount = _pagePopupList[CurrentPageBase].Count;

        if (lastCount > 0)
        {
            _pagePopupList[CurrentPageBase][lastCount-1].BelowStateDoSomething();
        }
        else
            CurrentPageBase.BelowStateDoSomething();

        if (hidePrev)
        {
            if (lastCount > 0)
            {
                _pagePopupList[CurrentPageBase][lastCount-1].HideBaseUI();                
            }
            else
                CurrentPageBase.HideBaseUI();
        }        

        _pagePopupList[CurrentPageBase].Add(popup);
    }

    private void addPagePopup(UIPopupBase popup , bool delPrev)
    {
        if (delPrev)
            RemoveCurrentPageUI();
        else
            hidePrevBasePopup();

        CurrentPageBase = popup;
        _pagePopupList.Add(popup ,new List<UIPopupBase>());
    }

    private void hidePrevBasePopup()
    {
        if (CurrentPageBase == null)
            return;

        if (_pagePopupList.ContainsKey(CurrentPageBase) == false)
            return;

        List<UIPopupBase> stackList = _pagePopupList[CurrentPageBase];        

        for(int i =0; i < stackList.Count; i++)
            stackList[i].HideBaseUI();

        CurrentPageBase.HideBaseUI();
    }

    private void ShowPrePagePopup()
    {
        if (CurrentPageBase == null)
            return;

        List<UIPopupBase> stackList;

        if (_pagePopupList.ContainsKey(CurrentPageBase))
        {
            stackList = _pagePopupList[CurrentPageBase];

            if (stackList.Count > 0)
            {
                var stactpop = stackList[stackList.Count - 1];
                stactpop.ShowBaseUI();

                var uidata = _datamanager.GetData(stactpop.GetType());

                stactpop.ResetUIUpdate(uidata);
            }
            else
            {
                CurrentPageBase.ShowBaseUI();
                var uidata = _datamanager.GetData(CurrentPageBase.GetType());
                CurrentPageBase.ResetUIUpdate(uidata);
            }
        }
        else
            Debug.Log("Page 없는 버그");
    }

    private RectTransform GetCurrentStackTransform()
    {
        if(CurrentPageBase == null)
            return null;

        RectTransform stackTransform;        

        string stackTransName = GlobalUtil.AddString(CurrentPageBase.name, STACK);

        if (_stackCanvasList.TryGetValue(stackTransName, out stackTransform) == false)
        {
            var emptyGo = Instantiate(Resources.Load<GameObject>("Prefab/UI/CommonUI/STACK"));
            emptyGo.transform.SetParent(PageUI);
            emptyGo.name = stackTransName;

            stackTransform = emptyGo.transform as RectTransform;
            stackTransform.localPosition = Vector3.one;
            stackTransform.localScale = Vector3.one;
            stackTransform.anchorMin = Vector2.zero;
            stackTransform.anchorMax = Vector2.one;
            stackTransform.offsetMin = Vector2.zero;
            stackTransform.offsetMax = Vector2.one;
            stackTransform.SetAsLastSibling();
            //var canvas = emptyGo.GetComponent<Canvas>();

            _stackCanvasList.Add(stackTransName, stackTransform);
        }

        return stackTransform;
    }

    private void selectParentBelongtoUI(UIPopupBase popup, UIPopupBase.PopupType type, bool option)
    {
        switch(type)
        {
            case UIPopupBase.PopupType.SUB_STACK:

                var parent = GetCurrentStackTransform();

                // 넣을데가 없으면 Page로 만든다
                if(parent == null)
                {
                    popup.transform.SetParent(PageUI, false);
                    addPagePopup(popup, option);
                }
                else
                {
                    popup.transform.SetParent(parent, false);
                    addStackPopup(popup, option);
                }

                break;
            case UIPopupBase.PopupType.PAGE:
                popup.transform.SetParent(PageUI, false);
                addPagePopup(popup, option);
                break;
            case UIPopupBase.PopupType.POP_UP:
                popup.transform.SetParent(PopUpUI, false);
                _popupList.Add(popup);
                break;
            case UIPopupBase.PopupType.FADE:
                popup.PopType = UIPopupBase.PopupType.FADE;
                popup.transform.SetParent(FrontUI, false);
                _popupList.Add(popup);                
                break;
            case UIPopupBase.PopupType.FRONT:
                popup.transform.SetParent(FrontUI, false);
                _popupList.Add(popup);
                break;
       
        }

        popup.transform.SetAsLastSibling();
    }


    /// <summary>
    ///  스택 팝업이있으면 스택을 지우고 없으면 베이스를 지운다     
    ///  
    /// </summary>    
    public bool PrevPopup()
    {
        // 팝업이 있으면 팝업지우고 
        if (_popupList.Count > 0)
            return RemoveTopPopup();

        // 스택팝업이 있으면 스택팝업 지우고 
        bool removeStack = RemoveCurrentTopStackPopup();        

        // 스택팝업이 없으면 베이스를 지운다 
        if (removeStack == false)
        {           
            
            RemoveCurrentPageUI();

            if(CurrentPageBase == null)
                //CreatePagePopUp<LobbyPopup>();

            return true;
                            
        }

        return true;
    }   

    private List<UIPopupBase> _tempList = new List<UIPopupBase>();

    // 기본 베이스 UI를 전부 지운다 : 당연히 그위에 쌓여있던 스택도 모두 지워진다 
    public void RemoveAllPagePopup()
    {
        int count = _pagePopupList.Count;

        if (count < 1)
            return;

        Dictionary<UIPopupBase, List<UIPopupBase>>.Enumerator iterator = _pagePopupList.GetEnumerator();

        _tempList.Clear();

        while (iterator.MoveNext())
        {
            List<UIPopupBase> stackList = iterator.Current.Value;

            if (stackList != null)
            {
                int stackCount = stackList.Count;

                for (int i = 0; i < stackCount; ++i)
                {
                    UIPopupBase pop = stackList[i];
                    RemoveAndReleasePopup(pop);
                }

                stackList.Clear();
                stackList = null;

                _tempList.Add(iterator.Current.Key);
            }
        }

        int pageCount = _tempList.Count;
        
        for (int i =0; i < pageCount; ++i)
        {
            UIPopupBase pop = _tempList[i];            
            RemoveAndReleasePopup(pop);
        }

        _tempList.Clear();
        _pagePopupList.Clear();
        CurrentPageBase = null;        
    }


    /// <summary>
    ///  최상위 스택을 지우면서 바로 이전의 스택 팝업을 리셋시킨다 
    /// </summary>
    /// <returns>지우면 true</returns>
    public bool RemoveCurrentTopStackPopup()
    {
        if (CurrentPageBase == null)
            return false;

        List<UIPopupBase> stacklist = _pagePopupList[CurrentPageBase];

        if (stacklist.Count < 1)
            return false;

        UIPopupBase popup = stacklist[stacklist.Count - 1];
        stacklist.Remove(popup);
        RemoveAndReleasePopup(popup);

        if (stacklist.Count > 0)
        {
            var stackPop = stacklist[stacklist.Count - 1];

            var uidata = _datamanager.GetData(stackPop.GetType());

            stackPop.ResetUIUpdate(uidata);
        }
        else
        {
            var uidata = _datamanager.GetData(CurrentPageBase.GetType());
            CurrentPageBase.ResetUIUpdate(uidata);
        }

        return true;
    }

    /// <summary>
    /// 현재 페이지에서 원하는 스택 팝업만 지우는 함수 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void RemoveStackPopup<T>(bool bShowPrevPopup = false) where T : UIPopupBase
    {
        UIPopupBase popup = _pagePopupList[CurrentPageBase].Find( (pop)=> pop is T);

        if(popup)
        {
            _pagePopupList[CurrentPageBase].Remove(popup);
            RemoveAndReleasePopup(popup);

            bool remainStack = _pagePopupList[CurrentPageBase].Count > 0;

            if (bShowPrevPopup)
            {   // 이전 팝업을 업데이트해준다.
                if (remainStack)
                {
                    var pagePop = _pagePopupList[CurrentPageBase][_pagePopupList[CurrentPageBase].Count - 1];
                    var uidata = _datamanager.GetData(pagePop.GetType());
                    pagePop.ResetUIUpdate(uidata);
                }                    
                else
                {
                    var uidata = _datamanager.GetData(CurrentPageBase.GetType());
                    CurrentPageBase.ResetUIUpdate(uidata);
                    // 스택이 없으니 캔버스를 삭제한다. 
                    RemoveStackCanvas();
                }
            }
            else
            {
                if(remainStack == false)
                    RemoveStackCanvas();
            }

        }
    }

    /// <summary>
    ///  인자로 들어온 팝업을 리소스 해제와 함께 단순 삭제만 하는 함수 
    /// </summary>
    /// <param name="basepop"></param>
    private void RemoveAndReleasePopup(UIPopupBase basepop)
    {   
        if (basepop == null)
            Debug.Log("Error");

        Destroy(basepop.gameObject);
    }


    /// <summary>
    ///  현재  BaseUI 를 삭제하고 currentBase에 이전 마지막Stack BaseUI를 세팅한다 
    /// </summary>
    public void RemoveCurrentPageUI()
    {
        if (CurrentPageBase == null)
            return;

        // 현재 전체 스택을 지운다.
        RemoveAllStackPage();

        int count = _pagePopupList.Count;

        // 마지막 페이지면 삭제하지 않는다 
        if (count > 0)
        {
            _pagePopupList.Remove(CurrentPageBase);
            RemoveAndReleasePopup(CurrentPageBase);
            CurrentPageBase = null;
        }
        else
            return;

        int lastIndex = _pagePopupList.Count - 1;

        Dictionary<UIPopupBase, List<UIPopupBase>>.Enumerator keyIter = _pagePopupList.GetEnumerator();        

        int i = 0;

        while (keyIter.MoveNext())
        {
            if (i == lastIndex)
                CurrentPageBase = keyIter.Current.Key;

            i++;
        }

        ShowPrePagePopup();        
    }   

    /// <summary>
    ///  현재 베이스 위에있는 모든 스택팝업을 지운다 - 베이스는 존재하게 둠
    /// </summary>
    /// <returns></returns>
    public void RemoveAllStackPage()
    {
        if (CurrentPageBase == null)
            return;

        if (_pagePopupList.ContainsKey(CurrentPageBase) == false)
            return;

        List<UIPopupBase> poplist = _pagePopupList[CurrentPageBase];

        if (poplist.Count > 0)
        {
            int popcount = poplist.Count;

            for (int i = 0; i < popcount; i++)
            {
                UIPopupBase pop = poplist[0];

                poplist.Remove(pop);
                
                Destroy(pop.gameObject);
            }
        }

        RemoveStackCanvas();
    }

    private void RemoveStackCanvas()
    {
        Debug.Log("RemoveStackCanvas");

        var recttrans = GetCurrentStackTransform();
        _stackCanvasList.Remove(recttrans.name);
        Destroy(recttrans.gameObject);
    }



    public void AddToastMessage(ToastUIData data)
    {
        _toastQueueUI.Enqueue(data);
    }

    public void ClearAllToastMessage()
    {
        Debug.Log("clear Toast Message");
        _toastQueueUI.Clear();
    }

    /// <summary>
    ///  월드영역 터치가 가능한지 안한지 체크해준다
    /// </summary>
    private void CheckPossibleWorldTouch()
    {
        /*if(_blockWorldList.Count < 1)
        {
            //_touchPanel.SetCamera = Camera.main;
            _touchPanel.IsTouchAble = true;
        }
        else
            _touchPanel.IsTouchAble = false;*/

        //_touchPanel.SetCamera = null;
    }

    private void Update()
    {
        if(_toastQueueUI.Count > 0 && !_waitingToast)
        {
            var data = _toastQueueUI.Dequeue();

            var toastUI = CreateFrontUI<ToastMessageUI>();
            toastUI.SetUIData(data);
            _waitingToast = true;
        }
    }  

    public void ClearGameUI()
    {
        if (_inGameUIList.Count < 1)
            return;

        int uiCount = _inGameUIList.Count;

        UIBase uiGO;

        int lastIndex = uiCount - 1;

        for (int i =0; i < uiCount; i++)
        {
            uiGO = _inGameUIList[lastIndex - i];

            if(uiGO)
                Destroy(uiGO.gameObject);
        }

        _inGameUIList.Clear();
    }

    public void ClearUIManager()
    {
        ClearGameUI();
        ClearPopupList();
        RemoveAllPagePopup();
        DestroyPool();
    }

    void OnDestroy()
    {
        _applicationQuit = true;
    }


    #region Test

    /*void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "ToastMessage"))
        {
            UIManager.i.Toast("test :" + UnityEngine.Random.Range(0, 10));
        }

        if (GUI.Button(new Rect(0, 100, 100, 100), "Scene 2"))
        {

        }

        if (GUI.Button(new Rect(0, 200, 100, 100), "Scene 3"))
        {
            
        }

        if (GUI.Button(new Rect(0, 300, 100, 100), "Scene 4"))
        {


        }
    }*/
    #endregion
}


/// <summary>
///  UImanger 의 알람 팝업을 생성하기 위한 Extension 클래스 
/// </summary>
public static class UIExtension
{
    static public void PopupNotice(this MonoBehaviour obj, string text, UnityAction callback = null)
    {
        UINoticePopup popup = UIManager.i.CreatePopupUI<UINoticePopup>();

        popup.SetUIData(text);

        if (callback != null)
            popup.Callback = callback;
    }

    static public void StartLoading(this MonoBehaviour obj)
    {
        UILoadingProgress loadpop = UIManager.i.GetPopComponent<UILoadingProgress>();

        if (loadpop == null)
            UIManager.i.CreatePopupUI<UILoadingProgress>(null, UIPopupBase.PopupType.FRONT);
        else
        {
            var uidata = UIManager.i.GetUIData<UILoadingProgressData>();
            uidata.AddCount();
        }

    }

    static public void LoadComplete(this MonoBehaviour obj)
    {
        var uidata = UIManager.i.GetUIData<UILoadingProgressData>();

        if (uidata == null)
            return;

        uidata.ReduceCount();

        Debug.Log("Loading Count : " + uidata.WaitingCount);

        if (uidata.WaitingCount < 1)
            UIManager.i.RemovePopup<UILoadingProgress>();
    }

    static public void PrevPopup(this MonoBehaviour obj)
    {
        UIManager.i.PrevPopup();
    }    

    static public T Popup<T>(this MonoBehaviour obj) where T : UIPopupBase
    {
        return UIManager.i.CreatePopupUI<T>();
    }

    static public void ButtonText(this Button obj, string textValue)
    {
        Text btnText = null;

        btnText = obj.GetComponentInChildren<Text>();

        if (btnText)
            btnText.text = textValue;
    }

    static public void Toast(this MonoBehaviour obj, string text)
    {
        var data = new ToastUIData();
        data.Message = text;
        UIManager.i.AddToastMessage(data);
    }

}


