using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HB.UI
{
    /// <summary>
    /// Log panel with time/step per event for debugging. fileciteturn7file11
    /// </summary>
    public class LogPanel : MonoBehaviour
    {
        public TMP_Text text;
        public ScrollRect scroll;
        public int maxLines = 200;

        private readonly System.Collections.Generic.Queue<string> _lines = new();

        public void Append(int step, string message)
        {
            Enqueue($"[{DateTime.Now:HH:mm:ss}] step={step}  {message}");
        }

        public void AppendError(string message)
        {
            Enqueue($"[{DateTime.Now:HH:mm:ss}] ERROR  {message}");
        }

        private void Enqueue(string line)
        {
            _lines.Enqueue(line);
            while (_lines.Count > maxLines) _lines.Dequeue();

            if (text) text.text = string.Join("\n", _lines);

            if (scroll)
            {
                Canvas.ForceUpdateCanvases();
                scroll.verticalNormalizedPosition = 0f;
            }
        }
    }
}
