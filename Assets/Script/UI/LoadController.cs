using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadController : MonoBehaviour
{
    public Text IP;

    private bool loaded = false;

    private void Start()
    {
        IP.text = IPAddress.Parse(Network.player.ipAddress).ToString();
    }

    private void Update()
    {
        if (!loaded && AppManager.Instance.Connected)
        {
            // Routeシーンに遷移
            SceneManager.LoadScene("Route");

            loaded = true;
        }
    }
}
