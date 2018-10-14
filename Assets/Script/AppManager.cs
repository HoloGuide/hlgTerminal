using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : Singleton<AppManager>
{
    public void BackFromSetting()
    {
        if (!string.IsNullOrEmpty(m_prevSceneName))
        {
            SceneManager.LoadScene(m_prevSceneName);
        }
    }

    public void MoveToSetting()
    {
        m_prevSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Setting");
    }

    public void NavigationFinished()
    {
        // Loadシーンに戻る
        actionDoMainThreads.Enqueue(() =>
        {
            SceneManager.LoadScene("Load");
        });
    }

    public bool OutputDebugLog = false;
    public string Destination
    {
        get
        {
            return m_dst;
        }
        set
        {
            Destination_OnValueChanged(value);
            m_dst = value;
        }
    }

    private string m_dst = "";
    private HoloGuide.Location m_currentLocation = null;
    private string m_prevSceneName { get; set; }
    private string firstConnectedIP = null;
    private Queue<Action> actionDoMainThreads = new Queue<Action>();

    private void Start()
    {
        // WebServiceの初期化・イベント登録
        WebService.Instance.StartService();

        WebService.Instance.OnConnected += WebService_OnConnected;
        WebService.Instance.OnDisconnected += WebService_OnDisconnected;
        WebService.Instance.OnReceived += WebService_OnReceived;
        WebService.Instance.OnLocationChanged += WebService_OnLocationChanged;
    }

    private void Update()
    {
        while (actionDoMainThreads.Count > 0)
        {
            var action = actionDoMainThreads.Dequeue();

            action?.Invoke();
        }
    }

    private void Destination_OnValueChanged(string dst)
    {
        // 最寄り駅を取得
        var nearestStation = new GetStation().GetStationName(m_currentLocation.lng, m_currentLocation.lat);

        // 
        

    }

    private void WebService_OnDisconnected(string ip)
    {
        Debug.Log("Disconnected: " + ip);

        if (ip == firstConnectedIP)
        {
            // Loadシーンに戻る
            actionDoMainThreads.Enqueue(() =>
            {
                SceneManager.LoadScene("Load");
            });
        }
    }

    private void WebService_OnConnected(string ip)
    {
        Debug.Log("Connected: " + ip);
        firstConnectedIP = ip;

        // Searchシーンに遷移
        actionDoMainThreads.Enqueue(() =>
        {
            SceneManager.LoadScene("Search");
        });
    }

    private void WebService_OnReceived(string json)
    {
        Debug.Log("Received: " + json);
        JsonParser.Instance.ParseJson(json);
    }

    private void WebService_OnLocationChanged(string json)
    {
        Debug.Log("Location Changed.");
        var location = JsonConvert.DeserializeObject<HoloGuide.Location>(json);

        m_currentLocation = location;
    }

}
