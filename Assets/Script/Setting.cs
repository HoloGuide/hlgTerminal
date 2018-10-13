using UnityEngine;
using Newtonsoft.Json;

public class Setting : Singleton<Setting>
{
    public void Start()
    {
        m_setting = new HoloGuide.Setting();

        // Load Player Prefs.
        m_setting.dummy = PlayerPrefs.GetString(HoloGuide.PlayerPrefsKey.dummy);
    }

    public void Set(HoloGuide.Setting set)
    {
        m_setting = set;

        // Set Player Prefs.
        PlayerPrefs.SetString(HoloGuide.PlayerPrefsKey.dummy, set.dummy);
        PlayerPrefs.Save();

        // Convert to json and send broadcast.
        var json = JsonConvert.SerializeObject(m_setting);
        WebService.Instance.SendBroadcast(json);
    }

    public HoloGuide.Setting Get()
    {
        return m_setting;
    }

    private HoloGuide.Setting m_setting { get; set; }
}
