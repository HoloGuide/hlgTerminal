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

    private static string prevLog = "";
    private static List<string> logs = new List<string>();

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
        if (!AppManager.Instance.OutputDebugLog) return;
        if (Label == null) return;

        if (!Label.gameObject.activeInHierarchy)
        {
            Label.gameObject.SetActive(true);
        }

        Label.text = prevLog;

        foreach (var log in logs)
        {
            if (Label.text != "" && MultiLine)
            {
                string text = Label.text;

                text = (log.Substring(0, Math.Min(log.Length, MaxChInLine))) + Environment.NewLine + text;

                var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                if (lines.Count() > MaxLine)
                {
                    text = string.Join(Environment.NewLine, lines.Where(x => x != lines.Last()));
                }

                Label.text = text;
            }
            else
            {
                Label.text = log;
            }
        }

        prevLog = Label.text;

        if (logs.Count > 0)
        {
            logs.Clear();
        }
    }

    private void HandleLog(string logText, string stackTrace, LogType type)
    {
        logs.Add(logText);
    }
}