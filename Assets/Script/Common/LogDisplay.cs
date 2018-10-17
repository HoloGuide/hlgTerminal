using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LogDisplay : MonoBehaviour
{
    public Text Label = null;
    public bool MultiLine = false;
    public int MaxLine = 15;
    public int MaxChInLine = 50;

    private static string Log = "";
    private static Queue<string> logQueue = new Queue<string>();

    private void Awake()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    private void OnDestroy()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    private void Update()
    {
        if (Label == null) return;
        if (AppManager.Instance == null) return;
        if (!AppManager.Instance.OutputDebugLog) return;

        if (!Label.gameObject.activeInHierarchy)
        {
            Label.gameObject.SetActive(true);
        }

        while(logQueue.Count > 0)
        {
            var _log = logQueue.Dequeue();

            if (Log == "")
            {
                Log = _log;
                continue;
            }

            Log = (_log.Substring(0, Math.Min(_log.Length, MaxChInLine))) + Environment.NewLine + Log;
        }

        var lines = Log.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        if (lines.Count > MaxLine)
        {
            for(int i = MaxLine; i < lines.Count; i++)
            {
                lines.RemoveAt(i);
            }
            Log = string.Join(Environment.NewLine, lines);
        }

        Label.text = Log;
    }

    private void HandleLog(string logText, string stackTrace, LogType type)
    {
        logQueue.Enqueue(logText);
    }
}