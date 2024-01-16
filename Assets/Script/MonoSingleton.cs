using UnityEngine;
using System;

/// <summary>
///  모노비헤버를 가진 싱글톤 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    protected static bool _applicationQuit = false;

    public static T i
    {
        get
        {
            if (st_instance != null)
                return st_instance;

            st_instance = CreateInstance();

            return st_instance;
        }
    }

    private static T st_instance = null;

    private static T CreateInstance()
    {
        if (_applicationQuit)
            return null;

        T instance = FindObjectOfType<T>();

        if (instance != null)
            return instance;

        GameObject go = new GameObject();
        go.name = typeof(T).ToString();
        DontDestroyOnLoad(go);
        return go.AddComponent<T>();
    }   

    protected void Destroy()
    {
        st_instance = null;
    }
}

/// <summary>
/// 오리지널 C# 오브젝트 싱글톤 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingleTone<T> : System.Object where T : SingleTone<T> , new()
{
    public static T Instance
    {
        get
        {
            if (st_instance != null)
                return st_instance;

            st_instance = new T();
            st_instance.OnAwake();

            return st_instance;
        }
    }

    private static T st_instance = default(T);    

    public virtual void OnAwake() { }
    
} 
