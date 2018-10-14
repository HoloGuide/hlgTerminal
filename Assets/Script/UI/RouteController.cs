using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class RouteController : MonoBehaviour
{
    public Text TextRoute;
    public GameObject Panel;

    private void Start()
    {
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
        SceneManager.LoadScene("OpeInfo");
    }

    public void BtnSetting_OnClicked()
    {
        AppManager.Instance.MoveToSetting();
    }
}
