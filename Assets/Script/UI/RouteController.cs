using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RouteController : MonoBehaviour
{
    public Text BtnNavText;
    public Text TextRoute;
    public GameObject Panel;
    public Text PanelText;
    public Button BtnBack;

    private void Start()
    {
        // ルートの表示
        var r = AppManager.Instance.Routes[AppManager.Instance.SelectedRouteIndex];

        var text = "";
        text += r.LeftTime + " : " + r.LeftSta + "\n";
        text += "↓\n";
        if (r.TCount != 0)
        {
            var tl0 = getTransitLineNum(r.TLine[0]);
            var tl1 = getTransitLineNum(r.TLine[1]);

            text += r.TransTime[0] + " : " + r.PassSta + " " + (tl0 > 0 ? tl0 + "番線" : "") + "\n";
            text += r.TransTime[1] + " : " + r.PassSta + " " + (tl1 > 0 ? tl1 + "番線" : "") + "\n";
            text += "↓\n";
        }
        text += r.ArriveTime + " : " + r.ArriveSta + "\n";

        TextRoute.text = text;

        BtnBack.interactable = AppManager.Instance.NavigationState != NavState.Navigating;
        // 案内中なら、案内開始ボタンを案内中止ボタンにする
        if (AppManager.Instance.NavigationState == NavState.Navigating)
        {
            BtnNavText.text = "案内中止";
        }

    }

    private int getTransitLineNum(string tline)
    {
        var re = new Regex(@"[^0-9]");
        int ret;
        if (!int.TryParse(re.Replace(tline, ""), out ret))
        {
            return -1;
        }
        return ret;
    }

    public void BtnBack_OnClicked()
    {
        SceneManager.LoadScene("Time");
    }

    public void BtnNav_OnClicked()
    {
        if (AppManager.Instance.NavigationState == NavState.Navigating)
        {
            PanelText.text = "案内を中止しますか？";
        }

        Panel.SetActive(true);
    }

    public void BtnDialog_OnClicked(bool yes)
    {
        if (yes)
        {
            if (AppManager.Instance.NavigationState == NavState.Navigating)
            {
                AppManager.Instance.NavigationState = NavState.Ready;
                SceneManager.LoadScene("Search");
            }
            else
            {
                BtnBack.interactable = false;
                AppManager.Instance.NavigationState = NavState.Navigating;
                SceneManager.LoadScene("Load");
            }
        }
        else
        {
            Panel.SetActive(false);
        }
    }

    public void BtnInfo_OnClicked()
    {
        SceneManager.LoadScene("OpeInfo");
    }

    public void BtnSetting_OnClicked()
    {
        AppManager.Instance.MoveToSetting();
    }
}
