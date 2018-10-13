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
