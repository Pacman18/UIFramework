using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace UIContent
{
    public class UIDataManager
    {        
        // Key : DataType , Value : UIData
        private Dictionary<Type, UIData> _dataTypeContainer = new Dictionary<Type, UIData>();
        /// Key : PopupType , Value : DataType
        private Dictionary<Type, Type> _popupTypeContainer = new Dictionary<Type, Type>();

        private const string DATA = "Data";
        private const string _UINamespace = "UIContent.";
        

        public UIData AddData<T>() where T : UIPopupBase
        {
            var popupType = typeof(T);
            Type uiDataType;

            UIData createValue = null;

            // 팝업이 없엇단거임 
            if (_popupTypeContainer.ContainsKey(popupType) == false)
            {
                var compText = typeof(T).ToString().Replace(_UINamespace, string.Empty);

                string className = GlobalUtil.AddString(compText, DATA);

                var nameSpaceClassName = GlobalUtil.AddString(_UINamespace,className);

                uiDataType = Type.GetType(nameSpaceClassName);

                if (uiDataType != null)
                {
                    Debug.Log("Create UIData : "  + uiDataType);

                    createValue = Activator.CreateInstance(Type.GetType(nameSpaceClassName)) as UIData;
                    createValue.OpenInitData();

                    _dataTypeContainer.Add(createValue.GetType(), createValue);
                    _popupTypeContainer.Add(typeof(T), uiDataType);

                }
                else
                    Debug.LogWarning("UIData is Null : " + nameSpaceClassName);
            }
            else
            {
                Debug.Log("Waring Data Error Check Flow Need");
                /*uiDataType = _popupTypeContainer[popupType];
                data.OpenInitData();
                if(_dataTypeContainer.ContainsKey(uiDataType))
                {
                    _dataTypeContainer[uiDataType] = data;
                }
                else
                {
                    _dataTypeContainer.Add(uiDataType, data);
                }*/
            }

            return createValue;
        }

        public UIData GetData(Type popupType)
        {
            UIData data = null;
            Type dataType;

            if (_popupTypeContainer.TryGetValue(popupType, out dataType))
            {
                if (_dataTypeContainer.TryGetValue(dataType, out data))
                    return data;
            }

            Debug.Log("UIData is Null : " + popupType);

            return null;
        }

        public T GetUIData<T>() where T : UIData
        {
            foreach (var dataValue in _dataTypeContainer.Values)
            {
                if (dataValue is T)
                    return dataValue as T;
            }

            Debug.LogError("UIData is Null : " + typeof(T));
            return null;
        }

        public void UpdateData<T>(UIData data) where T : UIPopupBase
        {
            Type getClass;

            if (_popupTypeContainer.TryGetValue(typeof(T), out getClass))
            {
                if (_dataTypeContainer.ContainsKey(getClass))
                {
                    Debug.Log("UIData Update : " + getClass);
                    _dataTypeContainer[getClass] = data;
                }
                else
                {
                    _dataTypeContainer.Add(getClass,data);                    
                }
            }
            else
                Debug.LogWarning("UIData is Null : " + typeof(T));
        }

        public void UpdateData(UIData data)
        {
            Type getClass= data.GetType();

            if (_dataTypeContainer.ContainsKey(getClass))
                _dataTypeContainer[getClass] = data;
            else
                _dataTypeContainer.Add(getClass,data);                    
        }
    }

}
