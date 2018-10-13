using System;
using UnityEngine;

public class WebService : MonoBehaviour
{
#pragma warning disable 0414
    public event Action<string> OnConnected;
    public event Action<string> OnDisconnected;
    public event Action<string> OnLocationChanged;
    public event Action<string> OnReceived;

    private static WebService m_instance;
    public static WebService Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = (WebService)FindObjectOfType(typeof(WebService));
                if (m_instance == null)
                {
                    Debug.LogError("No GameObject with WebService attached could be found.");
                }
            }

            return m_instance;
        }
    }

    private bool CheckInstance()
    {
        if (m_instance == null)
        {
            m_instance = this as WebService;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }
        Destroy(this);
        return false;
    }

    private static AndroidJavaObject m_plugin = null;

    private void Awake()
    {
        if (!CheckInstance()) return;

        DontDestroyOnLoad(this);

#if UNITY_ANDROID && !UNITY_EDITOR
		m_plugin = new AndroidJavaObject("io.github.hologuide.hlgservice.ServiceManager");
        m_plugin.Call("Initialize", gameObject.name);
#endif
    }

    public void StartService()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
			m_plugin.Call ("StartService");
#endif
    }

    public void StopService()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
			m_plugin.Call ("StopService");
#endif
    }

    public void SendBroadcast(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
			m_plugin.CallStatic("SendBroadcast", message);
#endif
    }

    private void onConnected(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        OnConnected?.Invoke(message);
#endif
    }

    private void onDisconnected(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        OnDisconnected?.Invoke(message);
#endif
    }

    private void onLocationChanged(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        OnLocationChanged?.Invoke(message);
#endif
    }

    private void onReceived(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        OnReceived?.Invoke(message);
#endif
    }

    private void OnDestroy()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
			m_plugin.Call ("StopService");
#endif
    }

#pragma warning restore 0414
}