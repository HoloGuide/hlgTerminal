using Sgml;
using System.Collections;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class GetStation /* : MonoBehaviour */
{
    /// <summary>
    /// 最寄り駅の名前を返します。
    /// </summary>
    public string GetStationName(double lng, double lat)
    {
        string url;
        string html;
        XDocument doc;
        // Start();
        // url = "http://map.simpleapi.net/stationapi" + "?x=" + Location.longitude + "&y=" + Location.latitude;
        url = "http://map.simpleapi.net/stationapi" + "?x=" + lng + "&y=" + lat;
        html = GetHtml(url);
        doc = Parse(html);
        Debug.Log(html);

        var staName = doc.Element("result").Element("station").Element("name");
        Debug.Log(staName.Value);

        return staName.Value;
    }

    public static XDocument Parse(string content)
    {
        using (var reader = new StringReader(content))
        using (var sgmlReader = new SgmlReader { DocType = "HTML", CaseFolding = CaseFolding.ToLower, IgnoreDtd = true, InputStream = reader })
        {
            return XDocument.Load(sgmlReader);
        }
    }

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
}
