using UnityEngine;
using UnityEngine.EventSystems;

namespace HB.UI
{
    /// <summary>
    /// Hand area drop zone:
    /// - Shop -> Hand => BUY
    /// - Board -> Hand => MOVE (free)
    /// Doc: Board→Hand is free. fileciteturn7file0
    /// </summary>
    public class HandDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData) => DragService.Instance?.HoverHandZone(true);
        public void OnPointerExit(PointerEventData eventData) => DragService.Instance?.HoverHandZone(false);
        public void OnDrop(PointerEventData eventData) => DragService.Instance?.DropOnHandZone();
    }
}
