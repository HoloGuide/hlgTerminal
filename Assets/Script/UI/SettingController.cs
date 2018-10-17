using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public GameObject PanelGeneral;
    public GameObject PanelNotification;

    public Text Text_Volume;
    public Slider Slider_Volume;

    private Category currentCategory;

    private void Start()
    {
        currentCategory = Category.General;
    }

    public void BtnBack_OnClicked()
    {
        if (currentCategory == Category.General)
        {
            // General -> 設定ボタンが押されたシーン
            AppManager.Instance.BackFromSetting();
        } else if (currentCategory == Category.Notification)
        {
            // Notification -> General
            currentCategory = Category.General;
            PanelNotification.SetActive(false);
            PanelGeneral.SetActive(true);
        }
    }

    public void BtnNotification_OnClicked()
    {
        // General -> Notification
        currentCategory = Category.Notification;
        PanelGeneral.SetActive(false);
        PanelNotification.SetActive(true);
    }

    enum Category
    {
        General, // 全般
        Notification // 通知
    }

}
