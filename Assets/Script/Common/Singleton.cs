using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T m_instance;
    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                T[] objects = FindObjectsOfType<T>();
                if (objects.Length == 1)
                {
                    m_instance = objects[0];
                }
                else if (objects.Length > 1)
                {
                    Debug.LogErrorFormat("Expected exactly 1 {0} but found {1}.", typeof(T).Name, objects.Length);
                }
            }

            return m_instance;
        }
    }

    private bool CheckInstance()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }
        Destroy(this);
        return false;
    }

    protected virtual void Awake()
    {
        if (!CheckInstance()) return;

        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
        if (m_instance == this)
        {
            m_instance = null;
        }
    }
}