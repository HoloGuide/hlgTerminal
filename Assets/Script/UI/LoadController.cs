using Sgml;
using System.Collections;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadController : MonoBehaviour
{
    public Text IP;

    private int clickedCount = 0;

    public void Logo_OnClicked()
    {
        clickedCount++;

        if (clickedCount >= 5)
        {
            AppManager.Instance.OutputDebugLog = true;
        }
    }

    private void Start()
    {
        IP.text = IPAddress.Parse(Network.player.ipAddress).ToString();
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
