using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UIContent
{
    public class HUD : UIBase 
    {
        private const string HUDTRANS = "HUD_LAYER";
        private const string WORLDTRANS = "WorldUI";

        // Hud UI 
        private List<UIBase> _inGameHUDUIList = new List<UIBase>();

        // Game 속 3D UI
        private List<UIBase> _inGameWorldUIList = new List<UIBase>();

        private Transform _worldTrans;
        private Transform _hudTrans;

        public Camera _hudCamera;


        public Transform HudTrans
        {
            get { return _hudTrans; }
        }

        public Transform WorldTrans
        {
            get { return _worldTrans; }
        }

        public List<UIBase> HUDList
        {
            get { return _inGameHUDUIList; }
        }

        void Awake ()
        {
            _worldTrans = transform.Find(WORLDTRANS);
            _hudTrans = transform.Find(HUDTRANS);

            _hudCamera = GetComponentInChildren<Camera>();
        }


        public void AddHUDUI(UIBase ui)
        {
            _inGameHUDUIList.Add(ui);
        }

        public void RemoveHUDUI(UIBase ui)
        {
            if (ui == null)
                return;

            _inGameHUDUIList.Remove(ui);
            Destroy(ui.gameObject);
        }

        public void AddWorldUI(UIBase ui)
        {
            _inGameWorldUIList.Add(ui);
        }

        public void RemoveWorldUI(UIBase ui)
        {
            _inGameWorldUIList.Remove(ui);
            Destroy(ui.gameObject);
        }
        
        public T GetHUDUI<T>() where T : UIBase
        {
            T ui = _inGameHUDUIList.Find( (x)=> x is T) as T;

            if (ui == null)
                Debug.Log("HUD UI 찾는게 없음 " + typeof(T));

            return ui;
        }

        public void Clear()
        {
            UIBase ui;

            int count = _inGameHUDUIList.Count;

            for(int i =0; i < count; i++)
            {
                ui = _inGameHUDUIList[i];

                if(ui)
                    Destroy(ui.gameObject);

            }

            count = _inGameWorldUIList.Count;

            for (int i = 0; i < count; i++)
            {
                ui = _inGameWorldUIList[i];

                if (ui)
                    Destroy(ui.gameObject);

            }

            _inGameHUDUIList.Clear();
            _inGameWorldUIList.Clear();
        }
    }
}


