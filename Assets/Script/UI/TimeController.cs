using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    public List<GameObject> Blocks;

    private void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            var r = AppManager.Instance.Routes[i];

            var text = string.Format("{0}({1}) → {2}({3})", r.LeftSta, r.LeftTime, r.ArriveSta, r.ArriveTime);
            Blocks[i].GetComponentInChildren<Text>().text = text;
        }
    }

    public void BtnBack_OnClicked()
    {
        SceneManager.LoadScene("Search");
    }

    public void ListContent_OnClicked(int selectedIdx)
    {
        AppManager.Instance.SelectedRouteIndex = selectedIdx;
        SceneManager.LoadScene("Route");
    }

    public void BtnSetting_OnClicked()
    {
        AppManager.Instance.MoveToSetting();
    }
}
