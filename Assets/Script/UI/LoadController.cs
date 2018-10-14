using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
