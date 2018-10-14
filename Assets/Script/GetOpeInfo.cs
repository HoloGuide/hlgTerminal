using Sgml;
using System.Collections;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetOpeInfo  : MonoBehaviour
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

    // 運行情報取得関数
    private IEnumerator _GetOperateInfo()
    {

        Hashtable Data = new Hashtable()
        {
            ["仙山線"] = "https://transit.yahoo.co.jp/traininfo/detail/446/0/",
            ["東北本線"] = "https://transit.yahoo.co.jp/traininfo/detail/16/16/",
            ["仙石線"] = "https://transit.yahoo.co.jp/traininfo/detail/19/0/",
            ["常磐線"] = "https://transit.yahoo.co.jp/traininfo/detail/443/0/"
        };

        // 1.UnityWebRequestを生成
        UnityWebRequest request = UnityWebRequest.Get((string)Data[m_lineName]);

        // 2.SendWebRequestを実行し、送受信開始
        yield return request.SendWebRequest();

        // 3.isNetworkErrorとisHttpErrorでエラー判定
        if (request.isHttpError || request.isNetworkError)
        {
            // 4.エラー確認
            Debug.Log(request.error);
        }
        else
        {
            // 4.結果確認
            Debug.Log(request.downloadHandler.text);
        }

        XDocument document = Parse(request.downloadHandler.text);

        // 運行状況の取得
        XElement Operate_info = document.XPathSelectElement("//meta[@property='og:description']");

        // タグを外す操作
        string info = Operate_info.ToString();

        // 前部分
        string OutputData = info.Remove(0, 41);

        // 後ろ部分
        OutputData = OutputData.Remove((OutputData.Length - 43));

        // 出力
        // Text.text = OutputData;
        m_callback(OutputData);
    }

    // HTMLをXMLに変換
    public static XDocument Parse(string content)
    {
        using (var reader = new StringReader(content))
        using (var sgmlReader = new SgmlReader { DocType = "HTML", CaseFolding = CaseFolding.ToLower, IgnoreDtd = true, InputStream = reader })
        {
            return XDocument.Load(sgmlReader);
        }
    }

}

