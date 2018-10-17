using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderBehavior : MonoBehaviour
{
    public string SettingName;
    public Text TextPercent;
    
    private void Start()
    {
        GetComponent<Slider>().onValueChanged.AddListener(
            Slider_OnValueChanged);
    }

    public void Slider_OnValueChanged(float value)
    {
        TextPercent.text = (int)(value * 100) + "%";

        if (!string.IsNullOrEmpty(SettingName))
        {
            var setting = Setting.Instance.Get();
            var prop = typeof(HoloGuide.Setting).GetProperty(SettingName);
            prop.SetValue(setting, value);
            Setting.Instance.Set(setting);
        }
    }
}
