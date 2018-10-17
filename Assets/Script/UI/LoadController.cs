using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadController : MonoBehaviour
{
    public Text IP;

    private bool loaded = false;

    private int m_clickCount = 0;

    public void Text_OnClicked()
    {
        m_clickCount++;

        if (m_clickCount >= 5)
        {
            SceneManager.LoadScene("Route");
        }
    }

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
