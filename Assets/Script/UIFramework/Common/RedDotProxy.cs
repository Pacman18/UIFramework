using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UIContent;
using UnityEngine.Events;

public class RedDotProxy : SingleTone<RedDotProxy>
{
    // for Test 
    public enum RedDotKey
    {
        None = 0,
        Family, // Root 
        GrandFather, // Step_1
        GrandMother, // Step_1
        Father, // Step_2
        Mother, // Step_2
        Brother, // Step_3
    }

    private Dictionary<RedDotKey, RedDotData> _list = new Dictionary<RedDotKey, RedDotData>();

    private UnityAction<RedDotKey, int> _onChangeListener = null;   

    public void AddChangedListener(UnityAction<RedDotKey, int> listener)
    {
        _onChangeListener += listener;
    }

    public void RemoveChangedListener(UnityAction<RedDotKey, int> listener)
    {
        _onChangeListener -= listener;
    }

    public void SetCount(RedDotKey key, int count, RedDotKey parentKey = RedDotKey.None)
    {
        RedDotData parent = null;
        RedDotData child = null;        

        if (parentKey !=  RedDotKey.None)
        {
            if(_list.TryGetValue(parentKey, out parent) == false)
            {
                parent = new RedDotData(parentKey);
                _list.Add(parentKey, parent);
            }

            if (_list.TryGetValue(key, out child) == false)
            {
                child = new RedDotData(key);
                _list.Add(key, child);
            }

            child.SetParent(parent);
            child.SetCount(count);

            parent.AddChild(child);
        }
        else
        {
            if (_list.TryGetValue(key, out child) == false)
            {
                child = new RedDotData(key);
                _list.Add(key, child);
            }

            child.SetCount(count);
        }

        _onChangeListener.Invoke(child.Key, child.Count);        

        if(child.Parent != null)
        {
            RedDotData targetData = child.Parent;

            while (targetData != null)
            {
                _onChangeListener.Invoke(targetData.Key, targetData.Count);
                targetData = targetData.Parent;
            }
        }
    }

    public void Refresh(RedDotKey key)
    {
        RedDotData targetData;

        if (_list.TryGetValue(key, out targetData) == false)
        {
            targetData = new RedDotData(key);
            _list.Add(key, targetData);
        }

        while (targetData != null)
        {
            _onChangeListener.Invoke(targetData.Key, targetData.Count);
            targetData = targetData.Parent;
        }
    }

    public void Remove(RedDotKey key)
    {
        SetCount(key, 0);
    }

    public void Clear()
    {
        _list.Clear();
        _onChangeListener = null;
    }
}

public class RedDotData
{
    private RedDotProxy.RedDotKey _key;
    public RedDotProxy.RedDotKey Key=> _key;
    private int _count;

    private RedDotData _parent;
    private List<RedDotData> _children;

    public RedDotData Parent => _parent;
    public int Count
    {
        get
        {
            int sum = 0;

            if (_children != null)
            {
                foreach(var child in _children)
                    sum += child.Count;
            }   

            sum += _count;

            return sum;
        }
    }
    

    public void SetCount(int count)
    {
        _count = count;
    }

    public void RemoveCount()
    {
        _count = 0;
    }

    public void AddCount(int count)
    {
        _count += count;
    }

    public void SetParent( RedDotData parent)
    {
        _parent = parent;
    }

    public void AddChild(RedDotData child)
    {
        if (_children == null)
            _children = new List<RedDotData>();

        if (_children.Exists(x => x.Key == child.Key) == false)
        {   
            _children.Add(child);
        }
        else
            Debug.Log("레드닷 같은 키가 확인됨 키 확인 필요 : " + child.Key);
    }    

    public RedDotData(RedDotProxy.RedDotKey key)
    {
        _key = key;
    }

}
