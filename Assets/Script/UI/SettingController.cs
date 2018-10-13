using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public GameObject PanelGeneral;
    public GameObject PanelNotification;
    public GameObject PanelDebug;

    public Text Text_Volume;
    public Slider Slider_Volume;
    public InputField InputField_Dummy;

    private Category currentCategory;
    private float tappedStarted = 0;

    private void Start()
    {
        currentCategory = Category.General;

        InputField_Dummy.text = Setting.Instance.Get().dummy;
    }

    public void InputField_OnEndEdit()
    {
        var setting = Setting.Instance.Get();
        setting.dummy = InputField_Dummy.text;

        Setting.Instance.Set(setting);
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
        } else
        {
            // Debug -> Notification
            currentCategory = Category.Notification;
            PanelDebug.SetActive(false);
            PanelNotification.SetActive(true);
        }
    }

    public void BtnNotification_OnClicked()
    {
        // General -> Notification
        currentCategory = Category.Notification;
        PanelGeneral.SetActive(false);
        PanelNotification.SetActive(true);
    }

    public void NotificationText_PointerDown()
    {
        tappedStarted = Time.time;
    }

    public void NotificationText_PointerUp()
    {
        if (Time.time - tappedStarted > 5.0f)
        {
            // Notification -> Debug
            currentCategory = Category.Debug;
            PanelNotification.SetActive(false);
            PanelDebug.SetActive(true);
        }
    }

    public void SliderVolume_OnValueChanged()
    {
        var vol = (int)(Slider_Volume.value * 100);
        Text_Volume.text = vol + "%";
    }

    enum Category
    {
        General, // 全般
        Notification, // 通知
        Debug // デバッグ
    }

}
