using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HB.UI
{
    /// <summary>
    /// Discover / Choose-one popup. Must pause and send action back. fileciteturn7file10
    /// </summary>
    public class PopupChoicePanel : MonoBehaviour
    {
        public GameObject root;
        public TMP_Text titleText;

        public List<Button> optionButtons = new();
        public List<TMP_Text> optionLabels = new();

        public UICommandBridge commandBridge;

        private string _choiceUuid = "";
        private string _action = "DISCOVER_CHOICE";

        private void Awake()
        {
            if (root) root.SetActive(false);
            for (int i = 0; i < optionButtons.Count; i++)
            {
                int idx = i;
                optionButtons[i].onClick.AddListener(() => Choose(idx));
            }
        }

        public void Show(string title, string action, string choiceUuid, List<CardDTO> options)
        {
            _action = action;
            _choiceUuid = choiceUuid;

            if (titleText) titleText.text = title;
            if (root) root.SetActive(true);

            for (int i = 0; i < optionButtons.Count; i++)
            {
                bool on = options != null && i < options.Count;
                optionButtons[i].gameObject.SetActive(on);
                if (on && i < optionLabels.Count)
                    optionLabels[i].text = options[i].name;
            }

            Time.timeScale = 0f;
        }

        public void Hide()
        {
            Time.timeScale = 1f;
            if (root) root.SetActive(false);
        }

        private void Choose(int optionIndex)
        {
            commandBridge?.Send(new ActionCommand
            {
                action = _action,
                choice_uuid = _choiceUuid,
                option_index = optionIndex
            });
            Hide();
        }
    }
}
