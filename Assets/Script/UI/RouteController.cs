using UnityEngine;
using UnityEngine.SceneManagement;

public class RouteController : MonoBehaviour
{
    public GameObject Panel;

    public void BtnBack_OnClicked()
    {
        SceneManager.LoadScene("Time");
    }

    public void BtnStartNav_OnClicked()
    {
        Panel.SetActive(true);
    }

    public void BtnDialog_OnClicked(bool yes)
    {
        if (yes)
        {
            // TODO: 案内開始
        }
        else
        {
            Panel.SetActive(false);
        }
    }

    public void BtnInfo_OnClicked()
    {
        // TODO: 運行情報シーンへの遷移
    }

    public void BtnSetting_OnClicked()
    {
        AppManager.Instance.MoveToSetting();
    }
}
