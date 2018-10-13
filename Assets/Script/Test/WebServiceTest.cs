using System.Net;
using UnityEngine;

using Newtonsoft.Json;

public class WebServiceTest : MonoBehaviour
{
    public UnityEngine.UI.Text IP;

    private void Start()
    {
        IP.text = IPAddress.Parse(Network.player.ipAddress).ToString();

        var set = new HoloGuide.Setting();
        set.dummy = "HoloGuide";

        WebService.Instance.OnLocationChanged += (message) =>
        {
            Debug.Log("LocationChanged: " + message);
        };

        WebService.Instance.OnReceived += (message) =>
        {
            Debug.Log("Received: " + message);
            // WebService.Instance.SendBroadcast("broadcast: " + message);

            JsonParser.Instance.ParseJson(message);
        };

        WebService.Instance.OnConnected += (addr) =>
        {
            Debug.Log("New connection : " + addr.Substring(1).Split(':')[0]);

            // Androidは設定情報を送信後、待機画面から抜ける。
            var json = JsonConvert.SerializeObject(set);
            WebService.Instance.SendBroadcast(json);

        };

        WebService.Instance.OnDisconnected += (addr) =>
        {
            Debug.Log("Lost connection : " + addr.Substring(1).Split(':')[0]);

            // 待機画面に戻る

        };

    }

    public void BtnStart_OnClick()
    {
        Debug.Log("Starting service...");
        WebService.Instance.StartService();
    }

    public void BtnStop_OnClick()
    {
        Debug.Log("Stopping service...");
        WebService.Instance.StopService();
    }

}
