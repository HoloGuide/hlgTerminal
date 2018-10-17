using System;

namespace HoloGuide
{
    [Serializable]
    public class Route
    {
        public string type { get; set; }
        public string filename { get; set; }
        public int start { get; set; }
        public int goal { get; set; }
    }

    [Serializable]
    public class State
    {
        public string type = "state";
        public bool isNavigating;
    }

    [Serializable]
    public class Setting
    {
        public string type = "setting";
        public string dummy;
    }

    [Serializable]
    public class Location
    {
        public string type = "location";
        public double lat;
        public double lng;
        public long updated;
    }

    public static class PlayerPrefsKey
    {
        public const string dummy = "hlg_dummy_value";
    }

}