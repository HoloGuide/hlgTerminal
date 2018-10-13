using System.Collections;
using UnityEngine;
using System.Xml.Linq;
using System.IO;
using Sgml;

namespace Assets.Script
{
    public class getStation : MonoBehaviour
    {
        public float IntervalSeconds = 1.0f;
        public LocationServiceStatus Status;
        public LocationInfo Location;

        public void getPos()
        {
            string url;
            string html;
            XDocument doc;
            Start();
            url = "http://map.simpleapi.net/stationapi" + "?x=" + Location.longitude + "&y=" + Location.latitude;
            html = GetHtml(url);
            doc = Parse(html);
            Debug.Log(html);

            var staName = doc.Element("result").Element("station").Element("name");
            Debug.Log(staName.Value);
        }


        public static XDocument Parse(string content)
        {
            using (var reader = new StringReader(content))
            using (var sgmlReader = new SgmlReader { DocType = "HTML", CaseFolding = CaseFolding.ToLower, IgnoreDtd = true, InputStream = reader })
            {
                return XDocument.Load(sgmlReader);
            }
        }


        IEnumerator Start()
        {
            while(true)
            {
                Status = Input.location.status;
                if(Input.location.isEnabledByUser)
                {
                    switch(Status)
                    {
                        case LocationServiceStatus.Stopped:
                            Input.location.Start();
                            break;

                        case LocationServiceStatus.Running:
                            Location = Input.location.lastData;
                            //Debug.Log(Location);
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    Debug.Log("location is disabled by user!");
                }

                yield return new WaitForSeconds(IntervalSeconds);
            }

            
        }

        public string GetHtml(string url)
        {
            Debug.Log("Test Log!");
            string html;
            bool isDone = false;
            WWW www = new WWW(url);

            while (true)
            {
                if (isDone == false && www.isDone)
                {
                    html = www.text;

                    break;
                }
            }
            Debug.Log(html);
            return html;


        }
    }
}
