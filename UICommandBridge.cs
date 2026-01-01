using UnityEngine;

namespace HB.UI
{
    /// <summary>
    /// UI -> Core bridge. Put your core command receiver in 'commandReceiver' (must implement ICommandSender).
    /// </summary>
    public class UICommandBridge : MonoBehaviour, ICommandSender
    {
        public MonoBehaviour commandReceiver; // implements ICommandSender
        private ICommandSender _sink;

        private void Awake() => _sink = commandReceiver as ICommandSender;

        public void Send(ActionCommand cmd)
        {
            if (_sink != null) _sink.Send(cmd);
            else Debug.Log($"[UICommandBridge] {cmd}");
        }
    }
}
