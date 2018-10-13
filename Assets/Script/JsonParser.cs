using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

public class JsonParser : Singleton<JsonParser>
{
    public void ParseJson(string json)
    {
        var jObject = JObject.Parse(json);

        switch (jObject["type"].Value<string>())
        {
            case "state":
                // 案内状況
                var state = JsonConvert.DeserializeObject<HoloGuide.State>(json);

                if (state.navStarted && !state.isNavigating)
                {
                    // 案内終了
                    Debug.Log("--- 案内終了 ---");
                    AppManager.Instance.NavigationFinished();
                }

                break;
            default:
                break;
        }

    }
}
