using UnityEngine;

public class WebServiceTest : MonoBehaviour
{
    private void Start()
    {
        WebService.Instance.OnLocationChanged += (message) =>
        {
            Debug.Log("LocationChanged: " + message);
        };

        WebService.Instance.OnReceived += (message) =>
        {
            Debug.Log("Received: " + message);
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
