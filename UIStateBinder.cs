using UnityEngine;

namespace HB.UI
{
    /// <summary>
    /// Latest Recruit snapshot (for rendering + validating board-slot emptiness on drop).
    /// Core should call ApplyState after delta_state.
    /// </summary>
    public class UIStateBinder : MonoBehaviour
    {
        public static UIStateBinder Instance { get; private set; }

        public RecruitStateDTO Current { get; private set; }

        public RecruitScreen screen;
        public LogPanel logPanel;

        private void Awake() => Instance = this;

        public void ApplyState(RecruitStateDTO state)
        {
            Current = state;
            if (screen) screen.Render(state);
        }

        public void OnServerEvent(ServerEvent evt)
        {
            if (logPanel) logPanel.Append(evt.step, evt.log);
        }
    }
}
