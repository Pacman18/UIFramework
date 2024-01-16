using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UIContent
{
    public class RedDotLinker : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _countText;

        [SerializeField]
        private Image _redDotImg;

        [SerializeField]
        private RedDotProxy.RedDotKey _key = RedDotProxy.RedDotKey.None;

        [SerializeField]
        private bool _countHide = false;

        private void Awake()
        {
            _redDotImg.enabled = false;
            _countText.enabled = false;
        }

        private void OnEnable()
        {
            RedDotProxy.Instance.AddChangedListener(OnChangedCount);
        }

        public void RegistKey(RedDotProxy.RedDotKey key)
        {
            _key = key;
            RedDotProxy.Instance.Refresh(_key);
        }

        private void OnChangedCount(RedDotProxy.RedDotKey key, int count)
        {
            if (_key != key)
                return;

            bool isShow = count > 0;            

            _redDotImg.enabled = isShow;            

            if(_countHide == false)
            {
                _countText.enabled = isShow;
                _countText.text = count.ToString();
            }
            else
                _countText.enabled = _countHide;

            //Debug.Log("OnChangedCount : " + key + ", count : " + count);
        }


        private void OnDisable()
        {
            RedDotProxy.Instance.RemoveChangedListener(OnChangedCount);
        }
    }
}

