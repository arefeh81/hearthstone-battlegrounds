using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HB.UI
{
    /// <summary>
    /// Drag manager:
    /// - highlights drop targets
    /// - constructs commands BUY/PLAY/MOVE
    /// Requirement mentions drag_manager sends messages like:
    /// {action:"PLAY", source_hand_slot:2, target_board_slot:4} and {action:"BUY", shop_slot:1}. fileciteturn7file5
    /// Also: if slot is full show red and BoardFull feedback. fileciteturn7file0
    /// </summary>
    public class DragService : MonoBehaviour
    {
        public static DragService Instance { get; private set; }

        [Header("UI")]
        public Image dragGhostImage; // optional ghost

        [Header("Dependencies")]
        public LogPanel logPanel;
        public UICommandBridge commandBridge;

        private CardView _dragCard;
        private CardSlotView _hoverSlot;
        private bool _hoverHandZone;

        private void Awake()
        {
            Instance = this;
            if (dragGhostImage) dragGhostImage.gameObject.SetActive(false);
        }

        public void BeginDrag(CardView card, PointerEventData ev)
        {
            _dragCard = card;
            if (dragGhostImage)
            {
                dragGhostImage.gameObject.SetActive(true);
                dragGhostImage.rectTransform.position = ev.position;
            }
        }

        public void OnDrag(PointerEventData ev)
        {
            if (_dragCard == null) return;
            if (dragGhostImage) dragGhostImage.rectTransform.position = ev.position;

            if (_hoverSlot != null)
            {
                bool valid = IsDropValid(_dragCard, _hoverSlot);
                _hoverSlot.SetHighlight(true, valid);
            }
        }

        public void EndDrag(PointerEventData ev)
        {
            ClearHover();
            HideGhost();
            _dragCard = null;
        }

        public void HoverTarget(CardSlotView slot, bool enter)
        {
            if (_dragCard == null) return;

            if (enter)
            {
                _hoverSlot = slot;
                bool valid = IsDropValid(_dragCard, slot);
                slot.SetHighlight(true, valid);
            }
            else if (_hoverSlot == slot)
            {
                slot.SetHighlight(false, true);
                _hoverSlot = null;
            }
        }

        public void HoverHandZone(bool enter) => _hoverHandZone = enter;

        public void DropOn(CardSlotView slot)
        {
            if (_dragCard == null) return;

            bool valid = IsDropValid(_dragCard, slot);
            slot.SetHighlight(false, valid);

            if (!valid)
            {
                logPanel?.AppendError("BoardFull/InvalidDrop");
                HideGhost();
                _dragCard = null;
                return;
            }

            var cmd = BuildCommandForDrop(_dragCard, slot);
            if (!string.IsNullOrEmpty(cmd.action))
                commandBridge?.Send(cmd);

            HideGhost();
            _dragCard = null;
        }

        public void DropOnHandZone()
        {
            if (_dragCard == null) return;

            if (_dragCard.Source.zone == ZoneType.Shop)
            {
                commandBridge?.Send(new ActionCommand { action = "BUY", shop_slot = _dragCard.Source.index });
            }
            else if (_dragCard.Source.zone == ZoneType.Board)
            {
                commandBridge?.Send(new ActionCommand { action = "MOVE", source_board_slot = _dragCard.Source.index });
            }

            HideGhost();
            _dragCard = null;
        }

        private void ClearHover()
        {
            if (_hoverSlot != null) _hoverSlot.SetHighlight(false, true);
            _hoverSlot = null;
            _hoverHandZone = false;
        }

        private void HideGhost()
        {
            if (dragGhostImage) dragGhostImage.gameObject.SetActive(false);
        }

        private bool IsDropValid(CardView card, CardSlotView target)
        {
            if (card == null || target == null) return false;

            // Hand -> Board: PLAY only if board slot empty
            if (card.Source.zone == ZoneType.Hand && target.SlotRef.zone == ZoneType.Board)
            {
                var state = UIStateBinder.Instance?.Current;
                if (state != null && state.board != null && target.SlotRef.index >= 0 && target.SlotRef.index < state.board.Count)
                {
                    return state.board[target.SlotRef.index] == null;
                }
                return true;
            }

            // Others not allowed here (Shop must drop to Hand zone)
            return false;
        }

        private ActionCommand BuildCommandForDrop(CardView card, CardSlotView target)
        {
            if (card.Source.zone == ZoneType.Hand && target.SlotRef.zone == ZoneType.Board)
            {
                return new ActionCommand
                {
                    action = "PLAY",
                    source_hand_slot = card.Source.index,
                    target_board_slot = target.SlotRef.index
                };
            }

            return new ActionCommand { action = null };
        }
    }
}
