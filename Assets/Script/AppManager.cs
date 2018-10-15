using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : Singleton<AppManager>
{
    /// <summary>
    /// 設定から元のシーンに戻る
    /// </summary>
    public void BackFromSetting()
    {
        if (!string.IsNullOrEmpty(m_prevSceneName))
        {
            SceneManager.LoadScene(m_prevSceneName);
        }
    }

    /// <summary>
    /// 現在のシーン名を保存して設定シーンに遷移
    /// </summary>
    public void MoveToSetting()
    {
        m_prevSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Setting");
    }

    public void NavigationFinished()
    {
        // Searchシーンに戻る
        NavigationState = NavState.Completed;
        actionDoMainThreads.Enqueue(() =>
        {
            SceneManager.LoadScene("Search");
        });
    }

    public const string VERSION = "ver.1.1-20181015";

    public bool Connected { get; private set; }
    public NavState NavigationState = NavState.Ready;
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
            StartCoroutine(Destination_OnValueChanged(value));
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

        Connected = false;

        StartCoroutine("GetLocationFromInput");

#if UNITY_EDITOR
        // Editor用GPS位置情報 (長町駅)
        m_currentLocation = new HoloGuide.Location();
        m_currentLocation.lat = 38.2269767492871;
        m_currentLocation.lng = 140.8854836887674;
#endif

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

    private IEnumerator Destination_OnValueChanged(string dst)
    {
        // 最寄り駅を取得
        var nearestStation = (new GetStation()).GetStationName(m_currentLocation.lng, m_currentLocation.lat);

        // "駅"を削除
        nearestStation = nearestStation.Replace("駅", "");
        dst = dst.Replace("駅", "");

        // ルートを検索
        var ets = this.GetComponent<EkispertTransitSearch>();
        var routes = new List<HoloGuide.RouteInfo>();
        yield return StartCoroutine(ets.SearchDeperture(routes, DateTime.Now, nearestStation, dst));

        if (routes == null)
        {
            yield break;
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
            // Searchシーンに戻る
            NavigationState = NavState.Ready;
            actionDoMainThreads.Enqueue(() =>
            {
                SceneManager.LoadScene("Search");
            });
        }
    }

    private void WebService_OnConnected(string ip)
    {
        Debug.Log("Connected: " + ip);
        firstConnectedIP = ip;

        Connected = true;
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

public enum NavState
{
    Ready, // 案内待機
    Navigating, // 案内中
    Completed // 案内終了
}
