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
    public int SelectedRouteIndex = -1;
    public List<HoloGuide.RouteInfo> Routes;
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
    private bool useLocationFromInput = true;
    private float intervalSeconds = 1.0f;

    private void Start()
    {
        // WebServiceの初期化・イベント登録
        WebService.Instance.StartService();

        WebService.Instance.OnConnected += WebService_OnConnected;
        WebService.Instance.OnDisconnected += WebService_OnDisconnected;
        WebService.Instance.OnReceived += WebService_OnReceived;
        WebService.Instance.OnLocationChanged += WebService_OnLocationChanged;

        m_currentLocation = new HoloGuide.Location();
        m_currentLocation.type = "location";
        m_currentLocation.lat = 38.223516;
        m_currentLocation.lng = 140.872391;

        StartCoroutine("GetLocationFromInput");
    }

    private void Update()
    {
        while (actionDoMainThreads.Count > 0)
        {
            var action = actionDoMainThreads.Dequeue();

            action?.Invoke();
        }
    }

    // 位置情報の取得 (OnLocationChangedが発火されるまでの暫定的使用)
    private IEnumerable GetLocationFromInput()
    {
        while (true)
        {
            if (!useLocationFromInput)
            {
                yield break;
            }

            if (Input.location.isEnabledByUser)
            {
                switch (Input.location.status)
                {
                    case LocationServiceStatus.Stopped:
                        Input.location.Start();
                        break;
                    case LocationServiceStatus.Running:
                        var loc = Input.location.lastData;
                        m_currentLocation = new HoloGuide.Location();
                        m_currentLocation.type = "location";
                        m_currentLocation.lng = loc.longitude;
                        m_currentLocation.lat = loc.latitude;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Debug.Log("location is disabled by user!");
                yield break;
            }

            yield return new WaitForSeconds(intervalSeconds);
        }
    }

    private void Destination_OnValueChanged(string dst)
    {
        // 最寄り駅を取得
        var nearestStation = (new GetStation()).GetStationName(m_currentLocation.lng, m_currentLocation.lat);
        nearestStation = nearestStation.Substring(0, nearestStation.Length - 1); // "駅"を取り除く

        // ルートを検索
        var ets = new EkispertTransitSearch();
        var routes = ets.SearchDeperture(DateTime.Now, nearestStation, dst);

        if (routes == null)
        {
            return;
        }

        // Debug.LogFormat("{0}({1}) -> {2}({3})", routes[0].LeftSta, routes[0].LeftTime, routes[0].ArriveSta, routes[0].ArriveTime);

        Routes = routes;

        SceneManager.LoadScene("Time");
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
        useLocationFromInput = false;
        var location = JsonConvert.DeserializeObject<HoloGuide.Location>(json);

        m_currentLocation = location;
    }

}
