using UnityEngine;
using UnityEngine.EventSystems;

namespace HB.UI
{
    [RequireComponent(typeof(CardSlotView))]
    public class DropTarget : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public bool acceptFromHand = true;
        public bool acceptFromShop = false;
        public bool acceptFromBoard = true;

        private CardSlotView _slot;

        private void Awake() => _slot = GetComponent<CardSlotView>();

        public void OnPointerEnter(PointerEventData eventData) => DragService.Instance?.HoverTarget(_slot, true);
        public void OnPointerExit(PointerEventData eventData) => DragService.Instance?.HoverTarget(_slot, false);
        public void OnDrop(PointerEventData eventData) => DragService.Instance?.DropOn(_slot);

        public bool AllowsSource(ZoneType src)
        {
            return (src == ZoneType.Hand && acceptFromHand)
                || (src == ZoneType.Shop && acceptFromShop)
                || (src == ZoneType.Board && acceptFromBoard);
        }
    }
}
