using HoloGuide;
using Sgml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using UnityEngine;

public class EkispertTransitSearch
{
    public bool IsDebug = false;

    private string Station_Src = "";
    private string Station_Dst = "";
    private string Station_Transit = "";

    private bool Plane = true;
    private bool Shinkansen = true;
    private bool LimitedExpress = true;
    private bool Bus = true;

    private SearchType m_searchType;

    private enum SearchType
    {
        Deperture,  // 出発時刻探索
        Arrival,    // 到着時刻探索
        LastTrain,  // 終電探索
        FirstTrain  // 始発探索
    }

    private const string ApiUrl = "http://api.ekispert.com/"; // APIのURL
    private const string Key = "LE_3nFzm5rsgA8sf"; // アクセスキー

    public List<RouteInfo> SearchDeperture(DateTime date, string src, string dst, string transit = "")
    {
        m_searchType = SearchType.Deperture; // 出発時刻探索

        Station_Src = src; // 出発駅
        Station_Dst = dst; // 目的駅
        Station_Transit = transit; // 経由駅

        Plane = false; // 飛行機
        Shinkansen = false; // 新幹線
        LimitedExpress = false; // 特急
        Bus = false; // バス

        string html = GetHtml(getEWSUrl(date));

        XDocument document = Parse(html);
        
        // 経路のリスト
        var routes = new List<RouteInfo>();

        // 経路は4つと仮定して4回ループ
        for (int i = 0; i < 4; i++)
        {
            var rootXPath = "/" + (i == 0 ? @"/div[@id='tabs_color']" : "") + @"/div[@class='candidate_list']/table[@class='candidate_list_table tabs_content']/tr[{0}]/td[3]/p[@class='candidate_list_txt']";
            IEnumerable<XElement> elms = document.XPathSelectElements(string.Format(rootXPath, "" + (i + 1)));

            var route = GetElement(elms);

            route.RName = "経路" + i;

            route.LeftSta = Station_Src;
            route.ArriveSta = Station_Dst;

            var baseXPath = @"//*[@id='route0" + (i + 1) + @"']";

            if (route.TCount != 0)
            {
                // 乗り換え番線
                elms = document.XPathSelectElements(baseXPath + @"/div/table[2]/tr[3]/td[4]/div");
                GetTLine(elms, route);

                // 乗り換え時刻
                elms = document.XPathSelectElements(baseXPath + @"/div/table[2]/tr[3]/td[1]");
                GetTransTime(elms, route);

                // 乗り換え駅名
                elms = document.XPathSelectElements(baseXPath + @"/div/table[2]/tr[3]/td[3]");
                GetPassSta(elms, route);
            }

            routes.Add(route);
        }

        return routes;
    }

    private List<RouteInfo> _Search()
    {
        var date = new DateTime(2018, 10, 14, 12, 10, 00);

        m_searchType = SearchType.Deperture; // 出発時刻探索

        Station_Src = "長町"; // 出発駅
        Station_Dst = "愛子"; // 目的駅
        // Station_Transit = "仙台"; // 経由駅

        Plane = false; // 飛行機
        Shinkansen = false; // 新幹線
        LimitedExpress = false; // 特急
        Bus = false; // バス

        string html = GetHtml(getEWSUrl(date));

        XDocument document = Parse(html);

        //TODO : 駅名の取得、路線(仙山線等)の取得、完成後UIとのマージ
        //全体的にごり押し

        //経路①
        IEnumerable<XElement> getInfo = document.XPathSelectElements(@"//div[@id='tabs_color']/div[@class='candidate_list']/table[@class='candidate_list_table tabs_content']/tr[1]/td[3]/p[@class='candidate_list_txt']");
        var routeA = new RouteInfo();
        routeA.RName = "経路1";
        routeA = GetElement(getInfo);
        //getInfo = document.XPathSelectElements(@"//*[id='route01']//td[@id='route_start_txt']");

        if (routeA.TCount != 0)
        {
            getInfo = document.XPathSelectElements(@"//*[@id='route01']/div/table[2]/tr[3]/td[4]/div");
            GetTLine(getInfo, routeA);
            getInfo = document.XPathSelectElements(@"//*[@id='route01']/div/table[2]/tr[3]/td[1]");
            GetTransTime(getInfo, routeA);
            getInfo = document.XPathSelectElements(@"//*[@id='route01']/div/table[2]/tr[3]/td[3]");
            GetPassSta(getInfo, routeA);
        }
        //*[@id="route01"]/div/table[2]/tbody/tr[1]/td[2]
        //*[@id="route01"]/div/table[2]/tbody/tr[3]/td[3]
        //*[@id="route01"]/div/table[2]/tbody/tr[5]/td[2]

        //経路②
        getInfo = document.XPathSelectElements(@"//div[@class='candidate_list']/table[@class='candidate_list_table tabs_content']/tr[2]/td[3]/p[@class='candidate_list_txt']");
        var routeB = new RouteInfo();
        routeB.RName = "経路2";
        routeB = GetElement(getInfo);
        if (routeB.TCount != 0)
        {
            getInfo = document.XPathSelectElements(@"//*[@id='route02']/div/table[2]/tr[3]/td[4]/div");
            GetTLine(getInfo, routeB);
            getInfo = document.XPathSelectElements(@"//*[@id='route02']/div/table[2]/tr[3]/td[1]");
            GetTransTime(getInfo, routeB);
            getInfo = document.XPathSelectElements(@"//*[@id='route02']/div/table[2]/tr[3]/td[3]");
            GetPassSta(getInfo, routeB);
        }

        //経路③
        getInfo = document.XPathSelectElements(@"//div[@class='candidate_list']/table[@class='candidate_list_table tabs_content']/tr[3]/td[3]/p[@class='candidate_list_txt']");
        var routeC = new RouteInfo();
        routeC.RName = "経路3";
        routeC = GetElement(getInfo);
        if (routeC.TCount != 0)
        {
            getInfo = document.XPathSelectElements(@"//*[@id='route03']/div/table[2]/tr[3]/td[4]/div");
            GetTLine(getInfo, routeC);
            getInfo = document.XPathSelectElements(@"//*[@id='route03']/div/table[2]/tr[3]/td[1]");
            GetTransTime(getInfo, routeC);
            getInfo = document.XPathSelectElements(@"//*[@id='route03']/div/table[2]/tr[3]/td[3]");
            GetPassSta(getInfo, routeC);
        }

        //経路④
        getInfo = document.XPathSelectElements(@"//div[@class='candidate_list']/table[@class='candidate_list_table tabs_content']/tr[4]/td[3]/p[@class='candidate_list_txt']");
        var routeD = new RouteInfo();
        routeD.RName = "経路4";
        routeD = GetElement(getInfo);
        if (routeD.TCount != 0)
        {
            getInfo = document.XPathSelectElements(@"//*[@id='route04']/div/table[2]/tr[3]/td[4]/div");
            GetTLine(getInfo, routeD);
            getInfo = document.XPathSelectElements(@"//*[@id='route04']/div/table[2]/tr[3]/td[1]");
            GetTransTime(getInfo, routeD);
            getInfo = document.XPathSelectElements(@"//*[@id='route04']/div/table[2]/tr[3]/td[3]");
            GetPassSta(getInfo, routeD);
        }

        return new List<RouteInfo>() { routeA, routeB, routeC, routeD };
    }

    /// <summary>
    /// HTML -> XML　に変換
    /// </summary>
    /// <param name="content">HTML</param>
    /// <returns>XML</returns>
    public XDocument Parse(string content)
    {
        using (var reader = new StringReader(content))
        using (var sgmlReader = new SgmlReader { DocType = "HTML", CaseFolding = CaseFolding.ToLower, IgnoreDtd = true, InputStream = reader })
        {
            return XDocument.Load(sgmlReader);
        }
    }

    /// <summary>
    /// HTMLファイルの取得
    /// </summary>
    /// <param name="url">URL</param>
    /// <returns>HTML</returns>
    public string GetHtml(string url)
    {
        string html;
        bool isDone = false;
        WWW www = new WWW(url);

        while (true)
        {
            if (isDone == false && www.isDone)
            {
                html = www.text;

                break;
            }
        }
        // Debug.Log(html);
        return html;
    }

    /// <summary>
    /// 時間、金額、乗り換え回数取得
    /// </summary>
    /// <param name="Info"></param>
    private RouteInfo GetElement(IEnumerable<XElement> Info)
    {
        int i = 0;
        var rI = new RouteInfo();
        foreach (XElement el in Info)
        {
            switch (i)
            {
                case 0:
                    rI.Time = el.Value;
                    //Debug.Log(rI.Time);
                    rI.LeftTime = rI.Time.Remove(5);
                    rI.ArriveTime = rI.Time.Remove(13).Remove(0, 8);
                    if (IsDebug)
                    {
                        Debug.Log("出発: " + rI.LeftTime);
                        Debug.Log("到着: " + rI.ArriveTime);
                    }
                    break;
                case 1:
                    rI.TCount = int.Parse(el.Value.Remove(el.Value.Length - 1).Remove(0, 3));
                    if (IsDebug)
                    {
                        Debug.Log("乗り換え回数: " + (rI.TCount + 1));
                    }
                    Array.Resize(ref rI.TransTime, 2 + 2 * rI.TCount);
                    break;
                case 2:
                    rI.Money = int.Parse(el.Value.Remove(el.Value.Length - 1).Remove(0, 3));
                    if (IsDebug)
                    {
                        Debug.Log("料金: " + rI.Money);
                    }
                    break;
                default:
                    break;
            }
            i++;


        }
        return rI;
    }

    /// <summary>
    /// 乗り換え番線を取得したい人生だった
    /// 現状2回以上の乗り換えに対応してません。
    /// </summary>
    /// <param name="Info"></param>
    /// <param name="rI"></param>
    /// <returns></returns>
    private void GetTLine(IEnumerable<XElement> Info, RouteInfo rI)
    {
        int i = 0;
        foreach (XElement el in Info)
        {
            if (i < 2)
            {
                //Debug.Log(el.Value.Replace(" ", "").Replace("　", "").Replace("\n", ""));
                rI.TLine[i] = el.Value.Replace(" ", "").Replace("　", "").Replace("\n", "");
                i++;
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// 経由駅の取得
    /// </summary>
    private void GetPassSta(IEnumerable<XElement> Info, RouteInfo rI)
    {
        foreach (XElement el in Info)
        {
            rI.PassSta = el.Value.Replace("地図", "").Trim();
        }
    }

    /// <summary>
    /// 乗り換え時間の取得
    /// </summary>
    /// <param name="Info"></param>
    /// <param name="rI"></param>
    private void GetTransTime(IEnumerable<XElement> Info, RouteInfo rI)
    {
        string tmp = "";
        foreach (XElement el in Info)
        {
            //Debug.Log(el.Value.Replace(" ", "").Replace("　", "").Replace("\n", ""));
            tmp = el.Value.Replace(" ", "").Replace("　", "").Replace("\n", "");
        }

        rI.TransTime[0] = tmp.Remove(7).Remove(0, 2);
        if (IsDebug) Debug.Log("(乗り換え)到着: " + rI.TransTime[0]);
        rI.TransTime[1] = tmp.Remove(0, 9);
        if (IsDebug) Debug.Log("(乗り換え)出発: " + rI.TransTime[1]);
        //到着15:40出発16:00
    }

    private string getEWSUrl(DateTime date)
    {
        string url = ApiUrl + "v1/xml/search/course/light";
        url += "?key=" + Key;

        // 駅
        url += "&from=" + WWW.EscapeURL(Station_Src);
        url += "&to=" + WWW.EscapeURL(Station_Dst);

        if (Station_Transit != "")
        {
            url += "&via=" + WWW.EscapeURL(Station_Transit);
        }

        // 時刻・検索種別等
        url += "&date=" + date.Year + (date.Month <= 9 ? "0" : "") + date.Month + (date.Day <= 9 ? "0" : "") + date.Day;

        switch (m_searchType)
        {
            case SearchType.Deperture:
                url += "&searchType=departure";
                url += "&time=" + (date.Hour < 10 ? "0" : "") + date.Hour + (date.Minute < 10 ? "0" : "") + date.Minute;
                break;
            case SearchType.Arrival:
                url += "&searchType=arrival";
                url += "&time=" + (date.Hour < 10 ? "0" : "") + date.Hour + (date.Minute < 10 ? "0" : "") + date.Minute;
                break;
            case SearchType.LastTrain:
                url += "&searchType=lastTrain";
                break;
            case SearchType.FirstTrain:
                url += "&searchType=firstTrain";
                break;
        }

        // 交通手段選択
        url += "&plane=" + (Plane ? "true" : "false");
        url += "&shinkansen=" + (Shinkansen ? "true" : "false");
        url += "&limitedExpress=" + (LimitedExpress ? "true" : "false");
        url += "&bus=" + (Bus ? "true" : "false");

        url += "&redirect=true";

        if (IsDebug) Debug.Log(url);

        return url;
    }
}
