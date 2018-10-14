using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpeInfoController : MonoBehaviour
{
    public Image StatusIcon;
    public Text TextCaption;
    public InputField InputFieldLine;

    public Sprite StatusOK;
    public Sprite StatusInfo;

    public void BtnBack_OnClicked()
    {
        SceneManager.LoadScene("Route");
    }

    public void BtnSetting_OnClicked()
    {
        AppManager.Instance.MoveToSetting();
    }

    public void BtnSearch_OnClicked()
    {
        var opeInfo = this.GetComponent<GetOpeInfo>();
        opeInfo.GetOperateInfo(InputFieldLine.text, OnGetOpeInfoFinished);
    }

    private void OnGetOpeInfoFinished(string result)
    {
        TextCaption.text = result;

        if (result == "現在､事故･遅延に関する情報はありません。")
        {
            // 通常運行
            StatusIcon.sprite = StatusOK;
            StatusIcon.color = Color.green;
        }
        else
        {
            // 遅延等有り
            StatusIcon.sprite = StatusInfo;

            var hexColor = "#FFB800";
            var col = Color.white;
            ColorUtility.TryParseHtmlString(hexColor, out col);
            StatusIcon.color = col;
        }

    }
}
