using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Reflection;

namespace UIContent
{
    public class UIBase : MonoBehaviour
    {
        [HideInInspector]
        public int Id;   // 특정한 ID를 저장 및 Reupdate에 사용된다 

        [HideInInspector]
        public long UID;

        protected List<UIBase> childList;    

        private RectTransform _rectTrans;

        public RectTransform RectTrans
        {
            get
            {
                if (_rectTrans == null)
                    _rectTrans = transform as RectTransform;

                return _rectTrans;                       
            }

            set
            {
                _rectTrans = value;
            }
        }


        // 3D 월드 픽킹을 막기위한 변수 
        [HideInInspector]
        public bool Block = false;

        protected Graphic[] _tempGraphic = null;

        

        // UI Create가 된 이후 세팅되는 함수 
        public virtual void SetUIData(UIData data) {}

        public virtual void ShowBaseUI()
        {
            gameObject.SetActive(true);
        }
        public virtual void HideBaseUI()
        {
            gameObject.SetActive(false);
        }

        public virtual int GetWidth()
        {
            if (_rectTrans == null)
                _rectTrans = transform as RectTransform;

            return Mathf.RoundToInt(_rectTrans.rect.width);
        }

        public virtual float GetHeight()
        {
            if (_rectTrans == null)
                _rectTrans = transform as RectTransform;

            return _rectTrans.rect.height;
        }

        /// <summary>
        ///  UI 전용 GetComponent Wraping Code 
        /// </summary>
        /// <param name="childName"></param>
        /// <returns></returns>
        #region FindChild UI element Method Wrapping 

        protected TMPro.TextMeshProUGUI GetTMPText(string childName)
        {
            TMPro.TextMeshProUGUI childresult;

            Transform trans = transform.Find(childName);

            if (trans)
            {
                return trans.GetComponent<TMPro.TextMeshProUGUI>();
            }
            else
            {
                TMPro.TextMeshProUGUI[] texts = transform.GetComponentsInChildren<TMPro.TextMeshProUGUI>(true);

                if (texts.Length > 0)
                {
                    for (int i = 0; i < texts.Length; i++)
                    {
                        if (texts[i].name == childName)
                        {
                            childresult = texts[i];
                            return childresult;
                        }
                    }

                    //Debug.Log("Component is Not found" + childName);

                    return null;
                }
                else
                {
                    //Debug.Log("Component is Not found" + childName);
                    return null;
                }
            }
        }

        protected Text GetText(string childName)
        {
            Text childresult;

            Transform trans = transform.Find(childName);

            if (trans)
            {
                return trans.GetComponent<Text>();
            }
            else
            {
                Text[] texts = transform.GetComponentsInChildren<Text>(true);

                if (texts.Length > 0)
                {
                    for (int i = 0; i < texts.Length; i++)
                    {
                        if (texts[i].name == childName)
                        {
                            childresult = texts[i];
                            return childresult;
                        }
                    }

                    //Debug.Log("Component is Not found" + childName);

                    return null;
                }
                else
                {
                    //Debug.Log("Component is Not found" + childName);
                    return null;
                }
            }
        }

        protected Text GetText(Transform specTrans, string childName)
        {
            Text childresult;

            Transform trans = specTrans.Find(childName);

            if (trans)
            {
                return trans.GetComponent<Text>();
            }
            else
            {
                Text[] texts = specTrans.GetComponentsInChildren<Text>(true);

                if (texts.Length > 0)
                {
                    for (int i = 0; i < texts.Length; i++)
                    {
                        if (texts[i].name == childName)
                        {
                            childresult = texts[i];
                            return childresult;
                        }
                    }

                    //Debug.Log("Component is Not found " + childName);

                    return null;
                }
                else
                {

                    //Debug.Log("Component is Not found" + childName);
                    return null;
                }
            }
        }

        protected Image GetImage(string childName)
        {
            Image childresult;

            Transform trans = transform.Find(childName);

            if (trans)
            {
                return trans.GetComponent<Image>();
            }
            else
            {
                Image[] images = transform.GetComponentsInChildren<Image>(true);

                if (images.Length > 0)
                {
                    for (int i = 0; i < images.Length; i++)
                    {
                        if (images[i].name == childName)
                        {
                            childresult = images[i];
                            return childresult;
                        }
                    }

                    //Debug.Log("Component is Not found : " + childName);

                    return null;
                }
                else
                {
                    Debug.Log("Component is Not found");
                    return null;
                }
            }
        }
        protected Image GetImage(Transform specTrans, string childName)
        {
            Image childresult;

            Transform trans = specTrans.Find(childName);

            if (trans)
            {
                return trans.GetComponent<Image>();
            }
            else
            {
                Image[] images = specTrans.GetComponentsInChildren<Image>(true);

                if (images.Length > 0)
                {
                    for (int i = 0; i < images.Length; i++)
                    {
                        if (images[i].name == childName)
                        {
                            childresult = images[i];
                            return childresult;
                        }
                    }

                    Debug.Log("Component is Not found : " + childName);

                    return null;
                }
                else
                {
                    Debug.Log("Component is Not found");
                    return null;
                }
            }
        }

        protected Button GetButton(string childName)
        {
            Button childresult;

            Transform trans = transform.Find(childName);

            if (trans)
            {
                return trans.GetComponent<Button>();
            }
            else
            {
                Button[] buttons = transform.GetComponentsInChildren<Button>(true);

                if (buttons.Length > 0)
                {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        if (buttons[i].name == childName)
                        {
                            childresult = buttons[i];
                            return childresult;
                        }
                    }

                    //Debug.Log("Component is Not found");

                    return null;
                }
                else
                {
                    //Debug.Log("Component is Not found");
                    return null;
                }
            }
        }
        protected Button GetButton(Transform specTrans, string childName)
        {
            Button childresult;

            Transform trans = specTrans.Find(childName);

            if (trans)
            {
                return trans.GetComponent<Button>();
            }
            else
            {
                Button[] buttons = specTrans.GetComponentsInChildren<Button>(true);

                if (buttons.Length > 0)
                {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        if (buttons[i].name == childName)
                        {
                            childresult = buttons[i];
                            return childresult;
                        }
                    }

                    //Debug.Log("Component is Not found");

                    return null;
                }
                else
                {
                    //Debug.Log("Component is Not found");
                    return null;
                }
            }
        }

        #endregion

        /// <summary>
        /// ButtonEvent 스크립트없이 버튼 OnClick 등록 & 사용하는 Method
        /// 기존 : ButtonEvent public type 변경 
        /// </summary>
        /// <param name="button"></param>
        protected void RegistButtonOnClickEvent(Button button)
        {
            if (button == null)
                print(gameObject.name);

            button.onClick.AddListener( ()=> { OnButtonClick(button.name);} );                
        }
       
        protected void RegistButtonOnClickEvent(Button button, UnityAction callback)
        {
            if (button == null)
                print(gameObject.name);

            button.onClick.AddListener(callback);                
        }

        protected void RegistButtonOnClickEvent(string btnName, UnityAction callback)
        {
            RegistButtonOnClickEvent(GetButton(btnName), callback);           
        }

        /// <summary>
        ///  버튼을 이름으로 찾으면서 반환하는 함수 
        /// </summary>
        /// <param name="btnName"></param>
        /// <returns></returns>
        protected Button RegistButtonOnClickEvent(string btnName, Transform trans = null)
        {
            Button findButton;

            if(trans)
                findButton = GetButton(trans, btnName);
            else
                findButton = GetButton(btnName);

            if(findButton != null)
                findButton.onClick.AddListener(() => { OnButtonClick(findButton.name); });

            return findButton;
        }    

        /// <summary>
        ///  업데이트에서도 돌릴수 있음 
        /// </summary>
        /// <param name="c_color"></param>
        /// <param name="trans"></param>
        public void GraphicColorChange(Color c_color , Transform trans = null)
        {
            if (trans == null)
                trans = transform;

            if(_tempGraphic == null)
                _tempGraphic = trans.GetComponentsInChildren<Graphic>();

            for (int i = 0; i < _tempGraphic.Length; i++)
            {
                _tempGraphic[i].color = c_color;
            }
        }


        /// <summary>
        /// 모든 버튼에 대한 UIBase Onclick 이벤트 등록하는 함수 
        /// </summary>
        /// <returns>버튼 배열 반환</returns>
        protected Button[] RegistAllButtonOnClickEvent()
        {
            Button[] btns = transform.GetComponentsInChildren<Button>(true);

            for(int i = 0; i< btns.Length; i++)
                RegistButtonOnClickEvent(btns[i]);

            return btns;
        }
        // 특정 Transfom 하위만 찾아 등록한다 
        protected Button[] RegistChildButtonOnClickEvent(Transform spectTrans)
        {
            Button[] btns = spectTrans.GetComponentsInChildren<Button>(true);

            for (int i = 0; i < btns.Length; i++)
                RegistButtonOnClickEvent(btns[i]);

            return btns;
        }

        protected void UnRegistAllButtonListener(Transform trans)
        {
            Button[] btns = trans.GetComponentsInChildren<Button>();

            for (int i = 0; i < btns.Length; i++)
                btns[i].onClick.RemoveAllListeners();
        }

        protected void UnRegistButtonListener(Button btn)
        {
            btn.onClick.RemoveAllListeners();
        }
        

        /// <summary>
        ///  특정 UI만 제외하고 모두 Show 를 부른다 , param == null 일시 모두 켜기 
        /// </summary>
        /// <param name="exceptUIs"></param>
        protected void ShowUIExceptThese(params UIBase[] exceptUIs)
        {
            if (childList == null)
                return;

            for (int i = 0; i < childList.Count; i++)
            {
                if (Array.Exists<UIBase>(exceptUIs, (ui) => ui == childList[i]))
                    continue;

                childList[i].ShowBaseUI();
            }
        }

        /// <summary>
        ///  특정 UI 만 제외하고 모두 Hide 하는 함수 
        ///  단 childList에 있는 UI만 가능하다  , param == null 일시 모두 끄기s
        /// </summary>
        /// <param name="exceptUIs"></param>
        protected void HideUIExceptThese(params UIBase[] exceptUIs) 
        {
            if (childList == null)
                return;

            for(int i =0; i < childList.Count; i++)
            {
                if (Array.Exists<UIBase>(exceptUIs, (ui) => ui == childList[i]))
                    continue;

                childList[i].HideBaseUI();
            }
        }

        void OnDestroy()
        {
            if(childList != null)
            {
                childList.Clear();
                childList = null;
            }
        }

        public virtual void Close()
        {
            UIManager.i.RemoveGameUI(this);
            UIManager.i.Hud.RemoveHUDUI(this);
        }


        //
        // Summary:
        // 버튼 클릭 시 이벤트 받는다. 오브젝트 이름을 기준으로 한다.
        // 기본 버튼음 추가 
        //
        protected virtual void OnButtonClick(string name)
        {
            //SoundSystem.i.PlayShot(12);
        }

    }
}

