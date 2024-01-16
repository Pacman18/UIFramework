using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIContent
{
    public class UIWorldBase : UIBase
    {
        private Transform _target;
        private Camera _worldCam;
        protected float _offset; // Y ¿É¼Ç
        private RectTransform _canvasRect;

        protected virtual void Awake()
        {
            _worldCam = Camera.main;
            _canvasRect = UIManager.i.CanvasRect;
        }

        public void SetWorldCamera(Camera cam)
        {
            _worldCam = cam;
        }

        public void SetTarget(Transform parent)
        {
            _target = parent;
        }

        public void ReleaseTarget()
        {
            _target = null;
        }

        private void LateUpdate()
        {
            if (_target)
            {
                var pos = _target.position + Vector3.up * _offset;

                Vector2 ViewportPosition = _worldCam.WorldToViewportPoint(pos);

                RectTrans.anchoredPosition = new Vector2(
                ((ViewportPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 0.5f)),
                ((ViewportPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f)));
            }
        }
    }

}

