using Sgml;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using UnityEngine;
using UnityEngine.Networking;

public class GetOpeInfo : MonoBehaviour
{
    //// GUI変数の宣言
    //public Text Text;
    //public InputField inputField;

    //// Buttonが押された時の処理
    //public void OnClick()
    //{
    //    StartCoroutine(GetOperateInfo());
    //}

    private string m_lineName;
    private System.Action<string> m_callback;

    public void GetOperateInfo(string lineName, System.Action<string> callback)
    {
        m_lineName = lineName;
        m_callback = callback;

        StartCoroutine("_GetOperateInfo");
    }

    private IEnumerator _GetOperateInfo()
    {
        var Data = new Hashtable()
        {
            ["仙山線"] = "https://transit.yahoo.co.jp/traininfo/detail/446/0/",
            ["東北本線"] = "https://transit.yahoo.co.jp/traininfo/detail/16/16/",
            ["仙石線"] = "https://transit.yahoo.co.jp/traininfo/detail/19/0/",
            ["常磐線"] = "https://transit.yahoo.co.jp/traininfo/detail/443/0/"
        };

        string html;
        using (var req = UnityWebRequest.Get((string)Data[m_lineName]))
        {
            req.SetRequestHeader("User-Agent", "");
            yield return req.SendWebRequest();

            html = req.downloadHandler.text;
        }

        XDocument document = Parse(html);

        // 運行状況の取得
        XElement Operate_info = document.XPathSelectElement("//*[@id='main']/section/div/dl/dd/p");
        if (Operate_info == null)
        {
            Operate_info = document.XPathSelectElement("//*[@id='mdServiceStatus']/dl/dd/p");
        }

        // タグを外す操作
        string info = Regex.Replace(Operate_info.ToString(), "<[^>]*?>", ""); ;

        // 出力
        // Debug.Log(info);
        // Text.text = OutputData;
        m_callback(info);
    }

    // HTMLをXMLに変換
    private XDocument Parse(string content)
    {
        using (var reader = new StringReader(content))
        using (var sgmlReader = new SgmlReader { DocType = "HTML", CaseFolding = CaseFolding.ToLower, IgnoreDtd = true, InputStream = reader})
        {
            return XDocument.Load(sgmlReader);
        }
    }

}

