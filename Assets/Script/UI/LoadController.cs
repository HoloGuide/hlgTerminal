using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadController : MonoBehaviour
{
    public Text IP;

    private void Start()
    {
        IP.text = IPAddress.Parse(Network.player.ipAddress).ToString();

        var ets = new EkispertTransitSearch();
        var routes = ets.SearchDeperture(System.DateTime.Now, "長町", "愛子");

        Debug.LogFormat("{0}({1}) -> {2}({3})", routes[0].LeftSta, routes[0].LeftTime, routes[0].ArriveSta, routes[0].ArriveTime);

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            // Searchシーンへ遷移 (For Debug)
            SceneManager.LoadScene("Search");
        }

    }
}
