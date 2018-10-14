﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using Sgml;
using System.Collections.Generic;
using Assets.EkispertWebService.Scripts.Editor;
using System;

public class Ekispert : EditorWindow
{
    private string[] DateYearItems;
    private int DateYearIndex = -1;
    private string[] DateMonthItems;
    private int DateMonthIndex = -1;
    private string[] DateDayItems;
    private int DateDayIndex = -1;

    private string[] DateHourItems;
    private int DateHourIndex = -1;
    private string[] DateMinuteItems;
    private int DateMinuteIndex = -1;

    private string StationName1 = "";
    private string StationName2 = "";
    private string StationName3 = "";
    private bool Plane = true;
    private bool Shinkansen = true;
    private bool LimitedExpress = true;
    private bool Bus = true;
    private int SearchTypeIndex = 0;
    private string[] SearchTypeItems = { "出発時刻探索", "到着時刻探索", "終電探索", "始発探索" };

    private string Key = "";
    private string ApiUrl = "http://api.ekispert.com/";

    [@MenuItem("Window/駅すぱあと")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Ekispert));
    }

    void OnGUI()
    {
        GUILayout.Label("駅すぱあとWebサービス", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("探索種別", EditorStyles.label);
        SearchTypeIndex = EditorGUILayout.Popup(SearchTypeIndex, SearchTypeItems);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("日付", EditorStyles.label);
        int index;
        // 年
        ArrayList YearArray = new ArrayList();
        index = 0;
        for (int i = System.DateTime.Now.Year - 1; i <= System.DateTime.Now.Year + 1; i++)
        {
            if (DateYearIndex == -1 && i == System.DateTime.Now.Year)
            {
                DateYearIndex = index;
            }
            YearArray.Add(string.Format("{0}年", i));
            index++;
        }
        DateYearIndex = EditorGUILayout.Popup(DateYearIndex, (string[])YearArray.ToArray(typeof(string)));
        // 月
        index = 0;
        ArrayList MonthArray = new ArrayList();
        for (int i = 1; i <= 12; i++)
        {
            if (DateMonthIndex == -1 && i == System.DateTime.Now.Month)
            {
                DateMonthIndex = index;
            }
            MonthArray.Add(string.Format("{0}月", i));
            index++;
        }
        DateMonthIndex = EditorGUILayout.Popup(DateMonthIndex, (string[])MonthArray.ToArray(typeof(string)));
        // 日
        index = 0;
        ArrayList DayArray = new ArrayList();
        int LastDay = 31;
        if (DateMonthIndex + 1 == 4 || DateMonthIndex + 1 == 6 || DateMonthIndex + 1 == 9 || DateMonthIndex + 1 == 11)
        {
            LastDay = 30;
        }
        else if (DateMonthIndex + 1 == 2)
        {
            LastDay = 28;
            if ((System.DateTime.Now.Year - 1 + DateYearIndex) % 4 == 0)
            {
                if (!((System.DateTime.Now.Year - 1 + DateYearIndex) % 100 == 0 && (System.DateTime.Now.Year - 1 + DateYearIndex) % 400 != 0))
                {
                    LastDay = 29;
                }
            }
        }
        for (int i = 1; i <= LastDay; i++)
        {
            if (DateDayIndex == -1 && i == System.DateTime.Now.Day)
            {
                DateDayIndex = index;
            }
            DayArray.Add(string.Format("{0}日", i));
            index++;
        }
        DateDayIndex = EditorGUILayout.Popup(DateDayIndex, (string[])DayArray.ToArray(typeof(string)));
        EditorGUILayout.EndHorizontal();

        if (SearchTypeIndex == 0 || SearchTypeIndex == 1)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("時刻", EditorStyles.label);
            // 時
            ArrayList HourArray = new ArrayList();
            for (int i = 0; i <= 23; i++)
            {
                if (DateHourIndex == -1 && i == System.DateTime.Now.Hour)
                {
                    DateHourIndex = i;
                }
                HourArray.Add(string.Format("{0}時", i));
            }
            DateHourIndex = EditorGUILayout.Popup(DateHourIndex, (string[])HourArray.ToArray(typeof(string)));
            // 分
            ArrayList MinuteArray = new ArrayList();
            for (int i = 0; i <= 59; i++)
            {
                if (DateMinuteIndex == -1 && i == System.DateTime.Now.Minute)
                {
                    DateMinuteIndex = i;
                }
                MinuteArray.Add(string.Format("{00}分", i));
            }
            DateMinuteIndex = EditorGUILayout.Popup(DateMinuteIndex, (string[])MinuteArray.ToArray(typeof(string)));
            EditorGUILayout.EndHorizontal();
        }

        StationName1 = EditorGUILayout.TextField("出発地", StationName1);
        StationName2 = EditorGUILayout.TextField("目的地", StationName2);
        StationName3 = EditorGUILayout.TextField("経由地", StationName3);

        Plane = EditorGUILayout.Toggle("飛行機", Plane);
        Shinkansen = EditorGUILayout.Toggle("新幹線", Shinkansen);
        LimitedExpress = EditorGUILayout.Toggle("特急", LimitedExpress);
        Bus = EditorGUILayout.Toggle("バス", Bus);

        Key = EditorGUILayout.TextField("アクセスキー", Key);

        if (Key == "")
        {
            EditorGUILayout.HelpBox("アクセスキーを入力してください", MessageType.Warning);
        }
        else
        {
            if (StationName1 == "" || StationName2 == "")
            {
                EditorGUILayout.HelpBox("出発地と目的地を入力して探索を行ってください", MessageType.Info);
            }
            else
            {
                if (GUILayout.Button("探索結果を表示", EditorStyles.miniButton))
                {
                    if (StationName1 != "" && StationName2 != "")
                    {
                        string html = GetHtml(getEWSUrl());

                        XDocument document = Parse(html);



                        //TODO : 駅名の取得、路線(仙山線等)の取得、完成後UIとのマージ

                        //全体的にごり押し

                        //経路①
                        IEnumerable<XElement> getInfo = document.XPathSelectElements(@"//div[@id='tabs_color']/div[@class='candidate_list']/table[@class='candidate_list_table tabs_content']/tr[1]/td[3]/p[@class='candidate_list_txt']");
                        routeInfo routeA = new routeInfo();
                        routeA.RName = "経路1";
                        routeA = GetElement(getInfo);
                        //getInfo = document.XPathSelectElements(@"//*[id='route01']//td[@id='route_start_txt']");
                        GetStation(getInfo, routeA);
                        if (routeA.TCount != 0)
                        {
                            getInfo = document.XPathSelectElements(@"//*[@id='route01']/div/table[2]/tr[3]/td[4]/div");
                            GetTLine(getInfo, routeA);
                            getInfo = document.XPathSelectElements(@"//*[@id='route01']/div/table[2]/tr[3]/td[1]");
                            GetTransTime(getInfo, routeA);
                        }
                        //*[@id="route01"]/div/table[2]/tbody/tr[1]/td[2]
                        //*[@id="route01"]/div/table[2]/tbody/tr[3]/td[3]
                        //*[@id="route01"]/div/table[2]/tbody/tr[5]/td[2]

                        //経路②
                        getInfo = document.XPathSelectElements(@"//div[@class='candidate_list']/table[@class='candidate_list_table tabs_content']/tr[2]/td[3]/p[@class='candidate_list_txt']");
                        routeInfo routeB = new routeInfo();
                        routeB.RName = "経路2";
                        routeB = GetElement(getInfo);
                        if (routeB.TCount != 0)
                        {
                            getInfo = document.XPathSelectElements(@"//*[@id='route02']/div/table[2]/tr[3]/td[4]/div");
                            GetTLine(getInfo, routeB);
                            getInfo = document.XPathSelectElements(@"//*[@id='route02']/div/table[2]/tr[3]/td[1]");
                            GetTransTime(getInfo, routeB);
                        }

                        //経路③
                        getInfo = document.XPathSelectElements(@"//div[@class='candidate_list']/table[@class='candidate_list_table tabs_content']/tr[3]/td[3]/p[@class='candidate_list_txt']");
                        routeInfo routeC = new routeInfo();
                        routeC.RName = "経路3";
                        routeC = GetElement(getInfo);
                        if (routeC.TCount != 0)
                        {
                            getInfo = document.XPathSelectElements(@"//*[@id='route03']/div/table[2]/tr[3]/td[4]/div");
                            GetTLine(getInfo, routeC);
                            getInfo = document.XPathSelectElements(@"//*[@id='route03']/div/table[2]/tr[3]/td[1]");
                            GetTransTime(getInfo, routeC);
                        }

                        //経路④
                        getInfo = document.XPathSelectElements(@"//div[@class='candidate_list']/table[@class='candidate_list_table tabs_content']/tr[4]/td[3]/p[@class='candidate_list_txt']");
                        routeInfo routeD = new routeInfo();
                        routeD.RName = "経路4";
                        routeD = GetElement(getInfo);
                        if (routeD.TCount != 0)
                        {
                            getInfo = document.XPathSelectElements(@"//*[@id='route04']/div/table[2]/tr[3]/td[4]/div");
                            GetTLine(getInfo, routeD);
                            getInfo = document.XPathSelectElements(@"//*[@id='route04']/div/table[2]/tr[3]/td[1]");
                            GetTransTime(getInfo, routeD);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// HTML -> XML　に変換
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static XDocument Parse(string content)
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
    /// <param name="url"></param>
    /// <returns></returns>
    public string GetHtml(string url)
    {
        Debug.Log("Test Log!");
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
        Debug.Log(html);
        return html;
        

    }

    /// <summary>
    /// 時間、金額、乗り換え回数取得
    /// </summary>
    /// <param name="Info"></param>
    /// <param name="rI"></param>
    private static routeInfo GetElement(IEnumerable<XElement> Info)
    {
        int i = 0;
        routeInfo rI = new routeInfo();
        foreach (XElement el in Info)
        {
            switch (i)
            {
                case 0:
                    rI.Time = el.Value;
                    //Debug.Log(rI.Time);
                    rI.LeftTime = rI.Time.Remove(5);
                    rI.ArriveTime = rI.Time.Remove(13).Remove(0,8);
                    Debug.Log(rI.LeftTime);
                    Debug.Log(rI.ArriveTime);
                    break;
                case 1:
                    rI.TCount = int.Parse(el.Value.Remove(el.Value.Length - 1).Remove(0, 3));
                    Debug.Log(rI.TCount);
                    Array.Resize(ref rI.TransTime, 2+2*rI.TCount);
                    break;
                case 2:
                    rI.Money = int.Parse(el.Value.Remove(el.Value.Length - 1).Remove(0, 3));
                    Debug.Log(rI.Money);
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
    private static void GetTLine(IEnumerable<XElement> Info, routeInfo rI)
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
    /// 乗り換え時間の取得
    /// </summary>
    /// <param name="Info"></param>
    /// <param name="rI"></param>
    private static void GetTransTime(IEnumerable<XElement> Info, routeInfo rI)
    {
        
        string tmp = "";
        foreach (XElement el in Info)
        {

            //Debug.Log(el.Value.Replace(" ", "").Replace("　", "").Replace("\n", ""));
            tmp = el.Value.Replace(" ", "").Replace("　", "").Replace("\n", "");

        }

        rI.TransTime[0] = tmp.Remove(7).Remove(0,2);
        Debug.Log(rI.TransTime[0]);
        rI.TransTime[1] = tmp.Remove(0, 9);
        Debug.Log(rI.TransTime[1]);
        //到着15:40出発16:00
    }

    /// <summary>
    /// 出発駅、到着駅取得
    /// </summary>
    /// <param name=""></param>
    /// <param name="rI"></param>
    private static void GetStation(IEnumerable<XElement> Info, routeInfo rI)
    {
        foreach(XElement el in Info)
        {
            Debug.Log(el.Value);
        }
    }

    private string getEWSUrl()
    {
        string url = ApiUrl + "v1/xml/search/course/light";
        url += "?key=" + Key;
        url += "&from=" + WWW.EscapeURL(StationName1);
        url += "&to=" + WWW.EscapeURL(StationName2);
        if (StationName3 != "")
        {
            url += "&via=" + WWW.EscapeURL(StationName3);
        }
        url += "&date=" + (System.DateTime.Now.Year - 1 + DateYearIndex) + (DateMonthIndex < 9 ? "0" : "") + (DateMonthIndex + 1) + (DateDayIndex < 9 ? "0" : "") + (DateDayIndex + 1);
        switch (SearchTypeIndex)
        {
            case 0:
                url += "&searchType=departure";
                url += "&time=" + (DateHourIndex < 10 ? "0" : "") + DateHourIndex + (DateMinuteIndex < 10 ? "0" : "") + DateMinuteIndex;
                break;
            case 1:
                url += "&searchType=arrival";
                url += "&time=" + (DateHourIndex < 10 ? "0" : "") + DateHourIndex + (DateMinuteIndex < 10 ? "0" : "") + DateMinuteIndex;
                break;
            case 2:
                url += "&searchType=lastTrain";
                break;
            case 3:
                url += "&searchType=firstTrain";
                break;
        }
        url += "&plane=" + (Plane ? "true" : "false");
        url += "&shinkansen=" + (Shinkansen ? "true" : "false");
        url += "&limitedExpress=" + (LimitedExpress ? "true" : "false");
        url += "&bus=" + (Bus ? "true" : "false");
        url += "&redirect=true";
        return url;
    }

}
