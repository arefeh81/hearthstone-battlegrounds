using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HB.UI
{
    /// <summary>
    /// Drop slot on Board/Hand/Shop with validity highlight.
    /// Drag & Drop must light drop targets; if slot is full UI shows red and emits BoardFull feedback. fileciteturn7file0
    /// </summary>
    public class CardSlotView : MonoBehaviour
    {
        [Header("Slot")]
        public Image highlight;
        public Color validColor = new Color(0.2f, 0.9f, 0.2f, 0.35f);
        public Color invalidColor = new Color(0.9f, 0.2f, 0.2f, 0.35f);

        [Header("Optional")]
        public TMP_Text slotLabel;

        public ZoneRef SlotRef { get; private set; }

        public void Bind(ZoneType zone, int index)
        {
            SlotRef = new ZoneRef { zone = zone, index = index };
            if (slotLabel) slotLabel.text = $"{zone}:{index}";
            SetHighlight(false, true);
        }

        public void SetHighlight(bool on, bool valid)
        {
            if (!highlight) return;
            highlight.gameObject.SetActive(on);
            highlight.color = valid ? validColor : invalidColor;
        }
    }
}
