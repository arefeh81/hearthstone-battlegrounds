using System.Collections.Generic;
using UnityEngine;

namespace HB.UI
{
    /// <summary>
    /// Recruit screen renderer: Shop/Hand/Board.
    /// Shows gold + frozen state; renders cards into slot containers.
    /// </summary>
    public class RecruitScreen : MonoBehaviour
    {
        public KeywordIconMapper iconMapper;

        [Header("Top HUD")]
        public TMPro.TMP_Text goldText;
        public TMPro.TMP_Text tierText;
        public GameObject frozenBadge;

        [Header("Prefabs")]
        public CardView cardPrefab;

        [Header("Slots")]
        public List<CardSlotView> shopSlots = new();
        public List<CardSlotView> handSlots = new();
        public List<CardSlotView> boardSlots = new();

        private void Awake()
        {
            for (int i = 0; i < shopSlots.Count; i++) shopSlots[i].Bind(ZoneType.Shop, i);
            for (int i = 0; i < handSlots.Count; i++) handSlots[i].Bind(ZoneType.Hand, i);
            for (int i = 0; i < boardSlots.Count; i++) boardSlots[i].Bind(ZoneType.Board, i);
        }

        public void Render(RecruitStateDTO s)
        {
            if (goldText) goldText.text = $"Gold: {s.gold}";
            if (tierText) tierText.text = $"Tavern: {s.tavern_tier}";
            if (frozenBadge) frozenBadge.SetActive(s.shop_frozen);

            RenderIntoSlots(shopSlots, s.shop, ZoneType.Shop);
            RenderIntoSlots(handSlots, s.hand, ZoneType.Hand);
            RenderBoard(boardSlots, s.board);
        }

        private void ClearSlotChildren(CardSlotView slot)
        {
            for (int i = slot.transform.childCount - 1; i >= 0; i--)
                Destroy(slot.transform.GetChild(i).gameObject);
        }

        private void RenderIntoSlots(List<CardSlotView> slots, List<CardDTO> cards, ZoneType zone)
        {
            for (int i = 0; i < slots.Count; i++) ClearSlotChildren(slots[i]);

            for (int i = 0; i < Mathf.Min(slots.Count, cards.Count); i++)
            {
                var dto = cards[i];
                if (dto == null) continue;

                var cv = Instantiate(cardPrefab, slots[i].transform);
                cv.Bind(dto, new ZoneRef { zone = zone, index = i }, iconMapper);
            }
        }

        private void RenderBoard(List<CardSlotView> slots, List<CardDTO> board)
        {
            for (int i = 0; i < slots.Count; i++) ClearSlotChildren(slots[i]);

            for (int i = 0; i < Mathf.Min(slots.Count, board.Count); i++)
            {
                var dto = board[i];
                if (dto == null) continue;

                var cv = Instantiate(cardPrefab, slots[i].transform);
                cv.Bind(dto, new ZoneRef { zone = ZoneType.Board, index = i }, iconMapper);
            }
        }
    }
}
