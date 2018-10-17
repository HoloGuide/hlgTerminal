using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class SearchController : MonoBehaviour
{
    public Text Label;
    public Text Label_Deperture;
    public InputField InputField_Destination;

    private string m_nearestSta = "";
    private int clickedCount = 0;

    private void Start()
    {
        if (AppManager.Instance.NavigationState == NavState.Completed)
        {
            // 案内終了と表示
            Label.text = "案内が終了しました。";
            AppManager.Instance.NavigationState = NavState.Ready;
        }
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(m_nearestSta)) return;

        // 最寄り駅を取得
        m_nearestSta = AppManager.Instance.GetNearestStation();
        if (m_nearestSta == null)
        {
            Label_Deperture.text = "位置情報を取得できませんでした。";
        }
        else
        {
            Label_Deperture.text = m_nearestSta + "駅";
        }
    }

    private IEnumerator SearchRoute()
    {
        var dst = InputField_Destination.text;
        // "駅"を削除
        dst = dst.Replace("駅", "");

        // ルートを検索
        var ets = AppManager.Instance.GetComponent<EkispertTransitSearch>();
        var routes = new List<HoloGuide.RouteInfo>();
        yield return StartCoroutine(ets.SearchDeperture(routes, DateTime.Now, m_nearestSta, dst));

        if (routes == null)
        {
            yield break;
        }

        // Debug.LogFormat("{0}({1}) -> {2}({3})", routes[0].LeftSta, routes[0].LeftTime, routes[0].ArriveSta, routes[0].ArriveTime);

        AppManager.Instance.Routes = routes;

        SceneManager.LoadScene("Time");
    }

    public void Logo_OnClicked()
    {
        clickedCount++;

        if (clickedCount >= 5)
        {
            var r = new HoloGuide.Route();
            r.type = "route";
            r.filename = "map_test.json";
            r.start = 1;
            r.goal = 2;
            WebService.Instance.SendBroadcast(Newtonsoft.Json.JsonConvert.SerializeObject(r));

            AppManager.Instance.OutputDebugLog = true;
        }
    }

    public void BtnSerach_OnClicked()
    {
        var station = InputField_Destination.text;
        if (station == "")
        {
            Label.text = "駅名を入力してください。";
            return;
        }

        if (station == "仙台高専")
        {
            var routes = new List<HoloGuide.RouteInfo>();
            var r = new HoloGuide.RouteInfo();
            var t = DateTime.Now;
            r.LeftSta = "仙台高専";
            r.LeftTime = t.ToShortTimeString();
            t = t.AddMinutes(5);
            r.ArriveSta = "仙台高専";
            r.ArriveTime = t.ToShortTimeString();
            r.Money = 0;
            routes.Add(r);

            AppManager.Instance.Routes = routes;

            AppManager.Instance.SelectedRouteIndex = 0;
            SceneManager.LoadScene("Route");
            return;
        }

        StartCoroutine(SearchRoute());
    }

    public void BtnSetting_OnClicked()
    {
        AppManager.Instance.MoveToSetting();
    }
}
