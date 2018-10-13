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

    private List<string> logs = new List<string>();

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
        if (logs.Count == 0) return;

        if (!Label.gameObject.activeInHierarchy)
        {
            Label.gameObject.SetActive(true);
        }

        foreach (var log in logs)
        {
            if (Label.text != "" && MultiLine)
            {
                string text = Label.text;

                text = log + Environment.NewLine + text;

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


        logs.Clear();
    }

    private void HandleLog(string logText, string stackTrace, LogType type)
    {
        logs.Add(logText);
    }
}