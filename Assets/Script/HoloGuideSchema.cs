using System;

namespace HoloGuide
{
    [Serializable]
    public class State
    {
        public string type = "state";
        public bool navStarted;
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