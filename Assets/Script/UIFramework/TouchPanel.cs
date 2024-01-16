using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// 3D 오브젝트를 체크하면서 
/// UI 가 뜨면  3D 체크를 막기위한 중간 패널 
/// </summary>
public class TouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private Camera _camera;

    private int _targetLayer = 11; // Player Layer
    private int _mapLayer = 8; // 월드상 맵 Layer
    private int _buildLayer = 12; // 건물 Layer
    private Ray _ray; 

    public int Layer
    {
        set { _targetLayer = value; }
    }

    private UnityAction<GameObject> _pickGoCallback = null;
    private UnityAction<Vector3> _pickPositionCallback = null;
    private UnityAction _touchUpCallback = null;
    private bool _isTouch = true;

    private void AddPickObjectListener(UnityAction<GameObject> callback)
    {
        _pickGoCallback += callback;
    }

    private void RemovePickObjectListener(UnityAction<GameObject> callback)
    {
        _pickGoCallback -= callback;
    }

    private void AddPickPosListener(UnityAction<Vector3> callback)
    {
        _pickPositionCallback += callback;
    }

    private void RemovePickPosListener(UnityAction<Vector3> callback)
    {
        _pickPositionCallback -= callback;
    }

    private void AddTouchUpListener(UnityAction callback)
    {
        _touchUpCallback += callback;
    }

    private void RemoveTouchUpListener(UnityAction callback)
    {
        _touchUpCallback -= callback;
    }


    // 터치를 막기위한 변수
    public bool IsTouchAble
    {
        get { return _isTouch; }

        set { _isTouch = value; }
    }


    // 카메라가 없으면 터치가 불가능해진다 
    public Camera SetCamera
    {
        set
        {
            _camera = value;

            if(value)
                _isTouch = true;
            else
                _isTouch = false;
        }
    }

    void Awake()
    {
        _camera = Camera.main;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isTouch)
            return;

        if (_camera == null)
            return;

        _ray = _camera.ScreenPointToRay(eventData.position);        

        RaycastHit pickResult;

        if (_targetLayer == 0)
            _targetLayer = LayerMask.GetMask("Default");
        

        if (Physics.Raycast(_ray, out pickResult, float.MaxValue, 1 << _targetLayer | 1 << _buildLayer))
        {
            if (_pickGoCallback != null)
                _pickGoCallback(pickResult.collider.gameObject);
        }
        else
        {
            if (Physics.Raycast(_ray, out pickResult, float.MaxValue, 1 << _mapLayer))
            {
                if (_pickPositionCallback != null)
                    _pickPositionCallback(pickResult.point);
            }
            else
                if (_pickPositionCallback != null)
                    _pickPositionCallback(Vector3.zero);
        }
            
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _touchUpCallback?.Invoke();
    }

}
